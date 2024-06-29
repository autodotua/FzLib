using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters
{

    public class StringListConverter : IValueConverter
    {
        public string DefaultSeparator { get; set; } = ",";
        public string[] AcceptedSeparator { get; set; } = [",", "，"];
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<string> list = value as IEnumerable<string>;
            return string.Join(DefaultSeparator + " ", list) + DefaultSeparator + " ";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            value ??= "";
            if (value is string s)
            {
                var list = s.Split(AcceptedSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(p => p.Trim());

                if (targetType == typeof(string[]))
                {
                    return list.ToArray();
                }
                else if (targetType == typeof(List<string>))
                {
                    return list.ToList();
                }
                else if (targetType == typeof(ObservableCollection<string>))
                {
                    return new ObservableCollection<string>(list);
                }
                else
                {
                    throw new Exception("目标格式不是字符串数组或列表");
                }
            }
            throw new Exception("值不是字符串");
        }
    }
}