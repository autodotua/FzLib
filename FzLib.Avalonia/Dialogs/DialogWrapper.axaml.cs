using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Dialogs
{
    internal class DialogWrapper : ContentControl
    {
        public static string OkButtonText = "确定";
        public static string YesButtonText = "是";
        public static string NoButtonText = "否";
        public static string CancelButtonText = "取消";
        public static string CloseButtonText = "关闭";
        internal Button PrimaryButton;
        internal Button SecondaryButton;
        internal Button CloseButton;
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            PrimaryButton = e.NameScope.Find(nameof(PrimaryButton)) as Button;
            SecondaryButton = e.NameScope.Find(nameof(SecondaryButton)) as Button;
            CloseButton = e.NameScope.Find(nameof(CloseButton)) as Button;
            base.OnApplyTemplate(e);
        }
        public static readonly StyledProperty<object> PrimaryButtonContentProperty =
    AvaloniaProperty.Register<DialogWrapper, object>(nameof(PrimaryButtonContent));
        public object PrimaryButtonContent
        {
            get => GetValue(PrimaryButtonContentProperty);
            set => SetValue(PrimaryButtonContentProperty, value);
        }
        public static readonly StyledProperty<object> SecondaryButtonContentProperty =
    AvaloniaProperty.Register<DialogWrapper, object>(nameof(SecondaryButtonContent));
        public object SecondaryButtonContent
        {
            get => GetValue(SecondaryButtonContentProperty);
            set => SetValue(SecondaryButtonContentProperty, value);
        }
        public static readonly StyledProperty<object> CloseButtonContentProperty =
    AvaloniaProperty.Register<DialogWrapper, object>(nameof(CloseButtonContent));
        public object CloseButtonContent
        {
            get => GetValue(CloseButtonContentProperty);
            set => SetValue(CloseButtonContentProperty, value);
        }

    }
}
