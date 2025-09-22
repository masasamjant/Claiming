namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents <see cref="StorageClaimManager"/> that use <see cref="EntityClaimStorage"/>.
    /// </summary>
    public sealed class EntityClaimManager : StorageClaimManager
    {
        /// <summary>
        /// Initializes new instance of the <see cref="EntityClaimManager"/> class.
        /// </summary>
        /// <param name="storage">The <see cref="EntityClaimStorage"/>.</param>
        public EntityClaimManager(EntityClaimStorage storage)
            : base(storage)
        { }
    }
}
