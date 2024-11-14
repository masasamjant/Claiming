namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents claim of object instance.
    /// </summary>
    public interface IClaim
    {
        /// <summary>
        /// Gets the unique identifer of the claim.
        /// </summary>
        Guid ClaimIdentifier { get; }

        /// <summary>
        /// Gets the unique identifer of user who owns the claim.
        /// </summary>
        string OwnerIdentifier { get; }

        /// <summary>
        /// Gets the claim key to identify claimed object instance.
        /// </summary>
        ClaimKey ClaimKey { get; }

        /// <summary>
        /// Gets the date and time when claim expires.
        /// </summary>
        DateTimeOffset ExpiresAt { get; }

        /// <summary>
        /// Gets whether or not claim is empty. Empty claim is missing some information of
        /// <see cref="ClaimIdentifier"/>, <see cref="OwnerIdentifier"/> or <see cref="ClaimKey"/>.
        /// </summary>
        bool IsEmpty { get; }
    }
}
