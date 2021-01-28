using PassManager.Enums;

namespace PassManager.Models
{
    public static class EnumExtensions
    {
        public static string ToSampleString(this TypeOfItems item)
        {
            switch (item)
            {
                case TypeOfItems.Password: return "Password";
                case TypeOfItems.Wifi: return "Wi-Fi";
                case TypeOfItems.Note: return "Note";
            }
            return string.Empty;
        }
    }
}
