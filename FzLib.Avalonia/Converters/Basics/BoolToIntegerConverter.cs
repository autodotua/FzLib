using System;
using System.Globalization;

namespace FzLib.Avalonia.Converters;

public class BoolToIntegerConverter : BoolToValueConverterBase<int>
{
    public int TrueNumber { get; set; } = 1;
    public int FalseNumber { get; set; } = 0;
    protected override int TrueValue => TrueNumber;
    protected override int FalseValue => FalseNumber;

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return TrueNumber.Equals(value) || (FalseNumber.Equals(value) ? false : throw new ArgumentOutOfRangeException($"值应当为{TrueNumber}（表示true）或{FalseNumber}（表示false）"));
    }

}