using Masasamjant.Claiming.Abstractions;

namespace Masasamjant.Claiming.Memory
{
    /// <summary>
    /// Represents <see cref="ClaimStorage"/> that stores claims in memory using dictionary.
    /// </summary>
    /// <remarks>Instance of <see cref="MemoryClaimStorage"/> is thread-safe.</remarks>
    public sealed class MemoryClaimStorage : ClaimStorage
    {
        private readonly Dictionary<ClaimKey, Claim> claims;
        private readonly HashSet<Guid> lookup;
        private readonly object mutex;

        /// <summary>
        /// Initializes new instance of the <see cref="MemoryClaimStorage"/> class.
        /// </summary>
        public MemoryClaimStorage() 
        {
            mutex = new object();
            claims = new Dictionary<ClaimKey, Claim>();
            lookup = new HashSet<Guid>();   
        }

        /// <summary>
        /// Gets <see cref="Claim"/> of object instance specified by <see cref="ClaimKey"/>.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> to specify object instance.</param>
        /// <returns>A <see cref="Claim"/> or <c>null</c>, if claim not exist.</returns>
        /// <exception cref="ClaimStorageException">If exception occurs.</exception>
        public override Task<Claim?> GetClaimAsync(ClaimKey claimKey)
        {
            try
            {
                if (!claimKey.IsEmpty)
                {
                    lock (mutex)
                    {
                        if (claims.TryGetValue(claimKey, out Claim? claim))
                            return Task.FromResult<Claim?>(claim);
                    }
                }

                return Task.FromResult<Claim?>(null);
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
        public override Task<Claim?> GetClaimAsync(Guid claimIdentifier, string? ownerIdentifier)
        {
            try
            {
                lock (mutex)
                {
                    if (lookup.Contains(claimIdentifier))
                    {
                        var claim = claims.Values.FirstOrDefault(c => c.ClaimIdentifier == claimIdentifier);

                        if (claim != null)
                        {
                            if (!string.IsNullOrEmpty(ownerIdentifier) && !claim.OwnerIdentifier.Equals(ownerIdentifier))
                                return Task.FromResult<Claim?>(null);

                            return Task.FromResult<Claim?>(claim);
                        }
                    }
                }

                return Task.FromResult<Claim?>(null);
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
        public override Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            try
            {
                var claimList = new List<Claim>();

                lock (mutex)
                {
                    if (lookup.Count > 0)
                    {
                        foreach (var claim in claims.Values)
                            claimList.Add(claim);
                    }

                    return Task.FromResult<IEnumerable<Claim>>(claimList.AsReadOnly());
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
        public override Task<bool> IsClaimedAsync(ClaimKey claimKey)
        {
            try
            {
                bool result = false;

                if (!claimKey.IsEmpty)
                {
                    lock (mutex)
                    {
                        result = claims.ContainsKey(claimKey);
                    }
                }

                return Task.FromResult(result);
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
        public override Task<bool> ReleaseClaimAsync(Claim claim)
        {
            try
            {
                bool result = false;

                lock (mutex)
                {
                    if (lookup.Contains(claim.ClaimIdentifier))
                    {
                        if (claims.TryGetValue(claim.ClaimKey, out Claim? currentClaim))
                        {
                            if (currentClaim.ClaimIdentifier == claim.ClaimIdentifier &&
                                currentClaim.OwnerIdentifier == claim.OwnerIdentifier &&
                                currentClaim.ClaimKey.Equals(claim.ClaimKey))
                            {
                                claims.Remove(claim.ClaimKey);
                                lookup.Remove(claim.ClaimIdentifier);
                                result = true;
                            }
                        }
                    }
                }

                return Task.FromResult(result);
            }
            catch (Exception exception)
            {
                throw new ClaimStorageException("Unexpected exception when trying to release specified claim.", claim, exception);
            }
        }

        /// <summary>
        /// Try claim object instance to owner using specified <see cref="ClaimRequest"/>.
        /// </summary>
        /// <param name="request">The <see cref="ClaimRequest"/>.</param>
        /// <returns>A <see cref="ClaimResponse"/>.</returns>
        /// <exception cref="ClaimStorageException">If exception occurs.</exception>
        public override Task<ClaimResponse> TryGetClaimAsync(ClaimRequest request)
        {
            try
            {
                lock (mutex)
                {
                    ClaimResponse response;

                    if (claims.TryGetValue(request.ClaimKey, out Claim? claim))
                    {
                        // If claim is expired, then remove it and will create new one.
                        if (claim.IsExpired())
                        {
                            claims.Remove(request.ClaimKey);
                            lookup.Remove(claim.ClaimIdentifier);
                        }
                        else
                        {
                            // Otherwise create response from current claim.
                            if (claim.OwnerIdentifier.Equals(request.OwnerIdentifier))
                                response = ClaimResponse.Approved(claim);
                            else
                                response = ClaimResponse.Denied(claim);

                            return Task.FromResult(response);
                        }
                    }

                    var expires = DateTimeOffset.Now.AddMinutes(request.LifeTimeMinutes);
                    claim = new Claim(Guid.NewGuid(), request.OwnerIdentifier, request.ClaimKey, expires);
                    claims[request.ClaimKey] = claim;
                    lookup.Add(claim.ClaimIdentifier);
                    response = ClaimResponse.Approved(claim);
                    return Task.FromResult(response);
                }
            }
            catch (Exception exception)
            {
                throw new ClaimStorageException("Unexpected exception when attempt to claim using specified claim key.", request.ClaimKey, exception);
            }
        }
    }
}
