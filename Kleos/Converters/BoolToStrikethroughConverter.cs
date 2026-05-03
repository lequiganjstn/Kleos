using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace Kleos.Converters
{
    public class BoolToStrikethroughConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is true ? TextDecorations.Strikethrough : TextDecorations.None;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}