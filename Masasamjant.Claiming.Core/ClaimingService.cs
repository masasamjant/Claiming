namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents abstract service that supports claiming.
    /// </summary>
    public abstract class ClaimingService : IClaimingService
    {
        /// <summary>
        /// Initializes new instance of the <see cref="ClaimingService"/> class.
        /// </summary>
        /// <param name="claimManager">The <see cref="IClaimManager"/>.</param>
        protected ClaimingService(IClaimManager claimManager) 
        {
            ClaimManager = claimManager;
        }

        /// <summary>
        /// Gets the <see cref="IClaimManager"/>.
        /// </summary>
        protected IClaimManager ClaimManager;

        /// <summary>
        /// Tries to claim specified <see cref="IClaimable"/> to user specified by <paramref name="ownerIdentifier"/>.
        /// </summary>
        /// <param name="claimable">The <see cref="IClaimable"/> to attempt to claim.</param>
        /// <param name="ownerIdentifier">The unique identifier or claim owner.</param>
        /// <returns>A <see cref="IClaimDescriptor"/> to descibe the result.</returns>
        public Task<IClaimDescriptor> TryClaimAsync(IClaimable claimable, string ownerIdentifier)
        {
            return TryClaimAsync(claimable.GetClaimKey(), ownerIdentifier);
        }

        /// <summary>
        /// Tries to claim instance specified by <see cref="ClaimKey"/> to user specified by <paramref name="ownerIdentifier"/>.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> of instance to claim.</param>
        /// <param name="ownerIdentifier">The unique identifier or claim owner.</param>
        /// <returns>A <see cref="IClaimDescriptor"/> to descibe the result.</returns>
        public async Task<IClaimDescriptor> TryClaimAsync(ClaimKey claimKey, string ownerIdentifier)
        {
            var request = new ClaimRequest(claimKey, ownerIdentifier, ClaimManager.ClaimLifeTimeMinutes);
            var response = await ClaimManager.TryClaimAsync(request);
            if (response.Result == ClaimResult.NotFound)
                return new ClaimDescriptor(ClaimResult.NotFound, null);
            return new ClaimDescriptor(response.Result, response.Claim);
        }

        /// <summary>
        /// Gets the <see cref="IClaimDescriptor"/> of claim specified by <see cref="ClaimKey"/>. If claim exist and specified claim 
        /// owner is the actual claim owner then descriptor decribes approved claim; otherwise describes denied claim.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> of the claimed instance.</param>
        /// <param name="ownerIdentifier">The expected claim owner.</param>
        /// <returns>A <see cref="IClaimDescriptor"/>s of claims specified by <paramref name="claimKey"/>.</returns>
        public async Task<IClaimDescriptor> GetClaimAsync(ClaimKey claimKey, string ownerIdentifier)
        {
            var claim = await ClaimManager.GetClaimAsync(claimKey);
            if (claim == null)
                return new ClaimDescriptor(ClaimResult.NotFound, null);
            var claimResult = claim.OwnerIdentifier.Equals(ownerIdentifier) ? ClaimResult.Approved : ClaimResult.Denied;
            return new ClaimDescriptor(claimResult, claim);
        }

        /// <summary>
        /// Gets all describtors of all claims or alternatively claims owned by owner specified by <paramref name="ownerIdentifier"/>.
        /// </summary>
        /// <param name="ownerIdentifier">The expected claim owner.</param>
        /// <param name="onlyOwnedClaims"><c>true</c> to get only claims owned by user specified by <paramref name="ownerIdentifier"/>; <c>false</c> otherwise.</param>
        /// <returns>A <see cref="IClaimDescriptor"/>s of all claims or claims owner by <paramref name="ownerIdentifier"/>.</returns>
        public async Task<IEnumerable<IClaimDescriptor>> GetClaimsAsync(string ownerIdentifier, bool onlyOwnedClaims)
        {
            var claims = onlyOwnedClaims
                ? (await ClaimManager.GetClaimsAsync()).Where(claim => claim.OwnerIdentifier.Equals(ownerIdentifier)).ToList()
                : (await ClaimManager.GetClaimsAsync()).ToList();

            var descriptors = new List<ClaimDescriptor>();

            foreach (var claim in claims)
            {
                var claimResult = claim.OwnerIdentifier.Equals(ownerIdentifier) ? ClaimResult.Approved : ClaimResult.Denied;
                descriptors.Add(new ClaimDescriptor(claimResult, claim));
            }

            return descriptors;
        }

        /// <summary>
        /// Release claim described be specified <see cref="IClaimDescriptor"/>.
        /// </summary>
        /// <param name="claimDescriptor">The <see cref="IClaimDescriptor"/> of claim to release.</param>
        /// <returns><c>true</c> if claim was still valid and released; <c>false</c> otherwise.</returns>
        public async Task<bool> ReleaseClaimAsync(IClaimDescriptor claimDescriptor)
        {
            if (!claimDescriptor.IsClaimed || !claimDescriptor.IsApproved)
                return false;

            if (claimDescriptor.Claim == null)
                return false;

            return await ClaimManager.ReleaseClaimAsync(claimDescriptor.Claim);
        }
    }
}
