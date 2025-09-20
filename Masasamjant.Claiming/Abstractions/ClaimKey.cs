using System.Text.Json.Serialization;

namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents claim key to identify claimed object instance.
    /// </summary>
    public sealed class ClaimKey : IEquatable<ClaimKey>
    {
        internal const string InstanceIdentifierParameter = "instance";

        internal const string TypeNameParameter = "type";

        internal const string ApplicationParameter = "app";

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimKey"/> class.
        /// </summary>
        /// <param name="assemblyQualifiedTypeName">The assembly qualified name of type of object instance.</param>
        /// <param name="instanceIdentifier">The unique identifier to identify object instance in claiming.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="assemblyQualifiedTypeName"/>, <paramref name="instanceIdentifier"/> or <paramref name="application"/> is empty or contains only whitespace characters.</exception>
        public ClaimKey(string assemblyQualifiedTypeName, string instanceIdentifier, string application)
        {
            if (string.IsNullOrWhiteSpace(assemblyQualifiedTypeName))
                throw new ArgumentNullException(nameof(assemblyQualifiedTypeName), "The assembly qualitifed type name cannot be empty or only whitespace characters.");

            if (string.IsNullOrWhiteSpace(instanceIdentifier))
                throw new ArgumentNullException(nameof(instanceIdentifier), "The instance identifier cannot be empty or only whitespace characters.");

            if (string.IsNullOrWhiteSpace(application))
                throw new ArgumentNullException(nameof(application), "The application name cannot be empty or only whitespace characters.");

            AssemblyQualifiedTypeName = assemblyQualifiedTypeName;
            InstanceIdentifier = instanceIdentifier;
            Application = application;
        }

        /// <summary>
        /// Initializes new instance of the <see cref="ClaimKey"/> class.
        /// </summary>
        /// <param name="instance">The object instance to claim.</param>
        /// <param name="instanceIdentifier">The unique identifier to identify object instance in claiming.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="instanceIdentifier"/> or <paramref name="application"/> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ArgumentException">If <paramref name="instance"/> does not have assembly qualified name aka it represents generic type parameter.</exception>
        public ClaimKey(object instance, string instanceIdentifier, string application)
        {
            if (string.IsNullOrWhiteSpace(instanceIdentifier))
                throw new ArgumentNullException(nameof(instanceIdentifier), "The instance identifier cannot be empty or only whitespace characters.");

            if (string.IsNullOrWhiteSpace(application))
                throw new ArgumentNullException(nameof(application), "The application name cannot be empty or only whitespace characters.");

            AssemblyQualifiedTypeName = instance.GetType().AssemblyQualifiedName
                ?? throw new ArgumentException("The type of instance does not have assembly qualified name.", nameof(instance));
            InstanceIdentifier = instanceIdentifier;
            Application = application;
        }

        /// <summary>
        /// Initializes new empty instance of the <see cref="ClaimKey"/> class.
        /// </summary>
        public ClaimKey()
        { }

        /// <summary>
        /// Gets the unique identifier to identify object instance in claiming. Usually this is some key information like
        /// primary key value in database.
        /// </summary>
        [JsonInclude]
        public string InstanceIdentifier { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the assembly qualified name of type of object instance.
        /// </summary>
        [JsonInclude]
        public string AssemblyQualifiedTypeName { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        [JsonInclude]
        public string Application { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets if this represents empty claim key.
        /// </summary>
        [JsonIgnore]
        public bool IsEmpty
        {
            get 
            {
                return string.IsNullOrWhiteSpace(InstanceIdentifier) || 
                    string.IsNullOrWhiteSpace(AssemblyQualifiedTypeName);
            }
        }

        /// <summary>
        /// Check if other <see cref="ClaimKey"/> is not <c>null</c> and is equal to this.
        /// </summary>
        /// <param name="other">The other <see cref="ClaimKey"/>.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is not <c>null</c> and is equal to this; <c>false</c> otherwise.</returns>
        public bool Equals(ClaimKey? other)
        {
            return other != null && ((IsEmpty && other.IsEmpty) || 
                (string.Equals(InstanceIdentifier, other.InstanceIdentifier, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(AssemblyQualifiedTypeName, other.AssemblyQualifiedTypeName, StringComparison.Ordinal) &&
                string.Equals(Application, other.Application, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Check if object instance is <see cref="ClaimKey"/> and equal to this.
        /// </summary>
        /// <param name="obj">The object instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is <see cref="ClaimKey"/> and equal to this; <c>false</c> otherwise.</returns>
        public override bool Equals(object? obj)
        {
            return Equals(obj as ClaimKey);
        }

        /// <summary>
        /// Gets hash code
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(InstanceIdentifier, AssemblyQualifiedTypeName, Application);
        }
    }
}
