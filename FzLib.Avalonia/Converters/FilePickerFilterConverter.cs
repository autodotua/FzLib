using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Avalonia.Platform.Storage;

namespace FzLib.Avalonia.Converters
{
    public class FilePickerFilterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (value is List<FilePickerFileType> l)
            {
                return string.Join('|', l.Select(Filter2String));
            }
            else
            {
                throw new Exception($"给定的对象不是{nameof(FilePickerFileType)}类型");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string s)
            {
                value = s;
            }
            if (value == null)
            {
                return null;
            }

            if (value is string stringValue)
            {
                return String2FilterList(stringValue);
            }
            throw new Exception($"字符串格式不正确");
        }

        public static string Filter2String(FilePickerFileType f)
        {
            return $"{f.Name};{string.Join(',', f.Patterns)};{string.Join(',', f.MimeTypes)};{string.Join(',', f.AppleUniformTypeIdentifiers)}";
        }

        public static List<FilePickerFileType> String2FilterList(string value)
        {
            return new List<FilePickerFileType>(value.Split('|').Select(String2Filter));
        }
        public static FilePickerFileType String2Filter(string value)
        {

            var parts = value.Split(';');
            if (parts.Length != 4)
            {
                throw new Exception($"字符串格式不正确，预期格式：Name;Patterns;MimeTypes;AppleUniformTypeIdentifiers");
            }

            var name = parts[0];
            var patterns = parts[1].Split([','], StringSplitOptions.RemoveEmptyEntries);
            var mimeTypes = parts[2].Split([','], StringSplitOptions.RemoveEmptyEntries);
            var appleUniformTypeIdentifiers = parts[3].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            return new FilePickerFileType(name)
            {
                Patterns = patterns.ToList(),
                MimeTypes = mimeTypes.ToList(),
                AppleUniformTypeIdentifiers = appleUniformTypeIdentifiers.ToList()
            };
        }
    }
}