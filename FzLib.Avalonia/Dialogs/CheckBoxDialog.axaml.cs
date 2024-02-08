using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;


namespace FzLib.Avalonia.Dialogs
{
    public partial class CheckBoxDialogViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private IList<CheckDialogItem> items;
    }


    public partial class CheckBoxDialog : DialogHost
    {
        private readonly int minCheckCount;
        private readonly int maxCheckCount;

        public CheckBoxDialog(CheckBoxDialogViewModel vm, int minCheckCount,int maxCheckCount)
        {
            Title=vm.Title;
            if (minCheckCount < 0)
            {
                throw new ArgumentException("值不可小于0", nameof(minCheckCount));
            }
            if (maxCheckCount < 0)
            {
                throw new ArgumentException("值不可小于0", nameof(maxCheckCount));
            }
            if (minCheckCount > vm.Items.Count)
            {
                throw new ArgumentException("值不可大于选择项", nameof(minCheckCount));
            }
            if (maxCheckCount < minCheckCount)
            {
                throw new ArgumentException("值不可小于minCheckCount", nameof(maxCheckCount));
            }
            DataContext = vm;
            foreach (var item in vm.Items)
            {
                item.PropertyChanged += Item_PropertyChanged;
            }
            InitializeComponent();
            CheckCanApply();
            this.minCheckCount = minCheckCount;
            this.maxCheckCount = maxCheckCount;
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CheckDialogItem.IsChecked))
            {
                CheckCanApply();
            }
        }

        private void CheckCanApply()
        {
            int count = (DataContext as CheckBoxDialogViewModel).Items.Where(p => p.IsChecked).Count();
            PrimaryButtonEnable = count >= minCheckCount && count <= maxCheckCount;
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {

            PrimaryButtonContent = DialogHost.OkButtonText;
            CloseButtonContent = DialogHost.CancelButtonText;

            base.OnApplyTemplate(e);
        }

        protected override void OnPrimaryButtonClick()
        {
            Close(true);
        }
        protected override void OnSecondaryButtonClick()
        {
            throw new NotImplementedException();
        }
        protected override void OnCloseButtonClick()
        {
            Close(false);
        }
    }
}