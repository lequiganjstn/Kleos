using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Globalization;

namespace Kleos.Converters
{
    public class PriorityToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                2 => Color.FromArgb("#E94560"),
                1 => Color.FromArgb("#FDCB6E"),
                0 => Color.FromArgb("#00B894"),
                _ => Color.FromArgb("#FDCB6E")
            };
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}