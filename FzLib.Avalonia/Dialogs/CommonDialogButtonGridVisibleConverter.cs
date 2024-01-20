using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace FzLib.Avalonia.Dialogs
{
    public class CommonDialogButtonGridVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}