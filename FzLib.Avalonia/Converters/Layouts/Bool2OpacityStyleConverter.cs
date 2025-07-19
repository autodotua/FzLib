namespace FzLib.Avalonia.Converters;

public class Bool2OpacityStyleConverter : Bool2ValueConverterBase<double>
{
    protected override double TrueValue { get; } = 1d;
    protected override double FalseValue { get; } = 0d;
}