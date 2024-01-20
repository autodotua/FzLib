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


namespace FzLib.Avalonia.Dialogs
{
    public partial class SelectItemDialogViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private IList<SelectDialogItem> items;
    }


    public partial class SelectItemDialog : CommonDialogWindow
    {
        private readonly object buttonContent;
        private readonly Action buttonCommand;

        internal SelectItemDialog(SelectItemDialogViewModel vm, object buttonContent, Action buttonCommand)
        {
            DataContext = vm;
            InitializeComponent();
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
                (Content as DialogWrapper).SecondaryButtonContent = buttonContent;
            }
            (Content as DialogWrapper).CloseButtonContent = DialogWrapper.CancelButtonText;

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