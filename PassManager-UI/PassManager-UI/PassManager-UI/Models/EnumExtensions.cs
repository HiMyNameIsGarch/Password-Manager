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
                case TypeOfItems.PaymentCard: return "Payment Card";
            }
            return string.Empty;
        }
        public static string ToPluralString(this TypeOfItems item)
        {
            switch (item)
            {
                case TypeOfItems.Password: return "Passwords";
                case TypeOfItems.Wifi: return "Wi-Fis";
                case TypeOfItems.Note: return "Notes";
                case TypeOfItems.PaymentCard: return "Payment Cards";
            }
            return string.Empty;
        }
    }
}
