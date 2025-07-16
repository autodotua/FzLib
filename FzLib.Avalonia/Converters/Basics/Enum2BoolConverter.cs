using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters
{
    public class Enum2BoolConverter : Value2BoolConverter<Enum>
    {
        public List<Enum> TrueValues { get; set; } = new List<Enum>();

        protected override bool ConvertImpl(Enum value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || TrueValues == null || TrueValues.Count == 0)
            {
                return false;
            }

            return TrueValues.Contains(value);
        }

    }
}