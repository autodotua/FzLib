using System;
using System.Globalization;
using System.Windows.Data;

namespace FzLib.WPF.Converters
{
    public class FileLength2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return NumberConverter.ByteToFitString((long)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}