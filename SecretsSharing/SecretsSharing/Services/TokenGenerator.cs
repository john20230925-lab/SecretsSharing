using System.Security.Cryptography;

namespace SecretsSharing.Services
{
    public static class TokenGenerator
    {
        public static string GenerateToken(int length = 32)
        {
            var bytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes)
                .Replace("+", "")
                .Replace("/", "")
                .Replace("=", "");
        }
    }
}