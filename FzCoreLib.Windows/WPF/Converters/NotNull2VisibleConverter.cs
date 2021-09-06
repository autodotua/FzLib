using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FzLib.WPF.Converters
{
    /// <summary>
    /// 若值为非空或字符串不为空，则返回Visible，否则，若参数包含'h'，则返回Hidden，不包含返回Collapsed。
    /// </summary>
    public class NotNull2VisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var falseValue = parameter is string str && str.Contains('h') ? Visibility.Hidden : Visibility.Collapsed;
            if (value == null)
            {
                return falseValue;
            }
            if (value is string s && string.IsNullOrEmpty(s))
            {
                return falseValue;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}