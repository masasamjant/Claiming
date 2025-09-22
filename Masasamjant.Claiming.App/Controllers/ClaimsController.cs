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

            return Ok(claim);
        }

        [Route(HttpClaimDefaultRoute.GetClaimByKeyRoute)]
        public async Task<IActionResult> GetClaimAsync(string instance, string type, string app)
        {
            var claimKey = new ClaimKey(type, instance, app);
            var claim = await claimManager.GetClaimAsync(claimKey);

            if (claim == null)
                return NotFound();

            return Ok(claim);
        }

        [Route(HttpClaimDefaultRoute.GetClaimsRoute)]
        public async Task<IActionResult> GetClaimsAsync()
        {
            var claims = await claimManager.GetClaimsAsync();
            return Ok(claims.ToList());
        }

        [Route(HttpClaimDefaultRoute.IsClaimedRoute)]
        public async Task<IActionResult> IsClaimedAsync(string instance, string type, string app)
        {
            var claimKey = new ClaimKey(type, instance, app);
            var claimed = await claimManager.IsClaimedAsync(claimKey);
            return Ok(claimed);
        }

        [Route(HttpClaimDefaultRoute.ReleaseClaimRoute)]
        [HttpPost]
        public async Task<IActionResult> ReleaseClaimAsync([FromBody] Claim claim)
        {
            var released = await claimManager.ReleaseClaimAsync(claim);
            return Ok(released);
        }

        [Route(HttpClaimDefaultRoute.TryClaimRoute)]
        [HttpPost]
        public async Task<IActionResult> TryClaimAsync([FromBody] ClaimRequest request)
        {
            var response = await claimManager.TryClaimAsync(request);

            if (response == null)
                response = new ClaimResponse(ClaimResult.NotFound, null);

            return Ok(response);
        }

        [Route(HttpClaimDefaultRoute.UseClaimLifeTimeRoute)]
        [HttpPost]
        public void UseClaimLifeTime([FromBody] int minutes)
        {
            claimManager.UseClaimLifeTime(minutes);
        }
    }
}
