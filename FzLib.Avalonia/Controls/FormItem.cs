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
        /// <summary>
        /// Label StyledProperty definition
        /// </summary>
        public static readonly StyledProperty<string> LabelProperty =
            AvaloniaProperty.Register<FormItem, string>(nameof(Label));

        /// <summary>
        /// Gets or sets the Label property.
        /// </summary>
        public string Label
        {
            get => GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }



        /// <summary>
        /// LabelWidth StyledProperty definition
        /// </summary>
        public static readonly StyledProperty<double> LabelWidthProperty =
            AvaloniaProperty.Register<FormItem, double>(nameof(LabelWidth), double.NaN);

        /// <summary>
        /// Gets or sets the LabelWidth property.
        /// </summary>
        public double LabelWidth
        {
            get => GetValue(LabelWidthProperty);
            set => SetValue(LabelWidthProperty, value);
        }
    }
}
