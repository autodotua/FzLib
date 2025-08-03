using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters;

public class EqualToBoolConverter : ValueToBoolConverter<object>
{
    public StringComparison StringComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    protected override bool ConvertImpl(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Debug.Assert(value != null);
        return value.Equals(parameter)
               || value is string strValue && strValue.Equals(parameter?.ToString(), StringComparison)
               || parameter is string strParam && strParam.Equals(value?.ToString(), StringComparison);
    }
}