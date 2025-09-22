using Microsoft.Extensions.Configuration;

namespace Masasamjant.Claiming
{
    /// <summary>
    /// Provides helper methods to read claiming configuration.
    /// </summary>
    public static class ClaimingConfigurationHelper
    {
        /// <summary>
        /// Gets the "Claiming" configuration section.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <returns>A <see cref="IConfigurationSection"/> of "Claiming".</returns>
        public static IConfigurationSection GetClaimingConfigurationSection(IConfiguration configuration)
        {
            var topSection = configuration.GetRequiredSection("Masasamjant");
            var claimingSection = topSection.GetRequiredSection("Claiming");
            return claimingSection;
        }
    }
}
