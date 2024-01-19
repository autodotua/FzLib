using System;
using System.Globalization;
using System.Windows;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace FzLib.Avalonia.Converters
{
    /// <summary>
    /// 若绑定值为true，则返回斜体，否则为普通
    /// </summary>
    public class ItalicBool2FontStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return FontStyle.Italic;
            }
            return FontStyle.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}