using Masasamjant.Claiming.Http;
using Masasamjant.Claiming.Memory;
using Masasamjant.Claiming.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Masasamjant.Http;

namespace Masasamjant.Claiming
{
    /// <summary>
    /// Provides helper methods to <see cref="IServiceCollection"/> to register <see cref="IClaimManagerFactory"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds one of the built-in <see cref="IClaimManagerFactory"/> implementations to specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="httpClientConfiguration">The <see cref="HttpClientConfiguration"/>, if HTTP client is configured also; <c>null</c> otherwise.</param>
        /// <returns>A <paramref name="services"/>.</returns>
        /// <exception cref="InvalidOperationException">If configuration is missing manager type configuration.</exception>
        /// <exception cref="NotSupportedException">If configured manager type is not supported.</exception>
        public static IServiceCollection AddClaimManagerFactory(this IServiceCollection services, IConfiguration configuration, HttpClientConfiguration? httpClientConfiguration = null)
        {
            var topSection = configuration.GetRequiredSection("Masasamjant");
            var claimingSection = topSection.GetRequiredSection("Claiming");
            var managerSection = claimingSection.GetRequiredSection("Manager");

            // Get configuration what kind of claim manager should be used.
            var managerType = managerSection["Type"];

            // Claim manager type configuration is mandatory.
            if (string.IsNullOrWhiteSpace(managerType))
                throw new InvalidOperationException("Masasamjant.Claming.Manager.Type configuration is mandatory.");

            // Register correct claim manager factory implementation.
            switch (managerType)
            {
                case ClaimManagerType.MemoryClaimManager:
                    services.AddTransient<IClaimManagerFactory, MemoryClaimManagerFactory>();
                    break;
                case ClaimManagerType.HttpClaimManager:
                    services.AddHttpClientFromConfiguration(httpClientConfiguration);
                    services.AddTransient<IClaimManagerFactory, HttpClaimManagerFactory>();
                    break;
                case ClaimManagerType.EntityClaimManager:
                    services.AddTransient<IClaimManagerFactory, EntityClaimManagerFactory>();
                    break;
                default:
                    throw new NotSupportedException($"'{managerType}' is not supported claim manager.");
            }

            return services;
        }
    }
}
