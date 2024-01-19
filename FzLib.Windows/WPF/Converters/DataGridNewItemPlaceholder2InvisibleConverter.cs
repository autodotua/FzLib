using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FzLib.WPF.Converters
{
    /// <summary>
    /// 隐藏DataGrid的新项占位
    /// </summary>
    public class DataGridNewItemPlaceholder2InvisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType().Name == "NamedObject")
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}