using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;


namespace FzLib.Avalonia.Dialogs
{
    public partial class CheckBoxDialog : DialogHost
    {
        private readonly int maxCheckCount;
        private readonly int minCheckCount;
        public CheckBoxDialog(string title, string message, IEnumerable<CheckDialogItem> items, int minCheckCount = 1,
            int maxCheckCount = int.MaxValue)
        {
            Title = title;
            var itemList = items.ToList();
            Content = new CheckBoxDialogContent
            {
                Message = message,
                Items = itemList
            };
            if (minCheckCount < 0)
            {
                throw new ArgumentException("值不可小于0", nameof(minCheckCount));
            }

            if (maxCheckCount < 0)
            {
                throw new ArgumentException("值不可小于0", nameof(maxCheckCount));
            }

            if (maxCheckCount < minCheckCount)
            {
                throw new ArgumentException("值不可小于minCheckCount", nameof(maxCheckCount));
            }

            foreach (var item in itemList)
            {
                item.IsCheckedChanged += (s, e) => CheckCanApply();
            }

            this.minCheckCount = minCheckCount;
            this.maxCheckCount = maxCheckCount;
            CheckCanApply();
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            PrimaryButtonContent = DialogHost.OkButtonText;
            CloseButtonContent = DialogHost.CancelButtonText;

            base.OnApplyTemplate(e);
        }

        protected override void OnCloseButtonClick()
        {
            Close(false);
        }

        protected override void OnPrimaryButtonClick()
        {
            Close(true);
        }

        protected override void OnSecondaryButtonClick()
        {
            throw new NotImplementedException();
        }

        private void CheckCanApply()
        {
            int count = (Content as CheckBoxDialogContent).Items.Count(p => p.IsChecked);
            IsPrimaryButtonEnabled = count >= minCheckCount && count <= maxCheckCount;
        }
    }
}