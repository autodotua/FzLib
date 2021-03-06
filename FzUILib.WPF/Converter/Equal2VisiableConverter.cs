﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FzLib.UI.Converter
{
    public class Equal2VisiableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Equals(parameter) || value.ToString().Equals(parameter.ToString()))
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}