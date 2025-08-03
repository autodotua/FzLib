using Avalonia.Media;

namespace FzLib.Avalonia.Converters;

public class BoolToFontStyleConverter : BoolToValueConverterBase<FontStyle>
{
    public FontStyle TrueFontStyle { get; set; } = FontStyle.Italic;
    protected override FontStyle TrueValue => TrueFontStyle;
    protected override FontStyle FalseValue { get; } = FontStyle.Normal;
}
