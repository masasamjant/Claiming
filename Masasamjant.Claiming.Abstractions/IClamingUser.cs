namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents user that can claim object instances.
    /// </summary>
    public interface IClamingUser
    {
        /// <summary>
        /// Gets the unique identifier to identify user in claiming.
        /// </summary>
        /// <returns>A unique identifier to identify user in claiming.</returns>
        string GetClaimUserIdentifier();
    }
}
