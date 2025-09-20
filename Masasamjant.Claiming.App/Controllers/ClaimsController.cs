using Masasamjant.Claiming.Http;
using Microsoft.AspNetCore.Mvc;

namespace Masasamjant.Claiming.App.Controllers
{
    /// <summary>
    /// Controller to manage claims thru HTTP API.
    /// </summary>
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimManager claimManager;

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimsController"/> class.
        /// </summary>
        /// <param name="claimManagerFactory">The <see cref="IClaimManagerFactory"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        public ClaimsController(IClaimManagerFactory claimManagerFactory, IConfiguration configuration)
        {
            claimManager = claimManagerFactory.CreateClaimManager(configuration);
        }

        [Route(HttpClaimDefaultRoute.GetClaimByIdentifierRoute)]
        public async Task<IActionResult> GetClaimAsync(Guid claimIdentifier, string? ownerIdentifier)
        {
            var claim = await claimManager.GetClaimAsync(claimIdentifier, ownerIdentifier);
            
            if (claim == null)
                return NotFound();

            return Ok(Create(claim));
        }

        [Route(HttpClaimDefaultRoute.GetClaimByKeyRoute)]
        public async Task<Claim?> GetClaimAsync(string instance, string type, string app)
        {
            var claimKey = new ClaimKey(type, instance, app);
            var claim = await claimManager.GetClaimAsync(claimKey);

            if (claim == null)
                return null;

            return Create(claim);
        }

        [Route(HttpClaimDefaultRoute.GetClaimsRoute)]
        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            var claims = await claimManager.GetClaimsAsync();
            return claims.Select(Create).ToList();
        }

        [Route(HttpClaimDefaultRoute.IsClaimedRoute)]
        public async Task<bool> IsClaimedAsync(string instance, string type, string app)
        {
            var claimKey = new ClaimKey(type, instance, app);
            return await claimManager.IsClaimedAsync(claimKey);
        }

        [Route(HttpClaimDefaultRoute.ReleaseClaimRoute)]
        [HttpPost]
        public async Task<bool> ReleaseClaimAsync([FromBody] Claim claim)
        {
            return await claimManager.ReleaseClaimAsync(claim);
        }

        [Route(HttpClaimDefaultRoute.TryClaimRoute)]
        [HttpPost]
        public async Task<ClaimResponse> TryClaimAsync([FromBody] ClaimRequest request)
        {
            var response = await claimManager.TryClaimAsync(request);
            
            if (response.Result == ClaimResult.NotFound)
                return new ClaimResponse(ClaimResult.NotFound, null);

            if (response.Result == ClaimResult.Approved && response.Claim != null)
                return new ClaimResponse(ClaimResult.Approved, Create(response.Claim));

            return new ClaimResponse(ClaimResult.Denied, response.Claim != null ? Create(response.Claim) : null);
        }

        [Route(HttpClaimDefaultRoute.UseClaimLifeTimeRoute)]
        [HttpPost]
        public void UseClaimLifeTime([FromBody] int minutes)
        {
            claimManager.UseClaimLifeTime(minutes);
        }

        private static Claim Create(IClaim claim)
            => new(claim.ClaimIdentifier, claim.OwnerIdentifier, claim.ClaimKey, claim.ExpiresAt);
    }
}
