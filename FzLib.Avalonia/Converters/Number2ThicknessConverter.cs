using System;
using System.Globalization;
using System.Windows;
using Avalonia;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters
{
    /// <summary>
    /// 数字转四边等宽的Thickness
    /// </summary>
    public class Number2ThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}