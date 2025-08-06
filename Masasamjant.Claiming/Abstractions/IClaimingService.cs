namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents service that supports claiming.
    /// </summary>
    public interface IClaimingService
    {
        /// <summary>
        /// Tries to claim specified <see cref="IClaimable"/> to user specified by <paramref name="ownerIdentifier"/>.
        /// </summary>
        /// <param name="claimable">The <see cref="IClaimable"/> to attempt to claim.</param>
        /// <param name="ownerIdentifier">The unique identifier or claim owner.</param>
        /// <returns>A <see cref="IClaimDescriptor"/> to descibe the result.</returns>
        Task<IClaimDescriptor> TryClaimAsync(IClaimable claimable, string ownerIdentifier);

        /// <summary>
        /// Tries to claim instance specified by <see cref="ClaimKey"/> to user specified by <paramref name="ownerIdentifier"/>.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> of instance to claim.</param>
        /// <param name="ownerIdentifier">The unique identifier or claim owner.</param>
        /// <returns>A <see cref="IClaimDescriptor"/> to descibe the result.</returns>
        Task<IClaimDescriptor> TryClaimAsync(ClaimKey claimKey, string ownerIdentifier);

        /// <summary>
        /// Gets the <see cref="IClaimDescriptor"/> of claim specified by <see cref="ClaimKey"/>. If claim exist and specified claim 
        /// owner is the actual claim owner then descriptor decribes approved claim; otherwise describes denied claim.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> of the claimed instance.</param>
        /// <param name="ownerIdentifier">The expected claim owner.</param>
        /// <returns>A <see cref="IClaimDescriptor"/>s of claims specified by <paramref name="claimKey"/>.</returns>
        Task<IClaimDescriptor> GetClaimAsync(ClaimKey claimKey, string ownerIdentifier);

        /// <summary>
        /// Gets all describtors of all claims or alternatively claims owned by owner specified by <paramref name="ownerIdentifier"/>.
        /// </summary>
        /// <param name="ownerIdentifier">The expected claim owner.</param>
        /// <param name="onlyOwnedClaims"><c>true</c> to get only claims owned by user specified by <paramref name="ownerIdentifier"/>; <c>false</c> otherwise.</param>
        /// <returns>A <see cref="IClaimDescriptor"/>s of all claims or claims owner by <paramref name="ownerIdentifier"/>.</returns>
        Task<IEnumerable<IClaimDescriptor>> GetClaimsAsync(string ownerIdentifier, bool onlyOwnedClaims);

        /// <summary>
        /// Release claim described be specified <see cref="IClaimDescriptor"/>.
        /// </summary>
        /// <param name="claimDescriptor">The <see cref="IClaimDescriptor"/> of claim to release.</param>
        /// <returns><c>true</c> if claim was still valid and released; <c>false</c> otherwise.</returns>
        Task<bool> ReleaseClaimAsync(IClaimDescriptor claimDescriptor);
    }
}
