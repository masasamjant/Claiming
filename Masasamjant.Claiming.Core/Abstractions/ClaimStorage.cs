namespace Masasamjant.Claiming.Abstractions
{
    /// <summary>
    /// Represents abstract storage of claims.
    /// </summary>
    public abstract class ClaimStorage
    {
        /// <summary>
        /// Gets <see cref="Claim"/> of object instance specified by <see cref="ClaimKey"/>.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> to specify object instance.</param>
        /// <returns>A <see cref="Claim"/> or <c>null</c>, if claim not exist.</returns>
        public abstract Task<Claim?> GetClaimAsync(ClaimKey claimKey);

        /// <summary>
        /// Gets <see cref="Claim"/> specified by claim identifier. If claim owner is specified, 
        /// then claim must be owner by specified owner or <c>null</c> will be returned even if claim exist.
        /// </summary>
        /// <param name="claimIdentifier">The claim identifier.</param>
        /// <param name="ownerIdentifier">The optional identifier of claim owner.</param>
        /// <returns>A <see cref="Claim"/> or <c>null</c>, if claim not exist.</returns>
        public abstract Task<Claim?> GetClaimAsync(Guid claimIdentifier, string? ownerIdentifier);

        /// <summary>
        /// Gets all claims.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{Claim}"/> of all claims.</returns>
        public abstract Task<IEnumerable<Claim>> GetClaimsAsync();

        /// <summary>
        /// Check if object instance specified by <see cref="ClaimKey"/> is claimed.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> to specify object instance.</param>
        /// <returns><c>true</c> if object instance specified by <paramref name="claimKey"/> is claimed; <c>false</c> otherwise.</returns>
        public abstract Task<bool> IsClaimedAsync(ClaimKey claimKey);

        /// <summary>
        /// Releases specified <see cref="Claim"/>.
        /// </summary>
        /// <param name="claim">The <see cref="Claim"/> to release.</param>
        /// <returns><c>true</c> if claim was still valid and released; <c>false</c> otherwise.</returns>
        public abstract Task<bool> ReleaseClaimAsync(Claim claim);

        /// <summary>
        /// Try claim object instance to owner using specified <see cref="ClaimRequest"/>.
        /// </summary>
        /// <param name="request">The <see cref="ClaimRequest"/>.</param>
        /// <returns>A <see cref="ClaimResponse"/>.</returns>
        public abstract Task<ClaimResponse> TryGetClaimAsync(ClaimRequest request);
    }
}
