using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters;

/// <summary>
/// 布尔类型转字符串
/// </summary>
public class BoolToStringConverter : BoolToValueConverterBase<string>
{
    public string TrueString { get; set; }
    public string FalseString { get; set; }
    protected override string TrueValue => TrueString;
    protected override string FalseValue => FalseString;
}
