﻿using System.Text.RegularExpressions;
using System.Windows;

namespace FzLib.Avalonia.Converters
{
    internal static class ConverterHelper
    {
        private static bool HasEndFlag(string p, char c)
        {
            return Regex.IsMatch(p, $"#.*{c}.*#$");
        }

        public static string RemoveEndFlags(string str)
        {
            return Regex.Replace(str, @"#.*#$", "");
        }


        public static bool GetEndInverseResult(bool value, object parameter)
        {
            if (parameter is string str)
            {
                if (HasEndFlag(str, 'i'))
                {
                    return !value;
                }
            }
            return value;
        }

        public static bool GetInverseResult(bool value, object parameter)
        {
            if (parameter is string str)
            {
                if (str.Contains("i"))
                {
                    return !value;
                }
            }
            return value;
        }
    }
}