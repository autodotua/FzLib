using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;


namespace FzLib.Avalonia.Dialogs
{
    public partial class SelectItemDialog : DialogHost
    {
        private readonly Action buttonCommand;
        private readonly object buttonContent;
        public SelectItemDialog(string title, string message, IEnumerable<SelectDialogItem> items, object buttonContent = null, Action buttonCommand = null)
        {
            Title = title;
            Content = new SelectItemDialogContent
            {
                Message = message,
                Items = new List<SelectDialogItem>(items)
            };
            (Content as SelectItemDialogContent).SelectionChanged += ListBox_SelectionChanged;
            this.buttonContent = buttonContent;
            this.buttonCommand = buttonCommand;
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            if (buttonContent != null)
            {
                SecondaryButtonContent = buttonContent;
            }
            CloseButtonContent = CancelButtonText;
        }

        protected override void OnCloseButtonClick()
        {
            Close(null);
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

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ListBox).SelectedIndex;
            var item = (sender as ListBox).SelectedItem as SelectDialogItem;
            item.SelectAction?.Invoke();
            Close(index);
        }
    }
}