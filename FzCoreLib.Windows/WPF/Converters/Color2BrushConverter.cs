﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FzLib.WPF.Converters
{
    public class Color2BrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}