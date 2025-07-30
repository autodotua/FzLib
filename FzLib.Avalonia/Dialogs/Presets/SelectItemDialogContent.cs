using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using Avalonia;


namespace FzLib.Avalonia.Dialogs
{
    internal class SelectItemDialogContent : ContentControl
    {
        public static readonly StyledProperty<IList<SelectDialogItem>> ItemsProperty =
            AvaloniaProperty.Register<SelectItemDialogContent, IList<SelectDialogItem>>(nameof(Items));

        public static readonly StyledProperty<string> MessageProperty =
            AvaloniaProperty.Register<SelectItemDialogContent, string>(nameof(Message));

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        public IList<SelectDialogItem> Items
        {
            get => GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public string Message
        {
            get => GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        protected override Type StyleKeyOverride { get; } = typeof(SelectItemDialogContent);
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            var lst = e.NameScope.Find<ListBox>("PART_ListBox");
            lst.SelectionChanged += SelectionChanged;

        }
    }
}