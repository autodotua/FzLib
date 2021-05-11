﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FzLib.UI.Converter
{
    public class DataGridNewItemPlaceholder2InvisiableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType().Name == "NamedObject")
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}