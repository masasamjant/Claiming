using Masasamjant.Http;

namespace Masasamjant.Claiming.Http
{
    /// <summary>
    /// Provides helper methods to <see cref="ClaimKey"/> when using it with HTTP.
    /// </summary>
    public static class ClaimKeyHttpHelper
    {
        /// <summary>
        /// Add claim key properties as parameters to specified <see cref="HttpGetRequest"/>.
        /// </summary>
        /// <param name="claimKey">The claim key.</param>
        /// <param name="request">The HTTP GET request.</param>
        /// <param name="instanceIdentifierParameterName">The name of instance identifier parameter.</param>
        /// <param name="typeNameParameterName">The name of assembly qualified type name parameter.</param>
        /// <param name="applicationParameterName">The name of application parameter.</param>
        public static void AddHttpParameters(this ClaimKey claimKey, HttpGetRequest request,
            string instanceIdentifierParameterName = InstanceIdentifierParameter,
            string typeNameParameterName = TypeNameParameter,
            string applicationParameterName = ApplicationParameter)
        {
            request.Parameters.Add(instanceIdentifierParameterName, claimKey.InstanceIdentifier);
            request.Parameters.Add(typeNameParameterName, claimKey.AssemblyQualifiedTypeName);
            request.Parameters.Add(applicationParameterName, claimKey.Application);
        }

        private const string InstanceIdentifierParameter = "instance";

        private const string TypeNameParameter = "type";

        private const string ApplicationParameter = "app";
    }
}
