namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents object that can be claimed for user.
    /// </summary>
    public interface IClaimable
    {
        /// <summary>
        /// Gets unique identifier to identify object instance in claiming. Usually this is some key information like
        /// primary key value in database.
        /// </summary>
        /// <returns>A unique identifier of the object instance.</returns>
        string GetClaimInstanceIdentifier();
    }
}
