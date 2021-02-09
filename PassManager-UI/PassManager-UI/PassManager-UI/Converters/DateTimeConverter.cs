using System;
using System.Globalization;
using Xamarin.Forms;

namespace PassManager.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            var dateTime = DateTime.TryParse(value.ToString(), out DateTime newDateTime);
            if (dateTime)
            {
                return newDateTime.ToString("dd MMMM yyyy");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
