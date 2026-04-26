using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters;

public class DataSizeConverter : IValueConverter
{
    protected DataSizeConverter()
    {
    }

    public DataSizeConverter(string[] units)
    {
        Units = units;
    }

    public virtual string[] Units { get; protected set; }
    
    public int DecimalDigits { get; set; } = 2;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        if (value is IConvertible convertible)
        {
            double numericValue = convertible.ToDouble(CultureInfo.InvariantCulture);
            return FormatSize(numericValue, Units, DecimalDigits);
        }

        return null;
    }

    private static string FormatSize(double size, string[] units, int decimalDigits)
    {
        if (size < 0 || units == null || units.Length == 0)
        {
            return "";
        }

        double num = size;
        int index = 0;

        while (index < units.Length - 1 && (num >= 1024.0 || units[index] == null))
        {
            num /= 1024.0;
            index++;
        }

        string format = (index == 0) ? "F0" : $"N{decimalDigits}";

        return num.ToString(format) + units[index];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}