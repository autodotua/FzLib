using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace FzLib.WPF.Converters
{
    public class ValueTupleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ITuple t)
            {
                if (parameter is int index)
                {
                    return t[index];
                }
                if (parameter is string s && int.TryParse(s, out int index2))
                {
                    return t[index2];
                }
                throw new ArgumentException("参数必须为索引值");
            }

            throw new ArgumentException("绑定值必须为ValueTuple");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}