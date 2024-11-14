namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents request to claim object instance to specified owner.
    /// </summary>
    public interface IClaimRequest
    {
        /// <summary>
        /// Gets the unique identifier of user who attempts to claim.
        /// </summary>
        string OwnerIdentifier { get; }

        /// <summary>
        /// Gets the claim key to identify object instance to claim.
        /// </summary>
        ClaimKey ClaimKey { get; }

        /// <summary>
        /// Gets the claim life time in minutes.
        /// </summary>
        int LifeTimeMinutes { get; }
    }
}
