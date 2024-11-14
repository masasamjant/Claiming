namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents response to attempt to claim object instance.
    /// </summary>
    public interface IClaimResponse
    {
        /// <summary>
        /// Gets the <see cref="ClaimResult"/>.
        /// </summary>
        ClaimResult Result { get; }

        /// <summary>
        /// Gets the <see cref="IClaim"/>, if <see cref="Result"/> is <see cref="ClaimResult.Approved"/>.
        /// </summary>
        IClaim? Claim { get; }
    }
}
