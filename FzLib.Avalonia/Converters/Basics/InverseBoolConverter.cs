using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters
{
    /// <summary>
    /// 反转bool
    /// </summary>
    public class InverseBoolConverter : BoolToValueConverterBase<bool>
    {
        protected override bool TrueValue { get; } = false;
        protected override bool FalseValue { get; } = true;
    }
}