namespace Masasamjant.Claiming.Abstractions
{
    /// <summary>
    /// Represents exception thrown by <see cref="ClaimStorage"/>.
    /// </summary>
    public class ClaimStorageException : ClaimException
    {
        /// <summary>
        /// Initializes new instance of the <see cref="ClaimStorageException"/> class.
        /// </summary>
        /// <param name="claimKey">The claim key.</param>
        public ClaimStorageException(ClaimKey claimKey)
            : base(claimKey)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimStorageException"/> class.
        /// </summary>
        /// <param name="claim">The claim.</param>
        public ClaimStorageException(IClaim claim)
            : base(claim)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimStorageException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="claimKey">The claim key.</param>
        public ClaimStorageException(string message, ClaimKey claimKey)
            : this(message, claimKey, null)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimStorageException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="claim">The claim.</param>
        public ClaimStorageException(string message, IClaim claim)
            : this(message, claim, null)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimStorageException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="claim">The claim.</param>
        /// <param name="innerException">The inner exception or <c>null</c>.</param>
        public ClaimStorageException(string message, IClaim claim, Exception? innerException)
            : base(message, claim, innerException)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimStorageException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="claimKey">The claim key.</param>
        /// <param name="innerException">The inner exception or <c>null</c>.</param>
        public ClaimStorageException(string message, ClaimKey claimKey, Exception? innerException)
            : base(message, claimKey, innerException)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimStorageException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception or <c>null</c>.</param>
        public ClaimStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
