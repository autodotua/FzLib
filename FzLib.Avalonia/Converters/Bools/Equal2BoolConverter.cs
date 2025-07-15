using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters;

public class Equal2BoolConverter : Value2BoolConverter<object>
{
    protected override bool ConvertImpl(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Debug.Assert(value != null);
        return value.Equals(parameter);
    }
}