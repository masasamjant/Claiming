using Masasamjant.Claiming.Abstractions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Masasamjant.Claiming.Http
{
    /// <summary>
    /// Represents manager of claims that use HTTP claiming API.
    /// </summary>
    public sealed class HttpClaimManager : ClaimManager
    {
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes new instance of the <see cref="HttpClaimManager"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/>.</param>
        /// <param name="baseAddress">The base address.</param>
        public HttpClaimManager(IHttpClientFactory httpClientFactory, string baseAddress)
        {
            httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));   
        }

        /// <summary>
        /// Gets the routes of HTTP API.
        /// </summary>
        public HttpClaimRoutes Routes { get; } = new HttpClaimRoutes();

        public override async Task<IClaim?> GetClaimAsync(Guid claimIdentifier, string? ownerIdentifier)
        {
            try
            {
                var sb = new StringBuilder(Routes.GetClaimByIdentifierRoute);
                sb.Append($"?claimIdentifier={claimIdentifier}");

                if (!string.IsNullOrWhiteSpace(ownerIdentifier))
                    sb.Append($"&ownerIdentifier={ownerIdentifier}");

                var claim = await httpClient.GetFromJsonAsync<Claim>(sb.ToString());

                return claim;
            }
            catch (Exception exception)
            {
                throw new ClaimException($"Unexpected exception when getting claim of '{claimIdentifier}'", exception);
            }
        }

        public override async Task<IClaim?> GetClaimAsync(ClaimKey claimKey)
        {
            try
            {
                var sb = new StringBuilder(Routes.GetClaimByKeyRoute);
                sb.Append($"?instanceIdentifier={claimKey.InstanceIdentifier}");
                sb.Append($"?name={claimKey.AssemblyQualifiedTypeName}");

                var claim = await httpClient.GetFromJsonAsync<Claim>(sb.ToString());

                return claim;
            }
            catch (Exception exception)
            {
                throw new ClaimException("Unexpected exception when getting claim using specified claim key.", claimKey, exception);
            }
        }

        public override async Task<IEnumerable<IClaim>> GetClaimsAsync()
        {
            try
            {
                var claims = await httpClient.GetFromJsonAsync<List<Claim>>(Routes.GetClaimsRoute);

                if (claims == null)
                    claims = new List<Claim>();

                return claims.AsReadOnly();
            }
            catch (Exception exception)
            {
                throw new ClaimException("Unexpected exception when getting claims.", exception);
            }
        }

        public override async Task<bool> IsClaimedAsync(ClaimKey claimKey)
        {
            try
            {
                var sb = new StringBuilder(Routes.IsClaimedRoute);
                sb.Append($"?instanceIdentifier={claimKey.InstanceIdentifier}");
                sb.Append($"?name={claimKey.AssemblyQualifiedTypeName}");

                var exist = await httpClient.GetFromJsonAsync<bool>(sb.ToString());

                return exist;
            }
            catch (Exception exception)
            {
                throw new ClaimException("Unexpected exception when using specified claim key.", claimKey, exception);
            }
        }

        public override async Task<bool> ReleaseClaimAsync(IClaim claim)
        {
            try
            {
                var currentClaim = new Claim(claim.ClaimIdentifier, claim.OwnerIdentifier, claim.ClaimKey, claim.ExpiresAt);

                var response = await httpClient.PostAsJsonAsync(Routes.ReleaseClaimRoute, currentClaim);

                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception exception)
            {
                throw new ClaimException("Unexpected exception when releasing specified claim.", claim, exception);
            }
        }

        public override async Task<IClaimResponse> TryClaimAsync(IClaimRequest request)
        {
            try
            {
                var currentRequest = new ClaimRequest(request.ClaimKey, request.OwnerIdentifier, request.LifeTimeMinutes);

                var response = await httpClient.PostAsJsonAsync(Routes.TryClaimRoute, currentRequest);

                response.EnsureSuccessStatusCode();

                var claimResponse = await response.Content.ReadFromJsonAsync<ClaimResponse>();

                return claimResponse ?? new ClaimResponse(ClaimResult.NotFound, null);
            }
            catch (Exception exception)
            {
                throw new ClaimException("Unexpected exception when attempt to claim using specified claim key.", request.ClaimKey, exception);
            }
        }

        public override async void UseClaimLifeTime(int minutes)
        {
            if (minutes < 1)
                throw new ArgumentOutOfRangeException(nameof(minutes), minutes, "The claim life time in minutes must be greater than 0 minutes.");

            try
            {
                var response = await httpClient.PostAsJsonAsync(Routes.UseClaimLifeTimeRoute, minutes);
                response.EnsureSuccessStatusCode();
                base.UseClaimLifeTime(minutes);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Unexpected exception when attemt to update claim life time.", exception);
            }
        }
    }
}
