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

        public static readonly BoolLogicConverter OrLogic = new() { Operator = BoolLogicConverter.LogicalOperator.Or };

        public static readonly BoolLogicConverter
            XorLogic = new() { Operator = BoolLogicConverter.LogicalOperator.Xor };

        public static readonly BoolLogicConverter
            NorLogic = new() { Operator = BoolLogicConverter.LogicalOperator.Nor };

        public static readonly BoolLogicConverter NandLogic = new()
            { Operator = BoolLogicConverter.LogicalOperator.Nand };

        public static readonly BoolLogicConverter XnorLogic = new()
            { Operator = BoolLogicConverter.LogicalOperator.Xnor };

        public static readonly BoolToFontWeightConverter BoldFontWeight = new();
        public static readonly BoolToOpacityStyleConverter BoldOpacity = new();
        public static readonly BoolToIntegerConverter BoolOneZero = new() { TrueNumber = 1, FalseNumber = 0 };
        public static readonly BoolToIntegerConverter BoolZeroOne = new();

        public static readonly CountToBoolConverter CountGreaterThanZero =
            new() { ComparisonValue = 0, Operator = ComparisonOperator.GreaterThan };

        public static readonly CountToBoolConverter CountIsZero = new()
            { ComparisonValue = 0, Operator = ComparisonOperator.Equal };

        public static readonly DateTimeConverter DateTime = new();
        public static readonly DescriptionConverter Description = new();
        public static readonly EqualToBoolConverter EqualWithParameter = new();


        public static readonly DataSizeConverter ByteLength = new([" B", " KB", " MB", " GB", " TB"]);
        public static readonly DataSizeConverter ByteRate = new([" B/s", " KB/s", " MB/s", " GB/s", " TB/s"]);
        public static readonly DataSizeConverter BitLength = new([" b", " Kb", " Mb", " Gb", " Tb"]);
        public static readonly DataSizeConverter BitRate = new([" bps", " Kbps", " Mbps", " Gbps", " Tbps"]);
        public static readonly DataSizeConverter BitRateMbps = new DataSizeConverter([null, null, " Mbps"]);
        public static readonly DataSizeConverter BitRateKbps = new DataSizeConverter([null, " Kbps"]);
        
        [Obsolete("使用ByteLength")]
        public static readonly DataSizeConverter FileLength = ByteLength;

        [Obsolete("使用ByteRate")]
        public static readonly DataSizeConverter TransferSpeed = ByteRate;
        
        public static readonly InverseBoolConverter InverseBool = new();
        public static readonly NullToBoolConverter IsNotNull = new();
        public static readonly NullToBoolConverter IsNull = new() { ValueWhenNull = true };
        public static readonly BoolToFontStyleConverter ItalicFontStyle = new();
        public static readonly BoolToFontWeightConverter LightFontWeight = new() { TrueFontWeight = FontWeight.Light };
        public static readonly EqualToBoolConverter NotEqualWithParameter = new() { InverseResult = true };

        public static readonly BoolToTextDecorationConverter OverlineTextDecoration =
            new() { TrueTextDecoration = TextDecorations.Overline };

        public static readonly StringListConverter StringList = new();
        public static readonly BoolToTextWrappingConverter TextWrapping = new();
        public static readonly NumberToThicknessConverter Thickness = new();
        public static readonly TimeSpanConverter TimeSpan = new();
        public static readonly TimeSpanNumberConverter TimeSpanNumber = new();
        public static readonly BoolToTextDecorationConverter UnderlineTextDecoration = new();
        public static readonly BitmapAssetValueConverter BitmapAsset = new();
    }
}