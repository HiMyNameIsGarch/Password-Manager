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
        public static string EncryptString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            byte[] array;
            using (Aes aes = CreateCipher())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }
        public static string DecryptString(string textToDecrypt)
        {
            if (string.IsNullOrEmpty(textToDecrypt)) return textToDecrypt;

            byte[] buffer = Convert.FromBase64String(textToDecrypt);

            using (Aes aes = CreateCipher())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        private static Aes CreateCipher()
        {
            var cipher = Aes.Create();

            cipher.Padding = PaddingMode.ISO10126;

            cipher.Key = VaultKey;

            cipher.IV = new byte[16];

            return cipher;
        }
    }
}
