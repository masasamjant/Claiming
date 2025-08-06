using Masasamjant.Claiming.Abstractions;
using IHttpClientBuilder = Masasamjant.Http.Abstractions.IHttpClientBuilder;
using IHttpClient = Masasamjant.Http.Abstractions.IHttpClient;
using Masasamjant.Http;
using Masasamjant.Http.Abstractions;

namespace Masasamjant.Claiming.Http
{
    /// <summary>
    /// Represents manager of claims that use HTTP claiming API.
    /// </summary>
    public sealed class HttpClaimManager : ClaimManager
    {
        private readonly IHttpClientBuilder httpClientBuilder;
        private readonly HttpClaimManagerConfiguration configuration;

        /// <summary>
        /// Initializes new instance of the <see cref="HttpClaimManager"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/>.</param>
        /// <param name="configuration">The <see cref="HttpClaimManagerConfiguration"/>.</param>
        public HttpClaimManager(IHttpClientBuilder httpClientBuilder, HttpClaimManagerConfiguration configuration)
        {
            this.httpClientBuilder = httpClientBuilder;
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets the routes of HTTP API.
        /// </summary>
        public HttpClaimRoutes Routes { get; } = new HttpClaimRoutes();

        private IHttpClient HttpClient
        {
            get 
            {
                var client = httpClientBuilder.Build(configuration.ClientPurpose);
                return client;
            }
        }

        /// <summary>
        /// Gets <see cref="IClaim"/> specified by claim identifier. If claim owner is specified, 
        /// then claim must be owner by specified owner or <c>null</c> will be returned even if claim exist.
        /// </summary>
        /// <param name="claimIdentifier">The claim identifier.</param>
        /// <param name="ownerIdentifier">The optional identifier of claim owner.</param>
        /// <returns>A <see cref="IClaim"/> or <c>null</c>, if claim not exist.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        public override async Task<IClaim?> GetClaimAsync(Guid claimIdentifier, string? ownerIdentifier)
        {
            try
            {
                var request = new HttpGetRequest(Routes.GetClaimByIdentifierRoute);
                request.Parameters.Add(HttpParameter.From(nameof(claimIdentifier), claimIdentifier));
                
                if (!string.IsNullOrWhiteSpace(ownerIdentifier))
                    request.Parameters.Add(HttpParameter.From(nameof(ownerIdentifier), ownerIdentifier));

                AddApiKeyHeader(request);

                var claim = await HttpClient.GetAsync<Claim>(request);

                return claim;
            }
            catch (Exception exception)
            {
                throw new ClaimException($"Unexpected exception when getting claim of '{claimIdentifier}'", exception);
            }
        }

        /// <summary>
        /// Gets <see cref="IClaim"/> of object instance specified by <see cref="ClaimKey"/>.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> to specify object instance.</param>
        /// <returns>A <see cref="IClaim"/> or <c>null</c>, if claim not exist.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        public override async Task<IClaim?> GetClaimAsync(ClaimKey claimKey)
        {
            try
            {
                var request = new HttpGetRequest(Routes.GetClaimByKeyRoute);
                AddClaimKeyParameters(request, claimKey);

                AddApiKeyHeader(request);

                var claim = await HttpClient.GetAsync<Claim>(request);

                return claim;
            }
            catch (Exception exception)
            {
                throw new ClaimException("Unexpected exception when getting claim using specified claim key.", claimKey, exception);
            }
        }



        /// <summary>
        /// Gets all claims.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{IClaim}"/> of all claims.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        public override async Task<IEnumerable<IClaim>> GetClaimsAsync()
        {
            try
            {
                var request = new HttpGetRequest(Routes.GetClaimsRoute);

                AddApiKeyHeader(request);

                var claims = await HttpClient.GetAsync<IEnumerable<Claim>>(request);

                return claims ?? Enumerable.Empty<Claim>();
            }
            catch (Exception exception)
            {
                throw new ClaimException("Unexpected exception when getting claims.", exception);
            }
        }

        /// <summary>
        /// Check if object instance specified by <see cref="ClaimKey"/> is claimed.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> to specify object instance.</param>
        /// <returns><c>true</c> if object instance specified by <paramref name="claimKey"/> is claimed; <c>false</c> otherwise.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        public override async Task<bool> IsClaimedAsync(ClaimKey claimKey)
        {
            try
            {
                var request = new HttpGetRequest(Routes.IsClaimedRoute);
                AddClaimKeyParameters(request, claimKey);
                AddApiKeyHeader(request);

                var exist = await HttpClient.GetAsync<bool>(request);

                return exist;
            }
            catch (Exception exception)
            {
                throw new ClaimException("Unexpected exception when using specified claim key.", claimKey, exception);
            }
        }

        /// <summary>
        /// Releases specified <see cref="IClaim"/>.
        /// </summary>
        /// <param name="claim">The <see cref="IClaim"/> to release.</param>
        /// <returns><c>true</c> if claim was still valid and released; <c>false</c> otherwise.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        public override async Task<bool> ReleaseClaimAsync(IClaim claim)
        {
            try
            {
                var currentClaim = new Claim(claim.ClaimIdentifier, claim.OwnerIdentifier, claim.ClaimKey, claim.ExpiresAt);
                var request = new HttpPostRequest<Claim>(Routes.ReleaseClaimRoute, currentClaim);
                AddApiKeyHeader(request);
                return await HttpClient.PostAsync<bool, Claim>(request);
            }
            catch (Exception exception)
            {
                throw new ClaimException("Unexpected exception when releasing specified claim.", claim, exception);
            }
        }

        /// <summary>
        /// Try claim object instance to owner using specified <see cref="IClaimRequest"/>.
        /// </summary>
        /// <param name="request">The <see cref="IClaimRequest"/>.</param>
        /// <returns>A <see cref="IClaimResponse"/>.</returns>
        /// <exception cref="ClaimException">If exception occurs.</exception>
        public override async Task<IClaimResponse> TryClaimAsync(IClaimRequest request)
        {
            ValidateRequestChecksum(request);

            try
            {
                var currentRequest = new ClaimRequest(request.ClaimKey, request.OwnerIdentifier, request.LifeTimeMinutes);
                var httpRequest = new HttpPostRequest<ClaimRequest>(Routes.TryClaimRoute, currentRequest);

                AddApiKeyHeader(httpRequest);

                var claimResponse = await HttpClient.PostAsync<ClaimResponse, ClaimRequest>(httpRequest);

                return claimResponse ?? new ClaimResponse(ClaimResult.NotFound, null);
            }
            catch (Exception exception)
            {
                throw new ClaimException("Unexpected exception when attempt to claim using specified claim key.", request.ClaimKey, exception);
            }
        }

        /// <summary>
        /// Changes <see cref="ClaimLifeTimeMinutes"/>.
        /// </summary>
        /// <param name="minutes">The claim life time in minutes. Must be greater than 0 minutes.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="minutes"/> is less than 1 minute.</exception>
        /// <exception cref="InvalidOperationException">If fails to set <see cref="ClaimLifeTimeMinutes"/>.</exception>
        public override async void UseClaimLifeTime(int minutes)
        {
            if (minutes < 1)
                throw new ArgumentOutOfRangeException(nameof(minutes), minutes, "The claim life time in minutes must be greater than 0 minutes.");

            try
            {
                var request = new HttpPostRequest<int>(Routes.UseClaimLifeTimeRoute, minutes);
                AddApiKeyHeader(request);
                await HttpClient.PostAsync(request);
                base.UseClaimLifeTime(minutes);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Unexpected exception when attemt to update claim life time.", exception);
            }
        }

        private void AddApiKeyHeader(HttpRequest request)
        {
            if (!string.IsNullOrWhiteSpace(configuration.ApiKey) && !string.IsNullOrWhiteSpace(configuration.ApiKeyHeader))
                request.Headers.Add(configuration.ApiKeyHeader, configuration.ApiKey);
        }

        private static void AddClaimKeyParameters(HttpGetRequest request, ClaimKey claimKey)
        {
            request.Parameters.Add("instanceIdentifier", claimKey.InstanceIdentifier);
            request.Parameters.Add("type", claimKey.AssemblyQualifiedTypeName);
            request.Parameters.Add("app", claimKey.Application);
        }
    }
}
