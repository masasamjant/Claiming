namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents abstract manager of claims.
    /// </summary>
    public abstract class ClaimManager : IClaimManager
    {
        private int claimLifeTimeMinutes = Claims.DefaultClaimLifeTimeMinutes;

        /// <summary>
        /// Gets the claim life time in minutes. 
        /// Default value is <see cref="Claims.DefaultClaimLifeTimeMinutes"/>.
        /// </summary>
        public virtual int ClaimLifeTimeMinutes
        {
            get { return claimLifeTimeMinutes; }
        }

        /// <summary>
        /// Gets <see cref="Claim"/> specified by claim identifier. If claim owner is specified, 
        /// then claim must be owner by specified owner or <c>null</c> will be returned even if claim exist.
        /// </summary>
        /// <param name="claimIdentifier">The claim identifier.</param>
        /// <param name="ownerIdentifier">The optional identifier of claim owner.</param>
        /// <returns>A <see cref="Claim"/> or <c>null</c>, if claim not exist.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        public abstract Task<Claim?> GetClaimAsync(Guid claimIdentifier, string? ownerIdentifier);

        /// <summary>
        /// Gets <see cref="Claim"/> of object instance specified by <see cref="ClaimKey"/>.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> to specify object instance.</param>
        /// <returns>A <see cref="Claim"/> or <c>null</c>, if claim not exist.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        public abstract Task<Claim?> GetClaimAsync(ClaimKey claimKey);

        /// <summary>
        /// Gets all claims.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{Claim}"/> of all claims.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        public abstract Task<IEnumerable<Claim>> GetClaimsAsync();

        /// <summary>
        /// Check if object instance specified by <see cref="ClaimKey"/> is claimed.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> to specify object instance.</param>
        /// <returns><c>true</c> if object instance specified by <paramref name="claimKey"/> is claimed; <c>false</c> otherwise.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        public abstract Task<bool> IsClaimedAsync(ClaimKey claimKey);

        /// <summary>
        /// Releases specified <see cref="Claim"/>.
        /// </summary>
        /// <param name="claim">The <see cref="Claim"/> to release.</param>
        /// <returns><c>true</c> if claim was still valid and released; <c>false</c> otherwise.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        public abstract Task<bool> ReleaseClaimAsync(Claim claim);

        /// <summary>
        /// Try claim object instance to owner using specified <see cref="ClaimRequest"/>.
        /// </summary>
        /// <param name="request">The <see cref="ClaimRequest"/>.</param>
        /// <returns>A <see cref="ClaimResponse"/>.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        public abstract Task<ClaimResponse> TryClaimAsync(ClaimRequest request);

        /// <summary>
        /// Changes <see cref="ClaimLifeTimeMinutes"/>.
        /// </summary>
        /// <param name="minutes">The claim life time in minutes. Must be greater than 0 minutes.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="minutes"/> is less than 1 minute.</exception>
        /// <exception cref="InvalidOperationException">If fails to set <see cref="ClaimLifeTimeMinutes"/>.</exception>
        public virtual void UseClaimLifeTime(int minutes)
        {
            if (minutes < 1)
                throw new ArgumentOutOfRangeException(nameof(minutes), minutes, "The claim life time in minutes must be greater than 0 minutes.");

            claimLifeTimeMinutes = minutes;
        }

        /// <summary>
        /// Validates that checksum of <see cref="ClaimRequest"/> is expected.
        /// </summary>
        /// <param name="request">The <see cref="ClaimRequest"/>.</param>
        /// <exception cref="ClaimException">If the checksum does not match.</exception>
        protected static void ValidateRequestChecksum(ClaimRequest request)
        {
            var checksum = ClaimRequestHelper.ComputeChecksum(request);

            if (!string.Equals(checksum, request.Checksum, StringComparison.Ordinal))
                throw new ClaimException("Request checksum does not match.", request.ClaimKey);
        }
    }
}
