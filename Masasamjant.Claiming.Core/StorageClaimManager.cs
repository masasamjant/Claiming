﻿using Masasamjant.Claiming.Abstractions;

namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents manager of claims that use <see cref="ClaimStorage"/> to store claims.
    /// </summary>
    public class StorageClaimManager : ClaimManager
    {
        private readonly ClaimStorage storage;

        /// <summary>
        /// Initializes new instance of the <see cref="StorageClaimManager"/> class.
        /// </summary>
        /// <param name="storage">The <see cref="ClaimStorage"/>.</param>
        public StorageClaimManager(ClaimStorage storage)
        {
            this.storage = storage;
        }

        /// <summary>
        /// Gets <see cref="IClaim"/> specified by claim identifier. If claim owner is specified, 
        /// then claim must be owner by specified owner or <c>null</c> will be returned even if claim exist.
        /// </summary>
        /// <param name="claimIdentifier">The claim identifier.</param>
        /// <param name="ownerIdentifier">The optional identifier of claim owner.</param>
        /// <returns>A <see cref="IClaim"/> or <c>null</c>, if claim not exist.</returns>
        public override async Task<IClaim?> GetClaimAsync(Guid claimIdentifier, string? ownerIdentifier)
        {
            try
            {
                if (claimIdentifier.Equals(Guid.Empty))
                    return null;

                return await storage.GetClaimAsync(claimIdentifier, ownerIdentifier);
            }
            catch (Exception exception)
            {
                if (exception is ClaimStorageException)
                    throw;

                throw new ClaimException($"Unexpected exception when getting claim of '{claimIdentifier}'", exception);
            }
        }

        /// <summary>
        /// Gets <see cref="IClaim"/> of object instance specified by <see cref="ClaimKey"/>.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> to specify object instance.</param>
        /// <returns>A <see cref="IClaim"/> or <c>null</c>, if claim not exist.</returns>
        public override async Task<IClaim?> GetClaimAsync(ClaimKey claimKey)
        {
            try
            {
                if (claimKey.IsEmpty)
                    return null;

                return await storage.GetClaimAsync(claimKey);
            }
            catch (Exception exception)
            {
                if (exception is ClaimStorageException)
                    throw;

                throw new ClaimException("Unexpected exception when getting claim using specified claim key.", claimKey, exception);
            }
        }

        /// <summary>
        /// Gets all claims.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{IClaim}"/> of all claims.</returns>
        public override async Task<IEnumerable<IClaim>> GetClaimsAsync()
        {
            try
            {
                return await storage.GetClaimsAsync();
            }
            catch (Exception exception)
            {
                if (exception is ClaimStorageException)
                    throw;

                throw new ClaimException("Unexpected exception when getting claims.", exception);
            }
        }

        /// <summary>
        /// Check if object instance specified by <see cref="ClaimKey"/> is claimed.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> to specify object instance.</param>
        /// <returns><c>true</c> if object instance specified by <paramref name="claimKey"/> is claimed; <c>false</c> otherwise.</returns>
        public override async Task<bool> IsClaimedAsync(ClaimKey claimKey)
        {
            try
            {
                if (claimKey.IsEmpty)
                    return false;

                return await storage.IsClaimedAsync(claimKey);
            }
            catch (Exception exception)
            {
                if (exception is ClaimStorageException)
                    throw;

                throw new ClaimException("Unexpected exception when using specified claim key.", claimKey, exception);
            }
        }

        /// <summary>
        /// Releases specified <see cref="IClaim"/>.
        /// </summary>
        /// <param name="claim">The <see cref="IClaim"/> to release.</param>
        /// <returns><c>true</c> if claim was still valid and released; <c>false</c> otherwise.</returns>
        public override async Task<bool> ReleaseClaimAsync(IClaim claim)
        {
            try
            {
                if (claim.IsEmpty)
                    return false;

                return await storage.ReleaseClaimAsync(GetClaim(claim));
            }
            catch (Exception exception)
            {
                if (exception is ClaimStorageException)
                    throw;

                throw new ClaimException("Unexpected exception when releasing specified claim.", claim, exception);
            }
        }

        /// <summary>
        /// Try claim object instance to owner using specified <see cref="IClaimRequest"/>.
        /// </summary>
        /// <param name="request">The <see cref="IClaimRequest"/>.</param>
        /// <returns>A <see cref="IClaimResponse"/>.</returns>
        public override async Task<IClaimResponse> TryClaimAsync(IClaimRequest request)
        {
            try
            {
                return await storage.TryGetClaimAsync(GetClaimRequest(request));
            }
            catch (Exception exception)
            {
                if (exception is ClaimStorageException)
                    throw;

                throw new ClaimException("Unexpected exception when attempt to claim using specified claim key.", request.ClaimKey, exception);
            }
        }

        /// <summary>
        /// Creates <see cref="Claim"/> suitable for <see cref="ClaimStorage"/> from specified <see cref="IClaim"/>.
        /// </summary>
        /// <param name="claim">The <see cref="IClaim"/>.</param>
        /// <returns>A <see cref="Claim"/> suitable for <see cref="ClaimStorage"/>.</returns>
        protected virtual Claim GetClaim(IClaim claim)
        {
            if (claim is Claim c)
                return c;

            return new Claim(claim.ClaimIdentifier, claim.OwnerIdentifier, claim.ClaimKey, claim.ExpiresAt);
        }

        /// <summary>
        /// Creates <see cref="ClaimRequest"/> suitable for <see cref="ClaimStorage"/> from specified <see cref="IClaimRequest"/>.
        /// </summary>
        /// <param name="request"><see cref="IClaimRequest"/>.</param>
        /// <returns>A <see cref="ClaimRequest"/> suitable for <see cref="ClaimStorage"/>.</returns>
        protected virtual ClaimRequest GetClaimRequest(IClaimRequest request)
        {
            if (request is ClaimRequest claimRequest)
                return claimRequest;

            return new ClaimRequest(request.ClaimKey, request.OwnerIdentifier, request.LifeTimeMinutes);
        }
    }
}
