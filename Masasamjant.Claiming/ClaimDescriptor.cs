using System.Text.Json.Serialization;

namespace Masasamjant.Claiming
{
    /// <summary>
    /// Describes the claim.
    /// </summary>
    public class ClaimDescriptor : IClaimDescriptor
    {
        /// <summary>
        /// Initializes new instance of the <see cref="ClaimDescriptor"/> class.
        /// </summary>
        /// <param name="result">The <see cref="ClaimResult"/>.</param>
        /// <param name="claim">The <see cref="IClaim"/> or <c>null</c>.</param>
        public ClaimDescriptor(ClaimResult result, IClaim? claim)
        {
            Result = result;
            Claim = claim;
        }

        /// <summary>
        /// Initializes new empty instance of the <see cref="ClaimDescriptor"/> class.
        /// </summary>
        public ClaimDescriptor()
        { }

        /// <summary>
        /// Gets the <see cref="ClaimResult"/> or <c>null</c>.
        /// </summary>
        [JsonInclude]
        public ClaimResult Result { get; set; }

        /// <summary>
        /// Gets the <see cref="IClaim"/> or <c>null</c>.
        /// </summary>
        [JsonInclude]
        public IClaim? Claim { get; set; }

        /// <summary>
        /// Gets whether or not describes claimed claim.
        /// </summary>
        [JsonIgnore]
        public bool IsClaimed => Result != ClaimResult.NotFound && Claim != null && !Claim.IsEmpty;

        /// <summary>
        /// Gets whether or not describes approved claim.
        /// </summary>
        [JsonIgnore]
        public bool IsApproved => Result == ClaimResult.Approved && Claim != null && !Claim.IsEmpty;

        /// <summary>
        /// Gets whether or not decribes denied claim.
        /// </summary>
        [JsonIgnore]
        public bool IsDenied => Result == ClaimResult.Denied;

        /// <summary>
        /// Gets whether or not decribes claim of not found instance.
        /// </summary>
        [JsonIgnore]
        public bool IsNotFound => Result == ClaimResult.NotFound;   
    }
}
