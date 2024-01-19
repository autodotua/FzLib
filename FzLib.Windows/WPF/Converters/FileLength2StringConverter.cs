using System;
using System.Globalization;
using System.Windows.Data;

namespace FzLib.WPF.Converters
{
    /// <summary>
    /// 将字节（long）转换为合适的文件大小字符串
    /// </summary>
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