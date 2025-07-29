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
using Avalonia;


namespace FzLib.Avalonia.Dialogs
{
    internal class CheckBoxDialogContent : ContentControl
    {
        protected override Type StyleKeyOverride { get; } = typeof(CheckBoxDialogContent);

        public static readonly StyledProperty<string> TitleProperty =
            AvaloniaProperty.Register<CheckBoxDialogContent, string>(nameof(Title));

        public static readonly StyledProperty<string> MessageProperty =
            AvaloniaProperty.Register<CheckBoxDialogContent, string>(nameof(Message));

        public static readonly StyledProperty<IList<CheckDialogItem>> ItemsProperty =
            AvaloniaProperty.Register<CheckBoxDialogContent, IList<CheckDialogItem>>(nameof(Items));

        public string Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string Message
        {
            get => GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public IList<CheckDialogItem> Items
        {
            get => GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
    }

    public partial class CheckBoxDialog : DialogHost
    {
        private readonly int minCheckCount;
        private readonly int maxCheckCount;

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
                item.PropertyChanged += Item_PropertyChanged;
            }

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
            int count = (Content as CheckBoxDialogContent).Items.Count(p => p.IsChecked);
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