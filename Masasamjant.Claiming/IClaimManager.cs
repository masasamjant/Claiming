namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents manager of claims.
    /// </summary>
    public interface IClaimManager
    {
        /// <summary>
        /// Gets the claim life time in minutes. 
        /// Default value is <see cref="Claims.DefaultClaimLifeTimeMinutes"/>.
        /// </summary>
        int ClaimLifeTimeMinutes { get; }

        /// <summary>
        /// Gets <see cref="IClaim"/> specified by claim identifier. If claim owner is specified, 
        /// then claim must be owner by specified owner or <c>null</c> will be returned even if claim exist.
        /// </summary>
        /// <param name="claimIdentifier">The claim identifier.</param>
        /// <param name="ownerIdentifier">The optional identifier of claim owner.</param>
        /// <returns>A <see cref="IClaim"/> or <c>null</c>, if claim not exist.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        Task<Claim?> GetClaimAsync(Guid claimIdentifier, string? ownerIdentifier);

        /// <summary>
        /// Gets <see cref="IClaim"/> of object instance specified by <see cref="ClaimKey"/>.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> to specify object instance.</param>
        /// <returns>A <see cref="IClaim"/> or <c>null</c>, if claim not exist.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        Task<Claim?> GetClaimAsync(ClaimKey claimKey);

        /// <summary>
        /// Gets all claims.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{IClaim}"/> of all claims.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        Task<IEnumerable<Claim>> GetClaimsAsync();

        /// <summary>
        /// Check if object instance specified by <see cref="ClaimKey"/> is claimed.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> to specify object instance.</param>
        /// <returns><c>true</c> if object instance specified by <paramref name="claimKey"/> is claimed; <c>false</c> otherwise.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        Task<bool> IsClaimedAsync(ClaimKey claimKey);

        /// <summary>
        /// Releases specified <see cref="IClaim"/>.
        /// </summary>
        /// <param name="claim">The <see cref="IClaim"/> to release.</param>
        /// <returns><c>true</c> if claim was still valid and released; <c>false</c> otherwise.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        Task<bool> ReleaseClaimAsync(Claim claim);

        /// <summary>
        /// Try claim object instance to owner using specified <see cref="IClaimRequest"/>.
        /// </summary>
        /// <param name="request">The <see cref="IClaimRequest"/>.</param>
        /// <returns>A <see cref="IClaimResponse"/>.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        Task<ClaimResponse> TryClaimAsync(ClaimRequest request);

        /// <summary>
        /// Changes <see cref="ClaimLifeTimeMinutes"/>.
        /// </summary>
        /// <param name="minutes">The claim life time in minutes. Must be greater than 0 minutes.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="minutes"/> is less than 1 minute.</exception>
        /// <exception cref="InvalidOperationException">If fails to set <see cref="ClaimLifeTimeMinutes"/>.</exception>
        void UseClaimLifeTime(int minutes);
    }
}
