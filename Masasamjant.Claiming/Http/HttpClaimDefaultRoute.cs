namespace Masasamjant.Claiming.Http
{
    /// <summary>
    /// Defines default routes of HTTP claiming.
    /// </summary>
    public static class HttpClaimDefaultRoute
    {
        /// <summary>
        /// Default route to get claim by identifier.
        /// </summary>
        public const string GetClaimByIdentifierRoute = "api/Claims/GetClaimByIdentifier";

        /// <summary>
        /// Default route to get claim by key.
        /// </summary>
        public const string GetClaimByKeyRoute = "api/Claims/GetClaimByKey";

        /// <summary>
        /// Default route to get all claims.
        /// </summary>
        public const string GetClaimsRoute = "api/Claims/GetClaims";

        /// <summary>
        /// Default route to check claim by key.
        /// </summary>
        public const string IsClaimedRoute = "api/Claims/IsClaimed";

        /// <summary>
        /// Default route to release claim.
        /// </summary>
        public const string ReleaseClaimRoute = "api/Claims/ReleaseClaim";

        /// <summary>
        /// Default route to try to claim specified key.
        /// </summary>
        public const string TryClaimRoute = "api/Claims/TryClaim";

        /// <summary>
        /// Default route to change claim life time.
        /// </summary>
        public const string UseClaimLifeTimeRoute = "api/Claims/UseClaimLifeTime";
    }
}
