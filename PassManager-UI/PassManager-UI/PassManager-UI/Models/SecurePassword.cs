using System.Security.Cryptography;
using System.Text;

namespace PassManager.Models
{
    public class SecurePassword 
    {
        private const int HASH_SIZE = 32; // size in bytes
        private const int ITERATIONS = 100000; // default number of pbkdf2 iterations
        internal static byte[] HashPassword(string salt, string password, int iterations = ITERATIONS)
        {
            byte[] hashedPassword = null;
            var byteSalt = Encoding.UTF8.GetBytes(salt);
            if(byteSalt.Length < 8)
            {
                byte[] newSalt = new byte[8];
                for (int i = 0; i < byteSalt.Length; i++)
                {
                    newSalt[i] = byteSalt[i]; 
                }
                hashedPassword = CreatePBKDF2Hash(password, newSalt, iterations);
            }
            else
            {
                hashedPassword = CreatePBKDF2Hash(password, byteSalt, iterations);
            }
            return hashedPassword;
        }
        private static byte[] CreatePBKDF2Hash(string input, byte[] salt, int iterations)
        {
            if (string.IsNullOrEmpty(input) || iterations < 2 || salt.Length > 8) return null;
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, salt, iterations);
            return pbkdf2.GetBytes(HASH_SIZE);
        }
    }
}
