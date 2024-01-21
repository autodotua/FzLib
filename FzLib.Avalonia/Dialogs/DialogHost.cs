using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Dialogs
{
    public class DialogHost : ContentControl
    {
        public static readonly StyledProperty<object> CloseButtonContentProperty =
    AvaloniaProperty.Register<DialogHost, object>(nameof(CloseButtonContent));

        public static readonly StyledProperty<bool> CloseButtonEnableProperty =
    AvaloniaProperty.Register<DialogHost, bool>(nameof(CloseButtonEnable), true);

        public static readonly StyledProperty<object> PrimaryButtonContentProperty =
    AvaloniaProperty.Register<DialogHost, object>(nameof(PrimaryButtonContent));

        public static readonly StyledProperty<bool> PrimaryButtonEnableProperty =
AvaloniaProperty.Register<DialogHost, bool>(nameof(PrimaryButtonEnable), true);

        public static readonly StyledProperty<object> SecondaryButtonContentProperty =
    AvaloniaProperty.Register<DialogHost, object>(nameof(SecondaryButtonContent));

        public static readonly StyledProperty<bool> SecondaryButtonEnableProperty =
    AvaloniaProperty.Register<DialogHost, bool>(nameof(SecondaryButtonEnable), true);

        public static string CancelButtonText = "取消";
        public static string CloseButtonText = "关闭";
        public static string NoButtonText = "否";
        public static string OkButtonText = "确定";
        public static string RetryButtonText = "重试";
        public static string YesButtonText = "是";
        internal Button CloseButton;
        internal Button PrimaryButton;
        internal Button SecondaryButton;
        private IDialogHostContainer dialogContainer;

        public object CloseButtonContent
        {
            get => GetValue(CloseButtonContentProperty);
            set => SetValue(CloseButtonContentProperty, value);
        }

        public bool CloseButtonEnable
        {
            get => GetValue(CloseButtonEnableProperty);
            set => SetValue(CloseButtonEnableProperty, value);
        }

        public object PrimaryButtonContent
        {
            get => GetValue(PrimaryButtonContentProperty);
            set => SetValue(PrimaryButtonContentProperty, value);
        }

        public bool PrimaryButtonEnable
        {
            get => GetValue(PrimaryButtonEnableProperty);
            set => SetValue(PrimaryButtonEnableProperty, value);
        }

        public object SecondaryButtonContent
        {
            get => GetValue(SecondaryButtonContentProperty);
            set => SetValue(SecondaryButtonContentProperty, value);
        }

        public bool SecondaryButtonEnable
        {
            get => GetValue(SecondaryButtonEnableProperty);
            set => SetValue(SecondaryButtonEnableProperty, value);
        }

        public void Close()
        {
            dialogContainer.Close();
        }

        public void Close(object result)
        {
            dialogContainer.Close(result);
        }

        public Task<T> ShowWindowDialog<T>(Window window)
        {
            dialogContainer = new WindowDialogContainer();
            return (dialogContainer as WindowDialogContainer).ShowDialog<T>(window, this);
        }

        public Task ShowWindowDialog(Window window)
        {
            dialogContainer = new WindowDialogContainer();
            return (dialogContainer as WindowDialogContainer).ShowDialog(window, this);
        }

        public Task ShowPopupDialog(Grid control)
        {
            dialogContainer = new PopupDialogContainer();
            return (dialogContainer as PopupDialogContainer).ShowDialog(control, this);
        }

        public Task<T> ShowPopupDialog<T>(Grid control)
        {
            dialogContainer = new PopupDialogContainer();
            return (dialogContainer as PopupDialogContainer).ShowDialog<T>(control, this);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            PrimaryButton = e.NameScope.Find(nameof(PrimaryButton)) as Button;
            SecondaryButton = e.NameScope.Find(nameof(SecondaryButton)) as Button;
            CloseButton = e.NameScope.Find(nameof(CloseButton)) as Button;
            PrimaryButton.Click += (s, e) => OnPrimaryButtonClick();
            SecondaryButton.Click += (s, e) => OnSecondaryButtonClick();
            CloseButton.Click += (s, e) => OnCloseButtonClick();
            base.OnApplyTemplate(e);
        }
        protected virtual void OnCloseButtonClick() { }
        protected virtual void OnPrimaryButtonClick() { }
        protected virtual void OnSecondaryButtonClick() { }
    }
}
