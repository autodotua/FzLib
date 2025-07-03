using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Controls
{
    public class FormItem : ContentControl
    {
        public static readonly StyledProperty<string> DescriptionProperty =
            AvaloniaProperty.Register<FormItem, string>(nameof(Description));

        public static readonly StyledProperty<string> LabelProperty =
                    AvaloniaProperty.Register<FormItem, string>(nameof(Label));

        public static readonly StyledProperty<string> HeaderProperty =
                    AvaloniaProperty.Register<FormItem, string>(nameof(Header));

        public static readonly StyledProperty<double> LabelWidthProperty =
            AvaloniaProperty.Register<FormItem, double>(nameof(LabelWidth), double.NaN);

        public string Description
        {
            get => GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public string Label
        {
            get => GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public string Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        public double LabelWidth
        {
            get => GetValue(LabelWidthProperty);
            set => SetValue(LabelWidthProperty, value);
        }
    }
}
