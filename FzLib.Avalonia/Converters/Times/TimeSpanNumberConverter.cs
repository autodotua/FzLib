using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters
{
    public class TimeSpanNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan t)
            {
                return parameter switch
                {
                    "h" => System.Convert.ToDecimal(t.TotalHours),
                    "m" => System.Convert.ToDecimal(t.TotalMinutes),
                    "s" => System.Convert.ToDecimal(t.TotalSeconds),
                    _ => throw new NotSupportedException("Converter参数错误，未知的转换类型"),
                };
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double d = 0;
            try
            {
                d = System.Convert.ToDouble(value);
            }
            catch (Exception ex)
            {

                return new BindingNotification(
                            new InvalidOperationException("无法转换到数字：" + ex.Message),
                            BindingErrorType.Error
                        );
            }
            return parameter switch
            {
                "h" => TimeSpan.FromHours(d),
                "m" => TimeSpan.FromMinutes(d),
                "s" => TimeSpan.FromSeconds(d),
                _ => throw new NotSupportedException("Converter参数错误，未知的转换类型"),
            };
        }
    }
}