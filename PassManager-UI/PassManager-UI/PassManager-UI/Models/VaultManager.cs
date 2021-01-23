using System;
using System.IO;
using System.Security.Cryptography;

namespace PassManager.Models
{
    internal class VaultManager
    {
        private static byte[] VaultKey;
        public static string CreateAuthPassword(string email, string password)
        {
            string newPassword = email + password;//concat email and password to increase auth password
            VaultKey = SecurePassword.HashPassword(email, newPassword);//create a vault key based on new password
            string authPass = Convert.ToBase64String(VaultKey) + password;//concat the vault key and the new password
            authPass = Convert.ToBase64String(SecurePassword.HashPassword(email, authPass, 5000));//then hash it 5000 times on client side
            return authPass;//return it
        }
    }
}
