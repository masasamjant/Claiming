namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents claim, stored in database, of object instance.
    /// </summary>
    public class EntityClaim : Claim
    {
        /// <summary>
        /// Initializes new instance of the <see cref="EntityClaim"/> class.
        /// </summary>
        /// <param name="claimIdentifier">The unique identifer of the claim.</param>
        /// <param name="ownerIdentifier">The unique identifer of user who owns the claim.</param>
        /// <param name="claimKey">The claim key to identify claimed object instance.</param>
        /// <param name="expiresAt">The date and time when claim expires.</param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="claimIdentifier"/> equals <see cref="Guid.Empty"/>.
        /// -or-
        /// If <paramref name="claimKey"/> is empty claim key; <see cref="ClaimKey.IsEmpty"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">If <paramref name="ownerIdentifier"/> is empty or contains only whitespace characters.</exception>
        public EntityClaim(Guid claimIdentifier, string ownerIdentifier, ClaimKey claimKey, DateTimeOffset expiresAt)
            : base(claimIdentifier, ownerIdentifier, claimKey, expiresAt)
        {
            AssemblyQualifiedTypeName = claimKey.AssemblyQualifiedTypeName;
            InstanceIdentifier = claimKey.InstanceIdentifier;
        }

        /// <summary>
        /// Initializes new empty <see cref="EntityClaim"/> instance.
        /// </summary>
        public EntityClaim()
            : base()
        { }

        /// <summary>
        /// Gets the assembly qualified name of the type of claimed object.
        /// </summary>
        public string AssemblyQualifiedTypeName { get; protected set; } = string.Empty;

        /// <summary>
        /// Gets the unique identifier to identify object instance in claiming. Usually this is some key information like
        /// primary key value in database.
        /// </summary>
        public string InstanceIdentifier { get; protected set; } = string.Empty;

        /// <summary>
        /// Prepares entity claim instance by assigning claim key 
        /// from <see cref="AssemblyQualifiedTypeName"/> and <see cref="InstanceIdentifier"/>.
        /// </summary>
        public void Prepare()
        {
            if (ClaimKey.IsEmpty)
                ClaimKey = new ClaimKey(InstanceIdentifier, AssemblyQualifiedTypeName);
        }
    }
}
