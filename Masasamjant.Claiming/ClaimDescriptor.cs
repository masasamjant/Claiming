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
            
            if (claim != null)
            {
                ClaimIdentifier = claim.ClaimIdentifier;
                OwnerIdentifier = claim.OwnerIdentifier;
                ClaimKey = claim.ClaimKey;
                ExpiresAt = claim.ExpiresAt;
                IsEmpty = claim.IsEmpty;
            }
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
        /// Gets the unique identifer of the claim.
        /// </summary>
        public Guid ClaimIdentifier { get; internal set; }

        /// <summary>
        /// Gets the unique identifer of user who owns the claim.
        /// </summary>
        public string OwnerIdentifier { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the claim key to identify claimed object instance.
        /// </summary>
        public ClaimKey ClaimKey { get; internal set; } = new ClaimKey();

        /// <summary>
        /// Gets the date and time when claim expires.
        /// </summary>
        public DateTimeOffset ExpiresAt { get; internal set; }

        /// <summary>
        /// Gets whether or not claim is empty. Empty claim is missing some information of
        /// <see cref="ClaimIdentifier"/>, <see cref="OwnerIdentifier"/> or <see cref="ClaimKey"/>.
        /// </summary>
        public bool IsEmpty { get; internal set; } = true;

        /// <summary>
        /// Gets whether or not describes claimed claim.
        /// </summary>
        [JsonIgnore]
        public bool IsClaimed => Result != ClaimResult.NotFound && !IsEmpty;

        /// <summary>
        /// Gets whether or not describes approved claim.
        /// </summary>
        [JsonIgnore]
        public bool IsApproved => Result == ClaimResult.Approved && !IsEmpty;

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
