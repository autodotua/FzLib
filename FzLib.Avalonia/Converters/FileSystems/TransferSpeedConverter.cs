using Avalonia.Data.Converters;
using System;
using System.Globalization;
using System.Reactive;

namespace FzLib.Avalonia.Converters
{
    public class TransferSpeedConverter : IValueConverter
    {
        public string[] Units { get; set; } = [" B/s", " KB/s", " MB/s", " GB/s", " TB/s"];

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (value is double d)
            {
                if (double.IsNaN(d))
                {
                    return null;
                }
                if (double.IsInfinity(d))
                {
                    return "∞";
                }
                int unitIndex = 0;
                while (d >= 1024 && unitIndex < Units.Length - 1)
                {
                    d /= 1024;
                    unitIndex++;
                }
                return $"{d:F2}{Units[unitIndex]}";
            }

            throw new ArgumentException("不支持的类型");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}