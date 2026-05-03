using System;
using System.Globalization;

namespace Kleos.Converters
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime dt)
                return dt.ToString("MMM dd, yyyy");
            if (value is DateTime ? nullable && nullable.HasValue)
                return nullable.Value.ToString("MMM dd, yyyy");
            return "No due date";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}