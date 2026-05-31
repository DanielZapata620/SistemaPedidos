using System.Security.Cryptography;
using System.Text;

namespace API.Helpers
{
    public class HashHelper
    {
        public static string Sha256(string texto)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(texto));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}
