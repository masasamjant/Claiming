namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents object that can be claimed for user.
    /// </summary>
    public interface IClaimable
    {
        /// <summary>
        /// Gets the <see cref="ClaimKey"/> for this instance.
        /// </summary>
        /// <returns>A <see cref="ClaimKey"/> for this instance.</returns>
        ClaimKey GetClaimKey();
    }
}
