namespace Masasamjant.Claiming
{
    /// <summary>
    /// Defines what kind of instance of claim manager is used.
    /// </summary>
    public enum ClaimManagerInstance : int
    {
        /// <summary>
        /// Use singleton instance.
        /// </summary>
        Singleton = 0,

        /// <summary>
        /// Use transient instance.
        /// </summary>
        Transient = 1
    }
}
