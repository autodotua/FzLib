using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Platform.Storage;

namespace FzLib.Avalonia.Converters;
public class BoolToFontWeightConverter : BoolToValueConverterBase<FontWeight>
{
    public FontWeight TrueFontWeight { get; set; } = FontWeight.Bold;
    protected override FontWeight TrueValue => TrueFontWeight;
    protected override FontWeight FalseValue { get; }=FontWeight.Normal;
}