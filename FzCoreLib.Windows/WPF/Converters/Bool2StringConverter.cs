﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace FzLib.WPF.Converters
{
    /// <summary>
    /// 布尔类型转字符串，格式：true:false；true:fa\:lse
    /// </summary>
    public class Bool2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] parts = (parameter as string).Replace("\\:", "{colon}").Split(':');
            if (parts.Length != 2)
            {
                throw new Exception("参数格式错误");
            }

            return ((bool)value ? parts[0] : parts[1]).Replace("{colon}", ":");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Two-way binding not supported by IsNotNullToBoolConverter");
        }
    }
}