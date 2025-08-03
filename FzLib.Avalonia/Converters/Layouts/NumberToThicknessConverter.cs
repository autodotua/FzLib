using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters
{
    /// <summary>
    /// 数字与四边等宽的Thickness互相转换
    /// </summary>
    public class NumberToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness(System.Convert.ToDouble(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness)
            {
                // 检查四边是否等宽
                if (Math.Abs(thickness.Left - thickness.Top) > double.Epsilon ||
                    Math.Abs(thickness.Left - thickness.Right) > double.Epsilon ||
                    Math.Abs(thickness.Left - thickness.Bottom) > double.Epsilon)
                {
                    throw new ArgumentException("Thickness的四边宽度不相等，无法转换为单一数字");
                }

                return thickness.Left; // 返回任意一边的值（因为四边相等）
            }

            throw new ArgumentException("输入值必须是Thickness类型", nameof(value));
        }
    }
}