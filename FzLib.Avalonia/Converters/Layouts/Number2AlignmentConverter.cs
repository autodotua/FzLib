using System;
using System.Globalization;
using System.Windows;
using Avalonia.Data.Converters;
using Avalonia.Layout;

namespace FzLib.Avalonia.Converters
{
    /// <summary>
    /// 数字（int）转对齐类型。1：左/上；2：中；3：右/下；0：拉伸
    /// </summary>
    public class Number2AlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int num = System.Convert.ToInt32(value);
            if (targetType == typeof(HorizontalAlignment))
            {
                return num switch
                {
                    0 => HorizontalAlignment.Stretch,
                    1 => HorizontalAlignment.Left,
                    2 => HorizontalAlignment.Center,
                    3 => HorizontalAlignment.Right,
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }
            else if (targetType == typeof(VerticalAlignment))
            {
                return num switch
                {
                    0 => VerticalAlignment.Stretch,
                    1 => VerticalAlignment.Top,
                    2 => VerticalAlignment.Center,
                    3 => VerticalAlignment.Bottom,
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }
            else
            {
                throw new ArgumentException("不支持的目标类型", nameof(targetType));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0; // 默认返回 Stretch 对应的值
            }

            if (value is HorizontalAlignment horizontalAlignment)
            {
                return horizontalAlignment switch
                {
                    HorizontalAlignment.Stretch => 0,
                    HorizontalAlignment.Left => 1,
                    HorizontalAlignment.Center => 2,
                    HorizontalAlignment.Right => 3,
                    _ => 0 // 未知值默认返回 Stretch
                };
            }
            else if (value is VerticalAlignment verticalAlignment)
            {
                return verticalAlignment switch
                {
                    VerticalAlignment.Stretch => 0,
                    VerticalAlignment.Top => 1,
                    VerticalAlignment.Center => 2,
                    VerticalAlignment.Bottom => 3,
                    _ => 0 // 未知值默认返回 Stretch
                };
            }

            throw new ArgumentException("输入值必须是 HorizontalAlignment 或 VerticalAlignment", nameof(value));
        }
    }
}