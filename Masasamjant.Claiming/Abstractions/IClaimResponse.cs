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
        /// Gets the <see cref="IClaim"/>, if <see cref="Result"/> is <see cref="ClaimResult.Approved"/> or <see cref="ClaimResult.Denied"/>.
        /// </summary>
        /// <remarks>If <see cref="Result"/> is <see cref="ClaimResult.Denied"/>, then this tells to who has current claim.</remarks>
        IClaim? Claim { get; }
    }
}
