using Avalonia.Media;

namespace FzLib.Avalonia.Converters;

public class BoolToTextWrappingConverter : BoolToValueConverterBase<TextWrapping>
{
    public TextWrapping TrueTextWrapping { get; set; } = TextWrapping.Wrap;
    protected override TextWrapping TrueValue => TrueTextWrapping;
    protected override TextWrapping FalseValue { get; } = TextWrapping.NoWrap;
}
