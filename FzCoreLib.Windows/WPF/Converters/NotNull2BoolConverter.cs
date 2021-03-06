using System;
using System.Globalization;
using System.Windows.Data;

namespace FzLib.WPF.Converters
{
    public class NotNull2BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            if (value is string && string.IsNullOrEmpty(value as string))
            {
                return false;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}