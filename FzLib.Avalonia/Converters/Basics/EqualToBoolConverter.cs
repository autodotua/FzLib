using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters;

public class EqualToBoolConverter : ValueToBoolConverter<object>
{
    /// <summary>
    /// 当ComparisionValue或parameter为Type时，是否比较value的类型
    /// </summary>
    public bool CompareType { get; set; } = true;

    /// <summary>
    /// 用于与value比较的值，如果为null则使用parameter进行比较
    /// </summary>
    public object ComparisionValue { get; set; }

    /// <summary>
    /// 字符串比较时使用的比较规则，默认忽略大小写
    /// </summary>
    public StringComparison StringComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    protected override bool ConvertImpl(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Debug.Assert(value != null);
        //如果ComparisionValue或parameter为Type且CompareType为true，则比较value的类型
        if (CompareType && (ComparisionValue is Type || parameter is Type))
        {
            Type type = ComparisionValue as Type ?? parameter as Type;
            return type.IsInstanceOfType(value);
        }

        //正常比较
        if (ComparisionValue != null)
        {
            return value.Equals(ComparisionValue)
                   || value is string strValue && strValue.Equals(ComparisionValue.ToString(), StringComparison)
                   || ComparisionValue is string strParam && strParam.Equals(value?.ToString(), StringComparison);
        }
        else
        {
            return value.Equals(parameter)
                   || value is string strValue && strValue.Equals(parameter?.ToString(), StringComparison)
                   || parameter is string strParam && strParam.Equals(value?.ToString(), StringComparison);
        }
    }
}