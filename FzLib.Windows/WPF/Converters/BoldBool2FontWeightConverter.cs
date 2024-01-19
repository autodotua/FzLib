using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FzLib.WPF.Converters
{
    /// <summary>
    /// 若绑定值为true，则返回粗体，否则为普通
    /// </summary>
    public class BoldBool2FontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return FontWeights.Bold;
            }
            return FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}