using System;
using System.Globalization;
using System.Windows.Data;

namespace FzLib.UI.Converter
{
    public class AlignmentRadioButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool?)value == true)
            {
                return int.Parse(parameter as string);
            }
            else
            {
                return 0;
            }
        }
    }
}