using System.Text.Json.Serialization;

namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents claim of object instance.
    /// </summary>
    public class Claim : IClaim
    {
        /// <summary>
        /// Initializes new instance of the <see cref="Claim"/> class.
        /// </summary>
        /// <param name="claimIdentifier">The unique identifer of the claim.</param>
        /// <param name="ownerIdentifier">The unique identifer of user who owns the claim.</param>
        /// <param name="claimKey">The claim key to identify claimed object instance.</param>
        /// <param name="expiresAt">The date and time when claim expires.</param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="claimIdentifier"/> equals <see cref="Guid.Empty"/>.
        /// -or-
        /// If <paramref name="claimKey"/> is empty claim key; <see cref="ClaimKey.IsEmpty"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">If <paramref name="ownerIdentifier"/> is empty or contains only whitespace characters.</exception>
        public Claim(Guid claimIdentifier, string ownerIdentifier, ClaimKey claimKey, DateTimeOffset expiresAt)
        {
            if (Guid.Empty.Equals(claimIdentifier))
                throw new ArgumentException($"The claim identifier cannot be {Guid.Empty}.", nameof(claimIdentifier));

            if (string.IsNullOrWhiteSpace(ownerIdentifier))
                throw new ArgumentNullException(nameof(ownerIdentifier), "The owner identifier cannot be empty or only whitespace characters.");

            if (claimKey.IsEmpty)
                throw new ArgumentException("The claim key cannot be empty one.", nameof(claimKey));

            ClaimIdentifier = claimIdentifier;
            OwnerIdentifier = ownerIdentifier;
            ClaimKey = claimKey;
            ExpiresAt = expiresAt;
        }

        /// <summary>
        /// Initializes new empty <see cref="Claim"/> instance.
        /// </summary>
        public Claim() 
        { }

        /// <summary>
        /// Gets the unique identifer of the claim.
        /// </summary>
        [JsonInclude]
        public Guid ClaimIdentifier { get; protected set; }

        /// <summary>
        /// Gets the unique identifer of user who owns the claim.
        /// </summary>
        [JsonInclude]
        public string OwnerIdentifier { get; protected set; } = string.Empty;

        /// <summary>
        /// Gets the claim key to identify claimed object instance.
        /// </summary>
        [JsonInclude]
        public ClaimKey ClaimKey { get; protected set; } = new ClaimKey();

        /// <summary>
        /// Gets the date and time when claim expires.
        /// </summary>
        [JsonInclude]
        public DateTimeOffset ExpiresAt { get; protected set; }

        /// <summary>
        /// Gets whether or not claim is empty. Empty claim is missing some information of
        /// <see cref="ClaimIdentifier"/>, <see cref="OwnerIdentifier"/> or <see cref="ClaimKey"/>.
        /// </summary>
        [JsonIgnore]
        public bool IsEmpty
        {
            get
            {
                return ClaimIdentifier.Equals(Guid.Empty) ||
                    string.IsNullOrWhiteSpace(OwnerIdentifier) ||
                    ClaimKey.IsEmpty;
            }
        }

        /// <summary>
        /// Check if claim has expired.
        /// </summary>
        /// <returns><c>true</c> if claim is expired; <c>false</c> otherwise.</returns>
        public bool IsExpired()
        {
            return IsExpiredAt(DateTimeOffset.Now);
        }

        /// <summary>
        /// Check if claim will be expired at specified date and time.
        /// </summary>
        /// <param name="datetime">The date and time.</param>
        /// <returns><c>true</c> if claim will be expired at specified date and time; <c>false</c> otherwise.</returns>
        public bool IsExpiredAt(DateTimeOffset datetime)
        {
            return ExpiresAt <= datetime;
        }

        /// <summary>
        /// Check if object instance is <see cref="Claim"/> and equal to this.
        /// </summary>
        /// <param name="obj">The object instance.</param>
        /// <returns><c>true</c> <paramref name="obj"/> is <see cref="Claim"/> and equal to this; <c>false</c> otherwise.</returns>
        public override bool Equals(object? obj)
        {
            return Equals(obj as Claim);
        }

        /// <summary>
        /// Gets hash code.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            if (IsEmpty)
                return 0;

            return HashCode.Combine(ClaimIdentifier, OwnerIdentifier, ClaimKey, ExpiresAt);
        }

        private bool Equals(Claim? other)
        {
            return other != null && ((IsEmpty && other.IsEmpty) ||
                (ClaimIdentifier == other.ClaimIdentifier &&
                OwnerIdentifier == other.OwnerIdentifier &&
                ClaimKey.Equals(other.ClaimKey) &&
                ExpiresAt == other.ExpiresAt));
        }
    }
}
