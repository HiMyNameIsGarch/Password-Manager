using System;
using System.Text;

namespace PassManager.Models
{
    public static class PasswordGenerator
    {
        private static StringBuilder Sb = new StringBuilder();
        private static Random Random = new Random();
        private static string UpperCaseCharacters = "QWERTYUIOPASDFGHJKLZXCVBNM";
        private static string Numbers = "0123456789";
        private static string SpecialCharacters = @"!@#$%^&*()_+{}[]\,./:;'";
        private static string Characters = "qwertyuiopasdfghjklzxcvbnm";
        public static string GeneratePassword(int length, bool includeUpperCase, bool includeNums, bool includeSpecialChars)
        {
            if (length < 8 || length > 128)
            {
                return string.Empty;
            }
            if (includeUpperCase)
            {
                Characters += UpperCaseCharacters;
            }
            if (includeNums)
            {
                Characters += Numbers;
            }
            if (includeSpecialChars)
            {
                Characters += SpecialCharacters;
            }
            string password = string.Empty;

            while (Sb.ToString().Length < length)
            {
                string currentPass = Sb.ToString();
                Sb.Append(GetNextChar(currentPass == string.Empty ? default : currentPass[currentPass.Length - 1]));
            }
            password = Sb.ToString();
            Sb.Clear();
            Characters = "qwertyuiopasdfghjklzxcvbnm";//set characters to default
            return password;
        }
        private static char GetNextChar(char lastC)
        {
            char firstChar = Characters[Random.Next(0, Characters.Length)];
            if (firstChar == lastC) return GetNextChar(lastC);
            return firstChar;
        }
    }
}
