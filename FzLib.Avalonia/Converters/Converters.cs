using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Converters
{
    public static class Converters
    {
        public static readonly Number2AlignmentConverter Alignment = new();

        public static readonly BoolLogicConverter
            AndLogic = new() { Operator = BoolLogicConverter.LogicalOperator.And };

        public static readonly Bool2FontWeightConverter BoldFontWeight = new();

        public static readonly Bool2OpacityStyleConverter BoldOpacity = new();

        public static readonly Count2BoolConverter CountGreaterThanZero =
            new() { ComparisonValue = 0, Operator = ComparisonOperator.GreaterThan };

        public static readonly Count2BoolConverter CountIsZero = new()
            { ComparisonValue = 0, Operator = ComparisonOperator.Equal };

        public static readonly DescriptionConverter Description = new();
        public static readonly Equal2BoolConverter Equal = new();
        public static readonly FileLengthConverter FileLength = new();
        public static readonly FilePickerFilterConverter FilePickerFilter = new();
        public static readonly InverseBoolConverter InverseBool = new();
        public static readonly Null2BoolConverter IsNotNull = new();
        public static readonly Null2BoolConverter IsNull = new() { ValueWhenNull = true };
        public static readonly Bool2FontStyleConverter ItalicFontStyle = new();
        public static readonly Bool2FontWeightConverter LightFontWeight = new() { TrueFontWeight = FontWeight.Light };
        public static readonly Equal2BoolConverter NotEqual = new() { InverseResult = true };
        public static readonly BoolLogicConverter OrLogic = new() { Operator = BoolLogicConverter.LogicalOperator.Or };
        public static readonly StringListConverter StringList = new();
        public static readonly Bool2TextWrappingConverter TextWrapping = new();
        public static readonly Number2ThicknessConverter Thickness = new();
        public static readonly TimeSpanConverter TimeSpan = new();
        public static readonly TimeSpanNumberConverter TimeSpanNumber = new();
        public static readonly Bool2TextDecorationConverter UnderlineTextDecoration = new();

        public static readonly Bool2TextDecorationConverter OverlineTextDecoration =
            new() { TrueTextDecoration = TextDecorations.Overline };
    }
}