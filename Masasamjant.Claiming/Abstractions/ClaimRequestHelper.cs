using System.Security.Cryptography;
using System.Text;

namespace Masasamjant.Claiming
{
    public static class ClaimRequestHelper
    {
        public static string ComputeChecksum(IClaimRequest request)
        {
            if (request.ClaimKey.IsEmpty)
                return string.Empty;
            var buffer = GetRequestBytes(request);
            var hash = SHA256.HashData(buffer);
            return Convert.ToBase64String(hash);
        }

        private static byte[] GetRequestBytes(IClaimRequest request)
        {
            var values = new[]
            {
                request.OwnerIdentifier,
                request.ClaimKey.Application,
                request.ClaimKey.InstanceIdentifier,
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
