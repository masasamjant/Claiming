namespace Masasamjant.Claiming.Abstractions
{
    /// <summary>
    /// Represents repository to access entity claims stored to database using Entity Framework.
    /// </summary>
    public interface IEntityClaimRepository : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="EntityClaim"/> specified by claim key.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> of the claim.</param>
        /// <returns>A <see cref="EntityClaim"/> or <c>null</c>, if not exists.</returns>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        Task<EntityClaim?> GetClaimAsync(ClaimKey claimKey);

        /// <summary>
        /// Gets the <see cref="EntityClaim"/> specified by claim identifier.
        /// </summary>
        /// <param name="claimIdentifier">The claim identifier.</param>
        /// <returns>A <see cref="EntityClaim"/> or <c>null</c>, if not exists.</returns>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        Task<EntityClaim?> GetClaimAsync(Guid claimIdentifier);

        /// <summary>
        /// Gets the <see cref="EntityClaim"/> specified by other claim.
        /// </summary>
        /// <param name="other">The other claim.</param>
        /// <returns>A <see cref="EntityClaim"/> or <c>null</c>, if not exists.</returns>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception> 
        Task<EntityClaim?> GetClaimAsync(Claim other);

        /// <summary>
        /// Gets all claims stored.
        /// </summary>
        /// <returns>A read-only collection of current claims.</returns>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        Task<IReadOnlyCollection<EntityClaim>> GetClaimsAsync();

        /// <summary>
        /// Check if claim exists that use specified claim key.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> of the claim.</param>
        /// <returns><c>true</c> if claim exist; <c>false</c> otherwise.</returns>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        Task<bool> ExistsAsync(ClaimKey claimKey);

        /// <summary>
        /// Delete specified <see cref="EntityClaim"/> permanently from database.
        /// </summary>
        /// <param name="claim">The <see cref="EntityClaim"/> to delete.</param>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        Task RemoveClaimAsync(EntityClaim claim);

        /// <summary>
        /// Insert specified <see cref="EntityClaim"/> to database.
        /// </summary>
        /// <param name="claim">The <see cref="EntityClaim"/> to insert.</param>
        /// <returns>A <see cref="EntityClaim"/> inserted.</returns>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        Task<EntityClaim> AddClaimAsync(EntityClaim claim);
    }
}
