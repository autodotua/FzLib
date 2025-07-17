using System;
using System.Globalization;
using Avalonia.Data.Converters;
using FzLib.Numeric;

namespace FzLib.Avalonia.Converters
{
    /// <summary>
    /// 将字节（long）转换为合适的文件大小字符串
    /// </summary>
    public class FileLengthConverter : IValueConverter
    {
        public string[] Units { get; set; } = ["B", "KB", "MB", "GB", "TB"];
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return NumberConverter.ByteToFitString((long)value, Units);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}