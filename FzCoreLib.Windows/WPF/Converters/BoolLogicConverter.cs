using System;
using System.Globalization;
using System.Windows.Data;

namespace FzLib.WPF.Converters
{
    public class BoolLogicConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            switch (parameter as string)
            {
                case "or":
                    foreach (bool b in values)
                    {
                        if (b)
                        {
                            return true;
                        }
                    }
                    return false;

                case "nor":
                    foreach (bool b in values)
                    {
                        if (b)
                        {
                            return false;
                        }
                    }
                    return true;

                case "and":
                    foreach (bool b in values)
                    {
                        if (!b)
                        {
                            return false;
                        }
                    }
                    return true;

                default:
                    throw new ArgumentException(nameof(parameter));
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}