using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Avalonia;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters
{
    public class DescriptionConverter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T> : IValueConverter where T : struct, Enum
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
            FieldInfo field = type.GetField(en.ToString());
            if (field != null)
            {
                DescriptionAttribute attr = field.GetCustomAttribute<DescriptionAttribute>(false);
                if (attr != null)
                {
                    return attr.Description;
                }
            }
            return en.ToString();
        }

        public static string GetDescription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>(T en) where T : struct, Enum
        {
            FieldInfo field = typeof(T).GetField(en.ToString());
            DescriptionAttribute attr = field?.GetCustomAttribute<DescriptionAttribute>(false);
            return attr?.Description ?? en.ToString();
        }
    }
}