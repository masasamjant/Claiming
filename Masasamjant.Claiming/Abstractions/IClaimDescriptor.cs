namespace Masasamjant.Claiming
{
    /// <summary>
    /// Describes the claim.
    /// </summary>
    public interface IClaimDescriptor : IClaim
    {
        /// <summary>
        /// Gets the <see cref="ClaimResult"/> or <c>null</c>.
        /// </summary>
        ClaimResult Result { get; }

        /// <summary>
        /// Gets whether or not describes claimed claim.
        /// </summary>
        bool IsClaimed { get; }

        /// <summary>
        /// Gets whether or not describes approved claim.
        /// </summary>
        bool IsApproved { get; }

        /// <summary>
        /// Gets whether or not decribes denied claim.
        /// </summary>
        bool IsDenied { get; }

        /// <summary>
        /// Gets whether or not decribes claim of not found instance.
        /// </summary>
        bool IsNotFound { get; }
    }
}
