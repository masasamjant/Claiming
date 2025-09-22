using Microsoft.Extensions.Configuration;

namespace Masasamjant.Claiming.Memory
{
    /// <summary>
    /// Represents <see cref="IClaimManagerFactory"/> that creates <see cref="StorageClaimManager"/> instance that 
    /// use <see cref="MemoryClaimStorage"/> storage.
    /// </summary>
    public sealed class MemoryClaimManagerFactory : ClaimManagerFactory<StorageClaimManager>
    {
        private static readonly Lock singletonMutex = new Lock();
        private static StorageClaimManager? singleton;

        /// <summary>
        /// Gets the manager type string.
        /// </summary>
        protected override string ManagerType => "MemoryClaimManager";

        /// <summary>
        /// Gets the singleton <see cref="StorageClaimManager"/> instance with <see cref="MemoryClaimStorage"/>.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="claimLifeTime">The claim life time in minutes.</param>
        /// <returns>A singleton <see cref="StorageClaimManager"/> instance with <see cref="MemoryClaimStorage"/>.</returns>
        protected override StorageClaimManager GetSingletonInstance(IConfiguration configuration, int claimLifeTime)
        {
            if (singleton == null)
            {
                lock (singletonMutex)
                {
                    if (singleton == null)
                    {
                        singleton = CreateInstance(configuration, claimLifeTime);
                    }
                }
            }

            return singleton;
        }

        /// <summary>
        /// Creates new <see cref="StorageClaimManager"/> instance with <see cref="MemoryClaimStorage"/>.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="claimLifeTime">The claim life time in minutes.</param>
        /// <returns>A new <see cref="StorageClaimManager"/> instance with <see cref="MemoryClaimStorage"/>.</returns>
        protected override StorageClaimManager CreateInstance(IConfiguration configuration, int claimLifeTime)
        {
            var storage = new MemoryClaimStorage();
            var manager = new MemoryClaimManager(storage);
            manager.UseClaimLifeTime(claimLifeTime);
            return manager;
        }
    }
}
