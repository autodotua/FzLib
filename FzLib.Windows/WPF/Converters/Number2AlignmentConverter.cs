using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FzLib.WPF.Converters
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
            throw new NotSupportedException();
        }
    }
}