using Microsoft.Extensions.Configuration;

namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents factory to create instance of <see cref="IClaimManager"/> implementation.
    /// </summary>
    public interface IClaimManagerFactory
    {
        /// <summary>
        /// Creates instance of <see cref="IClaimManager"/> implementation.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>A instance of <see cref="IClaimManager"/> implementation.</returns>
        /// <exception cref="InvalidOperationException">If cannot create instance.</exception>
        IClaimManager CreateClaimManager(IConfiguration configuration);
    }
}
