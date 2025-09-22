using System.Text.Json.Serialization;

namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents request to claim specified <see cref="IClaimable"/> or <see cref="Claiming.ClaimKey"/> to specified owner.
    /// </summary>
    public sealed class ClaimRequest
    {
        /// <summary>
        /// Initializes new instance of the <see cref="ClaimRequest"/> class.
        /// </summary>
        /// <param name="claimable">The instance attempt to claim.</param>
        /// <param name="ownerIdentifier">The unique identifer of user who owns the claim.</param>
        /// <param name="lifeTimeMinutes">The claim life time in minutes. Default is <see cref="Claims.DefaultClaimLifeTimeMinutes"/>.</param>
        /// <exception cref="ArgumentException">
        /// If claim key of <paramref name="claimable"/> represents empty claim key; <see cref="ClaimKey.IsEmpty"/>.
        /// -or-
        /// If <paramref name="ownerIdentifier"/> is empty or contains only whitespace characters.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="lifeTimeMinutes"/> is less than 1 minute.</exception>
        public ClaimRequest(IClaimable claimable, string ownerIdentifier, int lifeTimeMinutes = Claims.DefaultClaimLifeTimeMinutes)
            : this(claimable.GetClaimKey(), ownerIdentifier, lifeTimeMinutes)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimRequest"/> class.
        /// </summary>
        /// <param name="claimKey">The claim key to identify object instance to claim.</param>
        /// <param name="ownerIdentifier">The unique identifer of user who owns the claim.</param>
        /// <param name="lifeTimeMinutes">The claim life time in minutes. Default is <see cref="Claims.DefaultClaimLifeTimeMinutes"/>.</param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="claimKey"/> represents empty claim key; <see cref="ClaimKey.IsEmpty"/>.
        /// -or-
        /// If <paramref name="ownerIdentifier"/> is empty or contains only whitespace characters.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="lifeTimeMinutes"/> is less than 1 minute.</exception>
        public ClaimRequest(ClaimKey claimKey, string ownerIdentifier, int lifeTimeMinutes = Claims.DefaultClaimLifeTimeMinutes)
        {
            if (claimKey.IsEmpty)
                throw new ArgumentException("The claim key cannot be empty one.", nameof(claimKey));

            if (string.IsNullOrWhiteSpace(ownerIdentifier))
                throw new ArgumentException("The claim owner identifier cannot be empty or only whitespace characters.", nameof(ownerIdentifier));

            if (lifeTimeMinutes < 1)
                throw new ArgumentOutOfRangeException(nameof(lifeTimeMinutes), lifeTimeMinutes, "The life time in minutes must be greater than 0.");

            ClaimKey = claimKey;
            OwnerIdentifier = ownerIdentifier;
            LifeTimeMinutes = lifeTimeMinutes;
            Checksum = ClaimRequestHelper.ComputeChecksum(this);
        }

        /// <summary>
        /// Initializes new empty instance of the <see cref="ClaimRequest"/> class.
        /// </summary>
        public ClaimRequest()
        { }

        /// <summary>
        /// Gets the unique identifier of user who attempts to claim.
        /// </summary>
        [JsonInclude]
        public string OwnerIdentifier { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the claim key to identify object instance to claim.
        /// </summary>
        [JsonInclude]
        public ClaimKey ClaimKey { get; internal set; } = new ClaimKey();

        /// <summary>
        /// Gets the claim life time in minutes.
        /// </summary>
        [JsonInclude]
        public int LifeTimeMinutes { get; internal set; } = Claims.DefaultClaimLifeTimeMinutes;

        /// <summary>
        /// Gets or sets what date time kind should be used. 
        /// Default is <see cref="DateTimeKind.Local"/>.
        /// </summary>
        /// <remarks>If <see cref="DateTimeKind.Unspecified"/>, then <see cref="DateTimeKind.Local"/> is used.</remarks>
        [JsonInclude]
        public DateTimeKind DateTimeKind { get; set; } = DateTimeKind.Local;

        /// <summary>
        /// Gets checksum of the request.
        /// </summary>
        [JsonInclude]
        public string Checksum { get; internal set; } = string.Empty;
    }
}
