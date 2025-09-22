namespace Masasamjant.Claiming.Http
{
    /// <summary>
    /// Represents configuration of HTTP claim manager.
    /// </summary>
    public sealed class HttpClaimManagerConfiguration
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpClaimManagerConfiguration"/> class.
        /// </summary>
        /// <param name="clientPurpose">The purpose of the HTTP client.</param>
        /// <param name="apiKey">The API key value, if in use.</param>
        /// <param name="apiKeyHeader">The name of API key HTTP header, if in use.</param>
        /// <exception cref="ArgumentNullException">
        /// If value of <paramref name="clientPurpose"/> is empty or only whitespace.
        /// -or-
        /// If value of <paramref name="apiKey"/> is set, but value of <paramref name="apiKeyHeader"/> is <c>null</c>, empty or only whitespace.
        /// </exception>
        public HttpClaimManagerConfiguration(string clientPurpose, string? apiKey, string? apiKeyHeader)
        {
            if (string.IsNullOrWhiteSpace(clientPurpose))
                throw new ArgumentNullException(nameof(clientPurpose), "The client purpose cannot be empty or only whitespace.");

            if (!string.IsNullOrWhiteSpace(apiKey) && string.IsNullOrWhiteSpace(apiKeyHeader))
                throw new ArgumentNullException(nameof(apiKeyHeader), "The API key header cannot be empty or contain only whitespace characters when API key is used.");

            ClientPurpose = clientPurpose;
            ApiKey = apiKey;
            ApiKeyHeader = apiKeyHeader;
        }

        /// <summary>
        /// Gets the purpose of the HTTP client. This is used to resolve correct configuration to use.
        /// </summary>
        public string ClientPurpose { get; }
    
        /// <summary>
        /// Gets the API key value, if in use.
        /// </summary>
        public string? ApiKey { get; }

        /// <summary>
        /// Gets the name of API key HTTP header, if in use.
        /// </summary>
        public string? ApiKeyHeader { get; }
    }
}
