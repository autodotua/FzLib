using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters
{
    /// <summary>
    /// 反转bool
    /// </summary>
    public class InverseBoolConverter : Bool2ValueConverterBase<bool>
    {
        protected override bool FalseValue { get; } = true;
        protected override bool TrueValue { get; } = false;
    }
}