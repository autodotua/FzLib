using Avalonia.Media;

namespace FzLib.Avalonia.Converters;
public class BoolToTextDecorationConverter : BoolToValueConverterBase<TextDecorationCollection>
{
    public TextDecorationCollection TrueTextDecoration { get; set; } = TextDecorations.Underline;
    protected override TextDecorationCollection TrueValue => TrueTextDecoration;
    protected override TextDecorationCollection FalseValue { get; } = new TextDecorationCollection();
}
