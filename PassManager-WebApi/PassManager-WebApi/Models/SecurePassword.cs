using System;
using System.Security.Cryptography;
using System.Text;

namespace PassManager_WebApi.Models
{
    public class SecurePassword
    {
        private const int HASH_SIZE = 32; // size in bytes
        private const int ITERATIONS = 10000; // default number of pbkdf2 iterations
        internal static byte[] HashPassword(string salt, string password, int iterations = ITERATIONS)
        {
            var newSalt = Encoding.UTF8.GetBytes(salt);
            var newPass = CreatePBKDF2Hash(password, newSalt, iterations);
            return newPass;
        }
        private static byte[] CreatePBKDF2Hash(string input, byte[] salt, int iterations)
        {
            if (string.IsNullOrEmpty(input) || iterations < 2) return null;
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, salt, iterations);
            return pbkdf2.GetBytes(HASH_SIZE);
        }
    }
}