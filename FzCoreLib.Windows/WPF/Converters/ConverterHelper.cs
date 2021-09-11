using System.Windows;

namespace FzLib.WPF.Converters
{
    internal static class ConverterHelper
    {
        public static Visibility GetHiddenMode(object parameter)
        {
            if (parameter is string str)
            {
                if (str.Contains('h'))
                {
                    return Visibility.Hidden;
                }
            }
            return Visibility.Collapsed;
        }

        public static Visibility GetEndHiddenMode(object parameter)
        {
            if (parameter is string str)
            {
                if (str.EndsWith("#h"))
                {
                    return Visibility.Hidden;
                }
            }
            return Visibility.Collapsed;
        }

        public static bool GetInverseResult(bool value, object parameter)
        {
            if (parameter is string str)
            {
                if (str.Contains('i'))
                {
                    return !value;
                }
            }
            return value;
        }
    }
}