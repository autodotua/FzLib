using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;
using FzLib.Text;

namespace FzLib.Avalonia.Converters
{
    public class StringListConverter : IValueConverter
    {
        public string DefaultSeparator { get; set; } = ", ";
        public string[] AcceptedSeparator { get; set; } = [",", "，"];

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null or UnsetValueType)
            {
                return string.Empty;
            }

            var separator = GetSeparator(parameter);

            if (value is IEnumerable<string> list)
            {
                return string.Join(separator, list);
            }

            if (value is IEnumerable<EditableString> list2)
            {
                return string.Join(separator, list2.Select(p => p.Value));
            }

            throw new Exception("值不是字符串数组或列表");
        }

        private string GetSeparator(object parameter)
        {
            string separator = DefaultSeparator;
            if (parameter is string sep)
            {
                separator = sep;
            }

            return separator;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            value ??= "";
            string str = "";
            if (value is string s)
            {
                str = s;
            }
            else if (value is EditableString e)
            {
                str = e.Value;
            }
            else
            {
                throw new Exception("值不是字符串");
            }

            var separator = GetSeparator(parameter);
            HashSet<string> acceptedSeparator = [separator, ..AcceptedSeparator];

            var list = str
                .Split(acceptedSeparator.ToArray(),
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(p => p.Trim());

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
    }
}