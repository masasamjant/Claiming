using System.Text.Json.Serialization;

namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents response to attempt to claim object instance.
    /// </summary>
    public class ClaimResponse : IClaimResponse
    {
        /// <summary>
        /// Initializes new instance of the <see cref="ClaimResponse"/> class.
        /// </summary>
        /// <param name="result">The <see cref="ClaimResult"/>.</param>
        /// <param name="claim">The claim, if <paramref name="result"/> is <see cref="ClaimResult.Approved"/>; <c>null</c> otherwise.</param>
        /// <exception cref="ArgumentException">If value of <paramref name="result"/> is not defined in <see cref="ClaimResult"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="result"/> is <see cref="ClaimResult.Approved"/> and <paramref name="claim"/> is <c>null</c>.</exception>
        public ClaimResponse(ClaimResult result, Claim? claim)
        {
            if (!Enum.IsDefined(result))
                throw new ArgumentException("The value is not defined.", nameof(result));

            Result = result;

            if (Result == ClaimResult.Approved)
            {
                if (claim == null)
                    throw new ArgumentNullException(nameof(claim), "The claim must be specified if claim was approved.");
            }
            else
                Claim = null;
        }

        /// <summary>
        /// Initializes new empty instance of the <see cref="ClaimResponse"/> class.
        /// </summary>
        public ClaimResponse() 
        { }

        /// <summary>
        /// Gets the <see cref="ClaimResult"/>.
        /// </summary>
        [JsonInclude]
        public ClaimResult Result { get; internal set; }

        /// <summary>
        /// Gets the claim, if <see cref="Result"/> is <see cref="ClaimResult.Approved"/>.
        /// </summary>
        [JsonInclude]
        public Claim? Claim { get; internal set; }

        /// <summary>
        /// Creates <see cref="ClaimResponse"/> for <see cref="ClaimResult.NotFound"/>.
        /// </summary>
        /// <returns>A <see cref="ClaimResponse"/> for <see cref="ClaimResult.NotFound"/>.</returns>
        public static ClaimResponse NotFound() => new ClaimResponse(ClaimResult.NotFound, null);

        /// <summary>
        /// Creates <see cref="ClaimResponse"/> for <see cref="ClaimResult.Approved"/>.
        /// </summary>
        /// <param name="claim">The approved claim.</param>
        /// <returns>A <see cref="ClaimResponse"/> for <see cref="ClaimResult.Approved"/>.</returns>
        public static ClaimResponse Approved(Claim claim) => new ClaimResponse(ClaimResult.Approved, claim);

        /// <summary>
        /// Creates <see cref="ClaimResponse"/> for <see cref="ClaimResult.Denied"/>.
        /// </summary>
        /// <returns>A <see cref="ClaimResponse"/> for <see cref="ClaimResult.Denied"/>.</returns>
        public static ClaimResponse Denied() => new ClaimResponse(ClaimResult.Denied, null);

        IClaim? IClaimResponse.Claim => Claim;
    }
}
