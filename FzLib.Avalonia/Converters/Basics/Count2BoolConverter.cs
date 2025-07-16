using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters;
/// <summary>
/// 将集合的元素数量与比较值进行指定条件的比较，返回布尔结果。
/// 示例：
///   Count >= 5:  ComparisonValue=5, Operator=GreaterThanOrEqual
///   Count != 0:  ComparisonValue=0, Operator=NotEqual
/// </summary>
public class Count2BoolConverter : IValueConverter
{
    /// <summary>
    /// 比较的基准值（默认0）
    /// </summary>
    public int ComparisonValue { get; set; } = 0;

    /// <summary>
    /// 比较操作符（默认 GreaterThan）
    /// </summary>
    public ComparisonOperator Operator { get; set; } = ComparisonOperator.GreaterThan;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return false;
        }

        int count = value switch
        {
            ICollection collection => collection.Count,
            IEnumerable enumerable => enumerable.OfType<object>().Count(),
            int i=>i,
            _ => throw new ArgumentException("输入值必须实现 IEnumerable 接口或提供Int32", nameof(value))
        };

        return Operator switch
        {
            ComparisonOperator.GreaterThan => count > ComparisonValue,
            ComparisonOperator.LessThan => count < ComparisonValue,
            ComparisonOperator.GreaterThanOrEqual => count >= ComparisonValue,
            ComparisonOperator.LessThanOrEqual => count <= ComparisonValue,
            ComparisonOperator.Equal => count == ComparisonValue,
            ComparisonOperator.NotEqual => count != ComparisonValue,
            _ => throw new ArgumentOutOfRangeException(nameof(Operator))
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}