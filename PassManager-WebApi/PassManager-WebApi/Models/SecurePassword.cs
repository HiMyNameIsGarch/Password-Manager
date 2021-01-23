using System.Security.Cryptography;
using System.Text;

namespace PassManager_WebApi.Models
{
    public class SecurePassword
    {
        private const int HASH_SIZE = 32; // size in bytes
        private const int ITERATIONS = 100000; // default number of pbkdf2 iterations
        internal static byte[] HashPassword(string salt, string password, int iterations = ITERATIONS)
        {
            var newSalt = GenerateSalt(salt);
            var newPass = CreatePBKDF2Hash(password, newSalt, iterations);
            return newPass;
        }
        private static byte[] GenerateSalt(string input)
        {
            HashAlgorithm sha = SHA1.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            return sha.ComputeHash(bytes);
        }
        private static byte[] CreatePBKDF2Hash(string input, byte[] salt, int iterations)
        {
            if (string.IsNullOrEmpty(input) || iterations < 2) return null;
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, salt, iterations);
            return pbkdf2.GetBytes(HASH_SIZE);
        }
    }
}