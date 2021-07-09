﻿using System;
using System.Globalization;
using System.Windows.Data;
using DColor = System.Drawing.Color;
using MColor = System.Windows.Media.Color;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;

namespace FzLib.WPF.Converters
{
    public class Color2SolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (value is DColor c)
            {
                return new SolidColorBrush(c.ToMeidaColor());
            }
            if (value is MColor mc)
            {
                return new SolidColorBrush(mc);
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// SolidColorBrush转Color
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">若为空或d，则返回<see cref="System.Drawing.Color"/>；若为m，则返回<see cref="System.Windows.Media.Color"/></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (value is SolidColorBrush b)
            {
                if (parameter.Equals("d") || parameter == null)
                {
                    return b.Color.ToDrawingColor();
                }
                else if (parameter.Equals("m"))
                {
                    return b.Color;
                }
                throw new ArgumentException(nameof(parameter) + "只能为m，d或空");
            }
            throw new ArgumentException();
        }
    }
}