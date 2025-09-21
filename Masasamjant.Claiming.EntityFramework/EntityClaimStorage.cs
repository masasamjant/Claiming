using Masasamjant.Claiming.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents <see cref="ClaimStorage"/> that stores claims in SQL server database using Entity Framework.
    /// </summary>
    /// <remarks>Instance of <see cref="EntityClaimStorage"/> is thread-safe.</remarks>
    public sealed class EntityClaimStorage : ClaimStorage, IDisposable
    {
        private readonly IConfiguration configuration;
        private readonly IEntityClaimRepositoryFactory repositoryFactory;
        private readonly SemaphoreSlim semaphore;
        private long disposed = 0L;

        /// <summary>
        /// Initializes new instance of the <see cref="EntityClaimStorage"/> class.
        /// </summary>
        /// <param name="repositoryFactory">The <see cref="IEntityClaimRepositoryFactory"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        public EntityClaimStorage(IEntityClaimRepositoryFactory repositoryFactory, IConfiguration configuration)
        {
            this.repositoryFactory = repositoryFactory;
            this.configuration = configuration;
            this.semaphore = new SemaphoreSlim(1, 1);
        }

        /// <summary>
        /// Finalizes current instance.
        /// </summary>
        ~EntityClaimStorage()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets <see cref="Claim"/> of object instance specified by <see cref="ClaimKey"/>.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> to specify object instance.</param>
        /// <returns>A <see cref="Claim"/> or <c>null</c>, if claim not exist.</returns>
        /// <exception cref="ClaimStorageException">If exception occurs.</exception>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public override async Task<Claim?> GetClaimAsync(ClaimKey claimKey)
        {
            CheckDisposed();

            if (claimKey.IsEmpty)
                return null;

            try
            {
                using (var repository = CreateRepository())
                {
                    var claim = await repository.GetClaimAsync(claimKey);

                    if (claim != null)
                        claim.Prepare();

                    return claim;
                }
            }
            catch (Exception exception)
            {
                throw new ClaimStorageException("Unexpected exception when getting claim specified by claim key.", claimKey, exception);
            }
        }

        /// <summary>
        /// Gets <see cref="Claim"/> specified by claim identifier. If claim owner is specified, 
        /// then claim must be owner by specified owner or <c>null</c> will be returned even if claim exist.
        /// </summary>
        /// <param name="claimIdentifier">The claim identifier.</param>
        /// <param name="ownerIdentifier">The optional identifier of claim owner.</param>
        /// <returns>A <see cref="Claim"/> or <c>null</c>, if claim not exist.</returns>
        /// <exception cref="ClaimStorageException">If exception occurs.</exception>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public override async Task<Claim?> GetClaimAsync(Guid claimIdentifier, string? ownerIdentifier)
        {
            CheckDisposed();

            try
            {
                using (var repository = CreateRepository())
                {
                    var claim = await repository.GetClaimAsync(claimIdentifier);

                    if (claim == null)
                        return null;

                    if (!string.IsNullOrEmpty(ownerIdentifier) && !claim.OwnerIdentifier.Equals(ownerIdentifier))
                        return null;

                    claim.Prepare();

                    return claim;
                }
            }
            catch (Exception exception)
            {
                throw new ClaimStorageException($"Unexpected exception when getting claim of '{claimIdentifier}'.", exception);
            }
        }

        /// <summary>
        /// Gets all claims.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{Claim}"/> of all claims.</returns>
        /// <exception cref="ClaimStorageException">If exception occurs.</exception>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public override async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            CheckDisposed();

            try
            {
                using (var repository = CreateRepository())
                {
                    var claims = await repository.GetClaimsAsync();

                    foreach (var claim in claims)
                        claim.Prepare();

                    return claims;
                }
            }
            catch (Exception exception)
            {
                throw new ClaimStorageException("Unexpected exception when getting claims.", exception);
            }
        }

        /// <summary>
        /// Check if object instance specified by <see cref="ClaimKey"/> is claimed.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> to specify object instance.</param>
        /// <returns><c>true</c> if object instance specified by <paramref name="claimKey"/> is claimed; <c>false</c> otherwise.</returns>
        /// <exception cref="ClaimStorageException">If exception occurs.</exception>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public override async Task<bool> IsClaimedAsync(ClaimKey claimKey)
        {
            CheckDisposed();

            try
            {
                using (var repository = CreateRepository())
                {
                    return await repository.ExistsAsync(claimKey);
                }
            }
            catch (Exception exception)
            {
                throw new ClaimStorageException("Unexpected exception when attempt to check status of claim specified by claim key.", claimKey, exception);
            }
        }

        /// <summary>
        /// Releases specified <see cref="Claim"/>.
        /// </summary>
        /// <param name="claim">The <see cref="Claim"/> to release.</param>
        /// <returns><c>true</c> if claim was still valid and released; <c>false</c> otherwise.</returns>
        /// <exception cref="ClaimStorageException">If exception occurs.</exception>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public override async Task<bool> ReleaseClaimAsync(Claim claim)
        {
            CheckDisposed();

            try
            {
                await semaphore.WaitAsync();

                using (var repository = CreateRepository()) 
                {
                    var currentClaim = await repository.GetClaimAsync(claim);

                    if (currentClaim == null)
                        return false;

                    await repository.RemoveClaimAsync(currentClaim);

                    return true;
                }
            }
            catch (Exception exception)
            {
                throw new ClaimStorageException("Unexpected exception when trying to release specified claim.", claim, exception);
            }
            finally
            { 
                semaphore.Release();
            }
        }

        /// <summary>
        /// Try claim object instance to owner using specified <see cref="ClaimRequest"/>.
        /// </summary>
        /// <param name="request">The <see cref="ClaimRequest"/>.</param>
        /// <returns>A <see cref="ClaimResponse"/>.</returns>
        /// <exception cref="ClaimStorageException">If exception occurs.</exception>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public override async Task<ClaimResponse> TryGetClaimAsync(ClaimRequest request)
        {
            CheckDisposed();

            try
            {
                await semaphore.WaitAsync();

                using (var repository = CreateRepository())
                {
                    var claim = await repository.GetClaimAsync(request.ClaimKey);

                    if (claim != null)
                    {
                        claim.Prepare();

                        // If claim is expired, then remove it and will create new one.
                        if (claim.IsExpired())
                        {
                            await repository.RemoveClaimAsync(claim);
                        }
                        else
                        {
                            // Otherwise create response from current claim.
                            if (claim.OwnerIdentifier == request.OwnerIdentifier)
                                return new ClaimResponse(ClaimResult.Approved, claim);
                            else
                                return new ClaimResponse(ClaimResult.Denied, claim);
                        }
                    }

                    // Create new claim.
                    var expires = GetDateTime(request.DateTimeKind).AddMinutes(request.LifeTimeMinutes);
                    claim = new EntityClaim(Guid.NewGuid(), request.OwnerIdentifier, request.ClaimKey, expires);
                    claim = await repository.AddClaimAsync(claim);
                    claim.Prepare();
                    return new ClaimResponse(ClaimResult.Approved, claim);
                }
            }
            catch (Exception exception)
            {
                throw new ClaimStorageException("Unexpected exception when attempt to claim using specified claim key.", request.ClaimKey, exception);
            }
            finally
            { 
                semaphore.Release();
            }
        }

        /// <summary>
        /// Disposes current instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void CheckDisposed()
        {
            if (Interlocked.Read(ref disposed) == 1L)
                throw new ObjectDisposedException(GetType().Name);
        }

        private void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref disposed, 1L) == 0L)
            {
                semaphore.Dispose();
            }
        }

        private IEntityClaimRepository CreateRepository()
            => repositoryFactory.CreateRepository(configuration);
    }
}
