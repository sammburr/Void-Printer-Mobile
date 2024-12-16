using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace HelloWorld;

public class IntensityToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intensity)
        {
            return intensity switch
            {
                -1 => Colors.LightGray,
                0 => Colors.Orange,
                1 => Colors.Yellow,
                2 => Colors.Lime,
                3 => Colors.Turquoise,
                4 => Colors.Teal,
                _ => Colors.Blue
            };
        }
        return Colors.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
