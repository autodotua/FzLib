using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters
{
    public class ValueMappingConverter​ : IValueConverter
    {
        public IDictionary<string,string> Map { get; set; }

        public bool ThrowIfNotInMap { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (Map == null)
            {
                throw new NullReferenceException("还未指定映射关系");
            }
            var key = GetString(value);
            if (Map.TryGetValue(key, out string result))
            {
                return result;
            }
            if (ThrowIfNotInMap)
            {
                throw new KeyNotFoundException($"值{key}没有找到");
            }
            return key;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public virtual string GetString(object obj)
        {
            return obj.ToString();
        }
    }
}