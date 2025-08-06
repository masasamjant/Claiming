using System.Security.Cryptography.X509Certificates;

namespace Masasamjant.Claiming.Http
{
    public sealed class HttpClaimManagerConfiguration
    {
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

        public string ClientPurpose { get; }
    
        public string? ApiKey { get; }

        public string? ApiKeyHeader { get; }
    }
}
