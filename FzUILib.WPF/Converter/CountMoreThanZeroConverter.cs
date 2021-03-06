﻿using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;
using System.Linq;

namespace FzLib.UI.Converter
{
    public class CountMoreThanZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection c)
            {
                return c.Count > 0;
            }
            else if (value is IEnumerable e)
            {
                return e.Cast<object>().Any();
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}