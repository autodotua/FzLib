using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using Avalonia;


namespace FzLib.Avalonia.Dialogs
{
    internal class SelectItemDialogContent : ContentControl
    {
        protected override Type StyleKeyOverride { get; } = typeof(SelectItemDialogContent);

        public static readonly StyledProperty<string> MessageProperty =
            AvaloniaProperty.Register<SelectItemDialogContent, string>(nameof(Message));

        public static readonly StyledProperty<IList<SelectDialogItem>> ItemsProperty =
            AvaloniaProperty.Register<SelectItemDialogContent, IList<SelectDialogItem>>(nameof(Items));
        
        public string Message
        {
            get => GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public IList<SelectDialogItem> Items
        {
            get => GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
    }


    public partial class SelectItemDialog : DialogHost
    {
        private readonly object buttonContent;
        private readonly Action buttonCommand;

        public SelectItemDialog(string title, string message, IEnumerable<SelectDialogItem> items, object buttonContent = null, Action buttonCommand = null)
        {
            Title = title;
            Content = new SelectItemDialogContent
            {
                Message = message,
                Items = new List<SelectDialogItem>(items)
            };
            this.buttonContent = buttonContent;
            this.buttonCommand = buttonCommand;
        }
      

        private void DialogWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }
        
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index=(sender as ListBox).SelectedIndex;
            var item=(sender as ListBox).SelectedItem as SelectDialogItem;
            item.SelectAction?.Invoke();
            Close(index);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            if (buttonContent != null)
            {
                SecondaryButtonContent = buttonContent;
            }
            CloseButtonContent = DialogHost.CancelButtonText;

            base.OnApplyTemplate(e);
        }

        protected override void OnPrimaryButtonClick()
        {
            throw new NotImplementedException();
        }

        protected override void OnSecondaryButtonClick()
        {
            buttonCommand?.Invoke();
            Close(null);
        }

        protected override void OnCloseButtonClick()
        {
            Close(null);
        }
    }
}