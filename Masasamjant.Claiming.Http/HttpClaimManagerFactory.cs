using Masasamjant.Http.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Masasamjant.Claiming.Http
{
    /// <summary>
    /// Represents <see cref="IClaimManagerFactory"/> that creates <see cref="HttpClaimManager"/> instance.
    /// </summary>
    public sealed class HttpClaimManagerFactory : ClaimManagerFactory<HttpClaimManager>
    {
        private readonly IHttpClientBuilder httpClientBuilder;
        private static readonly Lock singletonMutex = new Lock();
        private static HttpClaimManager? singleton;

        /// <summary>
        /// Initializes new instance of the <see cref="HttpClaimManagerFactory"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/>.</param>
        public HttpClaimManagerFactory(IHttpClientBuilder httpClientFactory)
        {
            this.httpClientBuilder = httpClientFactory;
        }

        /// <summary>
        /// Gets the manager type string.
        /// </summary>
        protected override string ManagerType => "HttpClaimManager";

        /// <summary>
        /// Creates new <see cref="HttpClaimManager"/> instance.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="claimLifeTime">The claim life time in minutes.</param>
        /// <returns>A new <see cref="HttpClaimManager"/> instance.</returns>
        protected override HttpClaimManager CreateInstance(IConfiguration configuration, int claimLifeTime)
        {
            var topSection = configuration.GetRequiredSection("Masasamjant");
            var claimSection = topSection.GetRequiredSection("Claiming");
            var clientPurpose = claimSection["ClientPurpose"] ?? string.Empty;
            var apiSection = claimSection.GetSection("Api");
            var apiKeyHeader = apiSection["Header"];
            var apiKey = apiSection["Key"];
            var apiChecksumHeader = apiSection["Checksum"];

            var managerConfiguration = new HttpClaimManagerConfiguration(clientPurpose, apiKey, apiKeyHeader);

            return new HttpClaimManager(httpClientBuilder, managerConfiguration);
        }

        /// <summary>
        /// Gets the singleton <see cref="HttpClaimManager"/> instance..
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="claimLifeTime">The claim life time in minutes.</param>
        /// <returns>A singleton <see cref="HttpClaimManager"/> instance.</returns>
        protected override HttpClaimManager GetSingletonInstance(IConfiguration configuration, int claimLifeTime)
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
    }
}
