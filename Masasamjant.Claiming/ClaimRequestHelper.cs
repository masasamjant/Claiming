using System.Security.Cryptography;
using System.Text;

namespace Masasamjant.Claiming
{
    /// <summary>
    /// Provides helper methods to <see cref="ClaimRequest"/> class.
    /// </summary>
    public static class ClaimRequestHelper
    {
        /// <summary>
        /// Computes checksum for specified <see cref="ClaimRequest"/>.
        /// </summary>
        /// <param name="request">The claim request.</param>
        /// <returns>A checksum string.</returns>
        public static string ComputeChecksum(ClaimRequest request)
        {
            if (request.ClaimKey.IsEmpty)
                return string.Empty;
            var buffer = GetRequestBytes(request);
            var hash = SHA256.HashData(buffer);
            return Convert.ToBase64String(hash);
        }

        private static byte[] GetRequestBytes(ClaimRequest request)
        {
            var values = new[]
            {
                request.OwnerIdentifier,
                request.ClaimKey.Application,
                request.ClaimKey.InstanceIdentifierSHA1,
                request.ClaimKey.AssemblyQualifiedTypeName,
                request.LifeTimeMinutes.ToString(),
                request.DateTimeKind.ToString()
            };

            var value = string.Join('|', values);
            var buffer = Encoding.Unicode.GetBytes(value);
            return buffer;
        }
    }
}
