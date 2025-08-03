namespace FzLib.Avalonia.Converters;

public class BoolToOpacityStyleConverter : BoolToValueConverterBase<double>
{
    protected override double TrueValue { get; } = 1d;
    protected override double FalseValue { get; } = 0d;
}