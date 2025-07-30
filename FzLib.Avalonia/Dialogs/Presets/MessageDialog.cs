using System;
using Avalonia.Controls.Primitives;
using Avalonia.Media;


namespace FzLib.Avalonia.Dialogs
{
    internal class MessageDialog : DialogHost
    {
        public MessageDialog(MessageDialogButtonDefinition buttonDefinition,
            string title,
            string message,
            string detail = null,
            string icon = null,
            IBrush iconBrush = null)
        {
            ButtonDefinition = buttonDefinition;
            Title = title;
            Content = new MessageDialogContent
            {
                Message = message,
                Detail = detail,
                Icon = icon,
                IconBrush = iconBrush
            };
        }

        public enum MessageDialogButtonDefinition
        {
            OK,
            YesNo,
            YesNoCancel,
            RetryCancel,
        }

        public MessageDialogButtonDefinition ButtonDefinition { get; }
        protected override Type StyleKeyOverride { get; } = typeof(DialogHost);
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            switch (ButtonDefinition)
            {
                case MessageDialogButtonDefinition.OK:
                    CloseButtonContent = OkButtonText;
                    break;
                case MessageDialogButtonDefinition.YesNo:
                    PrimaryButtonContent = YesButtonText;
                    SecondaryButtonContent = NoButtonText;
                    break;
                case MessageDialogButtonDefinition.YesNoCancel:
                    PrimaryButtonContent = YesButtonText;
                    SecondaryButtonContent = NoButtonText;
                    CloseButtonContent = CancelButtonText;
                    break;
                case MessageDialogButtonDefinition.RetryCancel:
                    PrimaryButtonContent = RetryButtonText;
                    CloseButtonContent = CancelButtonText;
                    break;
            }

            base.OnApplyTemplate(e);
        }

        protected override void OnCloseButtonClick()
        {
            Close(null);
        }

        protected override void OnPrimaryButtonClick()
        {
            Close(true);
        }

        protected override void OnSecondaryButtonClick()
        {
            Close(false);
        }
    }
}