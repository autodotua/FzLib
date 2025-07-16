using Avalonia.Media;

namespace FzLib.Avalonia.Converters;
public class Bool2FontStyleConverter : Bool2ValueConverterBase<FontStyle>
{
    public FontStyle TrueFontStyle { get; set; } = FontStyle.Italic;
    protected override FontStyle TrueValue => TrueFontStyle;
    protected override FontStyle FalseValue { get; } = FontStyle.Normal;
}
