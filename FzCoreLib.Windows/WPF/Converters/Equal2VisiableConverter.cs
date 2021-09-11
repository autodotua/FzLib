﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FzLib.WPF.Converters
{
    /// <summary>
    /// 绑定值和参数相等，则Visible，否则Collapsed。参数以"#h"结尾，则为Hidden。
    /// </summary>
    public class Equal2VisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException();
            }
            if (value.Equals(parameter) || value.ToString().Equals(parameter.ToString().RemoveEnd("#h")))
            {
                return Visibility.Visible;
            }
            return ConverterHelper.GetEndHiddenMode(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}