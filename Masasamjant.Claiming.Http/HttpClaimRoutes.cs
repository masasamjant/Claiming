namespace Masasamjant.Claiming.Http
{
    /// <summary>
    /// Represents routes of HTTP claiming API.
    /// </summary>
    public sealed class HttpClaimRoutes
    {
        private readonly Dictionary<string, string> routes;

        /// <summary>
        /// Initializes new instance of the <see cref="HttpClaimRoutes"/> class with default 
        /// routes from <see cref="HttpClaimDefaultRoute"/>.
        /// </summary>
        public HttpClaimRoutes()
        {
            routes = new Dictionary<string, string>();
            GetClaimByIdentifierRoute = HttpClaimDefaultRoute.GetClaimByIdentifierRoute;
            GetClaimByKeyRoute = HttpClaimDefaultRoute.GetClaimByKeyRoute;
            GetClaimsRoute = HttpClaimDefaultRoute.GetClaimsRoute;
            IsClaimedRoute = HttpClaimDefaultRoute.IsClaimedRoute;
            ReleaseClaimRoute = HttpClaimDefaultRoute.ReleaseClaimRoute;
            TryClaimRoute = HttpClaimDefaultRoute.TryClaimRoute;
            UseClaimLifeTimeRoute = HttpClaimDefaultRoute.UseClaimLifeTimeRoute;
        }

        /// <summary>
        /// Gets or sets the route to get claim by identifier.
        /// </summary>
        public string GetClaimByIdentifierRoute
        {
            get { return routes[nameof(GetClaimByIdentifierRoute)]; }
            set { routes[nameof(GetClaimByIdentifierRoute)] = value; }
        }

        /// <summary>
        /// Gets or sets the route to get claim by key.
        /// </summary>
        public string GetClaimByKeyRoute
        {
            get { return routes[nameof(GetClaimByKeyRoute)]; }
            set { routes[nameof(GetClaimByKeyRoute)] = value; }
        }

        /// <summary>
        /// Gets or sets the route to get all claims.
        /// </summary>
        public string GetClaimsRoute
        {
            get { return routes[nameof(GetClaimsRoute)]; }
            set { routes[nameof(GetClaimsRoute)] = value; }
        }

        /// <summary>
        /// Gets or sets the route to check claim by key.
        /// </summary>
        public string IsClaimedRoute
        {
            get { return routes[nameof (IsClaimedRoute)]; }
            set { routes[nameof(IsClaimedRoute)] = value; } 
        }

        /// <summary>
        /// Gets or sets the route to release claim.
        /// </summary>
        public string ReleaseClaimRoute
        {
            get { return routes[nameof(ReleaseClaimRoute)]; }
            set { routes[nameof(ReleaseClaimRoute)] = value; }
        }

        /// <summary>
        /// Gets or sets the route to try to claim specified key.
        /// </summary>
        public string TryClaimRoute
        {
            get { return routes[nameof(TryClaimRoute)]; }
            set { routes[nameof(TryClaimRoute)] = value; }
        }

        /// <summary>
        /// Gets or sets the route to change claim life time.
        /// </summary>
        public string UseClaimLifeTimeRoute
        {
            get { return routes[nameof(UseClaimLifeTimeRoute)]; }
            set { routes [nameof(UseClaimLifeTimeRoute)] = value; }
        }
    }
}
