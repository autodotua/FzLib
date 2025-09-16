using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;
using Avalonia;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters
{
    public class DescriptionConverter<T> : IValueConverter where T : struct, Enum
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? AvaloniaProperty.UnsetValue : DescriptionConverter.GetDescription((T)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    public class DescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? AvaloniaProperty.UnsetValue : GetDescription(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public static string GetDescription(object en)
        {
            Type type = en.GetType();
            return GetDescription(type, en);
        }

        private static string GetDescription([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] Type type, object en)
        {
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }

        public static string GetDescription<T>([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T en) where T : struct, Enum
        {
            Type type = typeof(T);
            return GetDescription(type, en);
        }
    }
}