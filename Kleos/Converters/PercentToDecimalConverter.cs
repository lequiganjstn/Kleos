using System;
using System.Globalization;

namespace Kleos.Converters
{
    public class PercentToDecimalConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is double d ? d / 100.0 : 0.0;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}