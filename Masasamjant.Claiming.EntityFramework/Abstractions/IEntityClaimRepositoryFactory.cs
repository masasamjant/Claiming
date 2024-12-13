using Microsoft.Extensions.Configuration;

namespace Masasamjant.Claiming.Abstractions
{
    /// <summary>
    /// Represents factory to create instance of <see cref="IEntityClaimRepository"/> implementation.
    /// </summary>
    public interface IEntityClaimRepositoryFactory
    {
        /// <summary>
        /// Creates new instance of <see cref="IEntityClaimRepository"/> implementation.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <returns>A new instance of <see cref="IEntityClaimRepository"/> implementation.</returns>
        IEntityClaimRepository CreateRepository(IConfiguration configuration);
    }
}
