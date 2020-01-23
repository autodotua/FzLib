using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FzLib.UI.Converter
{
    /// <summary>
    /// 通过参数将enum转换为string。
    /// 参数格式示例：Downloading:暂停下载;Paused:继续下载;Stop:开始下载;Pausing:正在暂停
    /// </summary>
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] paras = (parameter as string).Split(';');
            foreach (var item in paras)
            {
                string[] parts = item.Split(':');
                if (parts.Length < 2)
                {
                    throw new Exception("参数格式错误");
                }
                string key = parts[0];
                string str = parts.Length == 2 ? parts[1] : item.Substring(parts[0].Length + 1);
                if (value.ToString() == key)
                {
                    return str;
                }
            }
            throw new Exception("找不到指定的值");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Two-way binding not supported by IsNotNullToBoolConverter");
        }
    }
    /// <summary>
    /// 通过参数将enum转换为bool。
    /// 参数格式示例：Pausing/Stop/Start:false
    /// </summary>
    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] parts = (parameter as string).Split(':');

            if (parts.Length != 2)
            {
                throw new Exception("参数格式错误");
            }
            string key = parts[0];
            if (!bool.TryParse(parts[1], out bool b))
            {
                throw new Exception("布尔值错误");
            }
            if (key.Split('/').Contains(value.ToString()))
            {
                return b;
            }
            else
            {
                return !b;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Two-way binding not supported by IsNotNullToBoolConverter");
        }
    }
    public abstract class EnumToStringConverter<T> : IValueConverter where T:Enum
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            T e = (T)value;
            if(EnumStringDictionary.ContainsKey(e))
            {
                return EnumStringDictionary[e];
            }
            throw new KeyNotFoundException("找不到枚举对应的字符串：" + e);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Two-way binding not supported by IsNotNullToBoolConverter");
        }

        public abstract Dictionary<T, string> EnumStringDictionary { get; }
    }
    /// <summary>
    /// 通过参数将enum转换为string。
    /// 参数格式示例：Stop:Start
    /// </summary>
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] parts = (parameter as string).Replace("\\:", "{colon}").Split(':'); if (parts.Length != 2) ;
            if (parts.Length != 2)
            {
                throw new Exception("参数格式错误");
            }

            return (((bool)value) ? parts[0] : parts[1]).Replace("{colon}", ":");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Two-way binding not supported by IsNotNullToBoolConverter");
        }
    }

}
