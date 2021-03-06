using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FzLib.WPF.Converters
{
    public class Num2AlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value) switch
            {
                1 => HorizontalAlignment.Left,
                2 => HorizontalAlignment.Center,
                3 => HorizontalAlignment.Right,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}