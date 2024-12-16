using Microsoft.Extensions.Configuration;

namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents factory to create instance of <typeparamref name="TManager"/>.
    /// </summary>
    /// <typeparam name="TManager">The type of the claim manager that implements <see cref="IClaimManager"/> interface.</typeparam>
    public abstract class ClaimManagerFactory<TManager> : IClaimManagerFactory where TManager : IClaimManager
    {
        /// <summary>
        /// Creates instance of <see cref="IClaimManager"/> implementation.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>A instance of <see cref="IClaimManager"/> implementation.</returns>
        /// <exception cref="InvalidOperationException">If cannot create instance.</exception>
        public IClaimManager CreateClaimManager(IConfiguration configuration)
        {
            try
            {
                var topSection = configuration.GetRequiredSection("Masasamjant");
                var claimingSection = topSection.GetRequiredSection("Claiming");
                var managerSection = claimingSection.GetRequiredSection("Manager");
                var type = managerSection["Type"];
                if (!string.Equals(type, ManagerType, StringComparison.Ordinal))
                    throw new InvalidOperationException($"The manager configuration is not for {ManagerType} claim manager.");
                var claimLifeTime = int.TryParse(managerSection["ClaimLifeTime"], out var result) && result > 0 ? result : Claims.DefaultClaimLifeTimeMinutes;
                var managerInstance = Enum.TryParse(managerSection["Instance"], true, out ClaimManagerInstance instance) ? instance : ClaimManagerInstance.Singleton;

                if (managerInstance == ClaimManagerInstance.Singleton)
                    return GetSingletonInstance(configuration, claimLifeTime);
                else
                    return CreateInstance(configuration, claimLifeTime);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Creation of {ManagerType} claim manager failed.", exception);
            }
        }

        /// <summary>
        /// Gets the manager type string.
        /// </summary>
        protected abstract string ManagerType { get; }

        /// <summary>
        /// Gets the singleton <typeparamref name="TManager"/> instance.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="claimLifeTime">The claim life time in minutes.</param>
        /// <returns>A singleton <typeparamref name="TManager"/> instance.</returns>
        protected abstract TManager GetSingletonInstance(IConfiguration configuration, int claimLifeTime);

        /// <summary>
        /// Creates new <typeparamref name="TManager"/> instance.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="claimLifeTime">The claim life time in minutes.</param>
        /// <returns>A new <typeparamref name="TManager"/> instance.</returns>
        protected abstract TManager CreateInstance(IConfiguration configuration, int claimLifeTime);
    }
}
