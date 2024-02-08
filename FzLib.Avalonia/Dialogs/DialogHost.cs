using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public static readonly StyledProperty<string> TitleProperty =
    AvaloniaProperty.Register<DialogHost, string>(nameof(Title), "");

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

        public string Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public void Close()
        {
            dialogContainer.Close();
        }

        public void Close(object result)
        {
            dialogContainer.Close(result);
        }

        public Task ShowDialog(DialogContainerType type, Visual visual)
        {
            return ShowDialog<object>(type, visual);
        }

        public Task<T> ShowDialog<T>(DialogContainerType type, Visual visual)
        {
            var topLevel = TopLevel.GetTopLevel(visual) ?? throw new ArgumentException("找不到TopLevel", nameof(visual));
            bool canWindowDialog = topLevel is Window;//在桌面端，TopLevel是窗口
            Grid grid = null;
            if (topLevel.Content is Grid g)
            {
                grid = g;
            }
            else if (topLevel.Content is ContentControl cc && cc.Content is Grid g2)
            {
                grid = g2;
            }
            bool canPopupDialog = grid != null;
            switch (type)
            {
                case DialogContainerType.Popup:
                    if (!canPopupDialog) throw new NotSupportedException("未找到顶层Grid，不支持Popup对话框");
                    return ShowPopupDialog<T>(grid);
                case DialogContainerType.Window:
                    if (!canWindowDialog) throw new NotSupportedException("顶层不是Window，不支持Window对话框");
                    return ShowWindowDialog<T>(topLevel as Window);
                case DialogContainerType.PopupPreferred:
                    if (!(canWindowDialog || canPopupDialog)) throw new NotSupportedException("顶层不是Window，也未找到顶层Grid，无法显示对话框");
                    if (canPopupDialog)
                    {
                        return ShowPopupDialog<T>(grid);
                    }
                    else
                    {
                        return ShowWindowDialog<T>(topLevel as Window);
                    }
                case DialogContainerType.WindowPreferred:
                    if (!(canWindowDialog || canPopupDialog)) throw new NotSupportedException("顶层不是Window，也未找到顶层Grid，无法显示对话框");
                    if (canWindowDialog)
                    {
                        return ShowWindowDialog<T>(topLevel as Window);
                    }
                    else
                    {
                        return ShowPopupDialog<T>(grid);
                    }

                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public Task ShowPopupDialog(Grid control)
        {
            return ShowPopupDialog<object>(control);
        }

        public Task<T> ShowPopupDialog<T>(Grid control)
        {
            dialogContainer = new PopupDialogContainer();
            return (dialogContainer as PopupDialogContainer).ShowDialog<T>(control, this);
        }

        public Task<T> ShowWindowDialog<T>(Window window)
        {
            dialogContainer = new WindowDialogContainer();
            return (dialogContainer as WindowDialogContainer).ShowDialog<T>(window, this);
        }

        public Task ShowWindowDialog(Window window)
        {
            return ShowWindowDialog<object>(window);
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
