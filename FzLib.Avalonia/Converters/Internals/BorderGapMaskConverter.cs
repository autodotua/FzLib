using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FzLib.Avalonia.Converters
{
    internal class BorderGapMaskConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            double hw = (double)values[0];
            double hh = (double)values[1];
            double w = (double)values[2];
            double h = (double)values[3];

            Grid grid = new Grid()
            {
                Width = w,
                Height = h,
                ColumnDefinitions =
                    {
                        new ColumnDefinition(8,GridUnitType.Pixel),
                        new ColumnDefinition(hw,GridUnitType.Pixel),
                        new ColumnDefinition(1,GridUnitType.Star),
                    },
                RowDefinitions =
                    {
                        new RowDefinition(hh/2,GridUnitType.Pixel),
                        new RowDefinition(1,GridUnitType.Star),
                    },
            };

            Rectangle r1 = new Rectangle() { Fill = Brushes.Black };
            Rectangle r2 = new Rectangle() { Fill = Brushes.Black };
            Rectangle r3 = new Rectangle() { Fill = Brushes.Black };

            Grid.SetRowSpan(r1, 2);
            Grid.SetColumn(r2, 1);
            Grid.SetRow(r2, 1);
            Grid.SetColumn(r3, 2);
            Grid.SetRowSpan(r3, 2);

            grid.Children.Add(r1);
            grid.Children.Add(r2);
            grid.Children.Add(r3);

            return (new VisualBrush(grid));
        }
    }
}