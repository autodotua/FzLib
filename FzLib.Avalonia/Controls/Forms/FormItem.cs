using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Templates;

namespace FzLib.Avalonia.Controls
{
    public class FormItem : ContentControl
    {
        public static readonly StyledProperty<object> DescriptionProperty =
            AvaloniaProperty.Register<FormItem, object>(nameof(Description));

        public static readonly StyledProperty<object> LabelProperty =
            AvaloniaProperty.Register<FormItem, object>(nameof(Label));

        public static readonly StyledProperty<object> HeaderProperty =
            AvaloniaProperty.Register<FormItem, object>(nameof(Header));

        public static readonly StyledProperty<double> LabelWidthProperty =
            AvaloniaProperty.Register<FormItem, double>(nameof(LabelWidth), double.NaN);

        public static readonly StyledProperty<IDataTemplate> HeaderTemplateProperty =
            AvaloniaProperty.Register<FormItem, IDataTemplate>(
                nameof(HeaderTemplate));

        public IDataTemplate HeaderTemplate
        {
            get => GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        public static readonly StyledProperty<IDataTemplate> LabelTemplateProperty =
            AvaloniaProperty.Register<FormItem, IDataTemplate>(
                nameof(LabelTemplate));

        public IDataTemplate LabelTemplate
        {
            get => GetValue(LabelTemplateProperty);
            set => SetValue(LabelTemplateProperty, value);
        }

        public static readonly StyledProperty<IDataTemplate> DescriptionTemplateProperty =
            AvaloniaProperty.Register<FormItem, IDataTemplate>(
                nameof(DescriptionTemplate));

        public IDataTemplate DescriptionTemplate
        {
            get => GetValue(DescriptionTemplateProperty);
            set => SetValue(DescriptionTemplateProperty, value);
        }

        public object Description
        {
            get => GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public object Label
        {
            get => GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public object Header
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