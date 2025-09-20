namespace Masasamjant.Claiming
{
    /// <summary>
    /// Defines types of built-in claim managers.
    /// </summary>
    public static class ClaimManagerType
    {
        /// <summary>
        /// Claim manager using database as claim storage.
        /// </summary>
        public const string EntityClaimManager = "EntityClaimManager";

        /// <summary>
        /// Claim manager using memory as claim storage.
        /// </summary>
        public const string MemoryClaimManager = "MemoryClaimManager";

        /// <summary>
        /// Claim manager that store claims over HTTP.
        /// </summary>
        public const string HttpClaimManager = "HttpClaimManager";
    }
}
