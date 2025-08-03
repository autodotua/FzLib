using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters;

public abstract class BoolToValueConverterBase<T> : IValueConverter
{
    protected abstract T TrueValue { get; }

    protected abstract T FalseValue { get; }


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is true ? TrueValue : FalseValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}