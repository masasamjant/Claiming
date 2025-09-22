namespace Masasamjant.Claiming.Memory
{
    /// <summary>
    /// Represents <see cref="StorageClaimManager"/> that use <see cref="MemoryClaimStorage"/>.
    /// </summary>
    public sealed class MemoryClaimManager : StorageClaimManager
    {
        /// <summary>
        /// Initializes new instance of the <see cref="MemoryClaimManager"/> class.
        /// </summary>
        /// <param name="storage">The <see cref="MemoryClaimStorage"/>.</param>
        public MemoryClaimManager(MemoryClaimStorage storage)
            : base(storage) 
        { }
    }
}
