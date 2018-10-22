using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FzLib.Control
{
    public class DarkerBrushConverter : IValueConverter
    {
        public static void GetDarkerColor(SolidColorBrush background, out SolidColorBrush darker1, out SolidColorBrush darker2, out SolidColorBrush darker3, out SolidColorBrush darker4)
        {
            darker4 = GetDarkerColor(darker3 = GetDarkerColor(darker2 = GetDarkerColor(darker1 = GetDarkerColor(background))));
            //  darker1 = new SolidColorBrush(Color.FromScRgb(background.Color.ScA, background.Color.ScR * 0.9f, background.Color.ScG * 0.9f, background.Color.ScB * 0.9f));
            // darker2 = new SolidColorBrush(Color.FromScRgb(background.Color.ScA, background.Color.ScR * 0.8f, background.Color.ScG * 0.8f, background.Color.ScB * 0.8f));
            // darker3 = new SolidColorBrush(Color.FromScRgb(background.Color.ScA, background.Color.ScR * 0.7f, background.Color.ScG * 0.7f, background.Color.ScB * 0.7f));
            // darker4 = new SolidColorBrush(Color.FromScRgb(background.Color.ScA, background.Color.ScR * 0.6f, background.Color.ScG * 0.6f, background.Color.ScB * 0.6f));
        }
        public static SolidColorBrush GetDarkerColor(SolidColorBrush rawBrush)
        {
            return new SolidColorBrush(Color.FromScRgb(rawBrush.Color.ScA, rawBrush.Color.ScR * 0.9f, rawBrush.Color.ScG * 0.9f, rawBrush.Color.ScB * 0.9f));
        }

        public static Color StringToColor(string color)
        {
            return (Color)ColorConverter.ConvertFromString(color);
        }
        public static SolidColorBrush StringToSolidColorBrush (string color)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is SolidColorBrush))
            {
                return value;
            }
            SolidColorBrush color = value as SolidColorBrush;
            float n = 1;
            switch (parameter as string)
            {
                case "1":
                    n = 0.9f;
                    break;
                case "2":
                    n = 0.8f;
                    break;
                case "3":
                    n = 0.7f;
                    break;
                case "4":
                    n = 0.6f;
                    break;
                case "t":
                    n = 0.5f;
                    break;
            }
            return new SolidColorBrush(Color.FromScRgb(color.Color.ScA, color.Color.ScR * n, color.Color.ScG * n, color.Color.ScB * n));
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    }

}
