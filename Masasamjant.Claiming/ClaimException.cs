namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents exception thrown by <see cref="IClaimManager"/>.
    /// </summary>
    public class ClaimException : Exception
    {
        /// <summary>
        /// Initializes new instance of the <see cref="ClaimException"/> class.
        /// </summary>
        /// <param name="claimKey">The claim key.</param>
        public ClaimException(ClaimKey claimKey)
            : this("Unexpected exception when using specified claim key.", claimKey)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimException"/> class.
        /// </summary>
        /// <param name="claim">The claim.</param>
        public ClaimException(Claim claim)
            : this("Unexpected exception when using specified claim.", claim)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="claimKey">The claim key.</param>
        public ClaimException(string message, ClaimKey claimKey)
            : this(message, claimKey, null) 
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="claim">The claim.</param>
        public ClaimException(string message, Claim claim)
            : this(message, claim, null)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="claim">The claim.</param>
        /// <param name="innerException">The inner exception or <c>null</c>.</param>
        public ClaimException(string message, Claim claim, Exception? innerException)
            : base(message, innerException)
        {
            Claim = claim;
        }

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="claimKey">The claim key.</param>
        /// <param name="innerException">The inner exception or <c>null</c>.</param>
        public ClaimException(string message, ClaimKey claimKey, Exception? innerException)
            : base(message, innerException) 
        {
            ClaimKey = claimKey;
        }

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception or <c>null</c>.</param>
        public ClaimException(string message, Exception? innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Gets the claim key associated with exception.
        /// </summary>
        public ClaimKey? ClaimKey { get; }

        /// <summary>
        /// Gets the claim associated with exception.
        /// </summary>
        public Claim? Claim { get; }
    }
}
