using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FzLib.WPF.Converters
{
    public class ItalicBool2FontStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return FontStyles.Italic;
            }
            return FontStyles.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}