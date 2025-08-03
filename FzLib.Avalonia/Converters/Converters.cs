using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Converters
{
    public static class Converters
    {
        public static readonly NumberToAlignmentConverter Alignment = new();

        public static readonly BoolLogicConverter
            AndLogic = new() { Operator = BoolLogicConverter.LogicalOperator.And };

        public static readonly BoolToFontWeightConverter BoldFontWeight = new();

        public static readonly BoolToOpacityStyleConverter BoldOpacity = new();

        public static readonly CountToBoolConverter CountGreaterThanZero =
            new() { ComparisonValue = 0, Operator = ComparisonOperator.GreaterThan };

        public static readonly CountToBoolConverter CountIsZero = new()
            { ComparisonValue = 0, Operator = ComparisonOperator.Equal };

        public static readonly DescriptionConverter Description = new();
        public static readonly EqualToBoolConverter EqualWithParameter = new();
        public static readonly FileLengthConverter FileLength = new();
        public static readonly FilePickerFilterConverter FilePickerFilter = new();
        public static readonly InverseBoolConverter InverseBool = new();
        public static readonly NullToBoolConverter IsNotNull = new();
        public static readonly NullToBoolConverter IsNull = new() { ValueWhenNull = true };
        public static readonly BoolToFontStyleConverter ItalicFontStyle = new();
        public static readonly BoolToFontWeightConverter LightFontWeight = new() { TrueFontWeight = FontWeight.Light };
        public static readonly EqualToBoolConverter NotEqualWithParameter = new() { InverseResult = true };
        public static readonly BoolLogicConverter OrLogic = new() { Operator = BoolLogicConverter.LogicalOperator.Or };
        public static readonly StringListConverter StringList = new();
        public static readonly BoolToTextWrappingConverter TextWrapping = new();
        public static readonly NumberToThicknessConverter Thickness = new();
        public static readonly TimeSpanConverter TimeSpan = new();
        public static readonly TimeSpanNumberConverter TimeSpanNumber = new();
        public static readonly BoolToTextDecorationConverter UnderlineTextDecoration = new();
        public static readonly DateTimeConverter DateTime = new();
        public static readonly BoolToTextDecorationConverter OverlineTextDecoration =
            new() { TrueTextDecoration = TextDecorations.Overline };
    }
}