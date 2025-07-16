using System;
using System.Globalization;
using System.Windows;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters
{
    public class Null2BoolConverter : IValueConverter
    {
        public bool ValueWhenNull { get; set; } = false;
        public bool AsNullIfStringWhiteSpace { get; set; } = true;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return ValueWhenNull;
            }
            if (AsNullIfStringWhiteSpace && value is string && string.IsNullOrEmpty(value as string))
            {
                return ValueWhenNull;
            }
            return !ValueWhenNull;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}