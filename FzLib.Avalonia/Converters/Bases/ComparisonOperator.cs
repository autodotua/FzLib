using System.ComponentModel;

namespace FzLib.Avalonia.Converters;

/// <summary>
/// 支持的比较操作符类型
/// </summary>
public enum ComparisonOperator
{
    [Description("大于")]
    GreaterThan,
    [Description("小于")]
    LessThan,
    [Description("大于等于")]
    GreaterThanOrEqual,
    [Description("小于等于")]
    LessThanOrEqual,
    [Description("等于")]
    Equal,
    [Description("不等于")]
    NotEqual
}
