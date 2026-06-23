using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ApiColomiersVolley.BLL.Core.Helloasso
{
    public static class HelloassoWebhookSignatureValidator
    {
        public static bool IsValid(string rawBody, string signatureHeader, string signatureKey)
        {
            if (string.IsNullOrEmpty(signatureHeader) || string.IsNullOrEmpty(signatureKey) || string.IsNullOrEmpty(rawBody))
            {
                return false;
            }

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(signatureKey));
            var computedHex = Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(rawBody))).ToLowerInvariant();

            var expected = Encoding.UTF8.GetBytes(computedHex);
            var actual = Encoding.UTF8.GetBytes(signatureHeader.Trim().ToLowerInvariant());
            return expected.Length == actual.Length && CryptographicOperations.FixedTimeEquals(expected, actual);
        }

        // Statut "partenaire" HelloAsso (signature HMAC) non accessible avec les identifiants
        // actuels (403 sur partners/me/api-notifications) : repli sur la liste d'IP HelloAsso
        // documentée (vérification "basique" disponible à toute association).
        public static bool IsAllowedIp(string remoteIp, IEnumerable<string> allowedIps)
        {
            if (string.IsNullOrEmpty(remoteIp) || allowedIps == null)
            {
                return false;
            }

            return allowedIps.Contains(remoteIp);
        }
    }
}
