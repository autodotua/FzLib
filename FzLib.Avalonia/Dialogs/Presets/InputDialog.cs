using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Avalonia;


namespace FzLib.Avalonia.Dialogs
{
    public partial class InputDialog : DialogHost
    {
        internal static readonly Func<string, ValidationResult> NotNullValidation = text =>
        {
            return string.IsNullOrWhiteSpace(text) ? ValidationResult.Error("输入为空") : ValidationResult.Valid();
        };

        public InputDialog(string title, string message, string defaultText = null,
            bool multiLines = false, int minLines = 1, int maxLines = 10,
            string watermark = null, char passwordChar = '\0',
            IEnumerable<Func<string, ValidationResult>> validations = null)
        {
            PrimaryButtonEnable = true;
            Title = title;
            var content = new InputDialogContent
            {
                Validations = validations?.ToList(), //先设置Validation在设置默认值
                Message = message,
                Text = defaultText,
                MultiLines = multiLines,
                Watermark = watermark,
                PasswordChar = passwordChar,
                MinLines = minLines,
                MaxLines = maxLines,
            };
            Content = content;
            content.GetObservable(InputDialogContent.HasErrorProperty).Subscribe(OnValidationChanged);
        }

        internal static Func<string, ValidationResult> GetNumberValidation<T>() where T : INumber<T>
        {
            return text =>
            {
                if (!T.TryParse(text, CultureInfo.InvariantCulture, out _))
                {
                    return ValidationResult.Error("无法转为数字");
                }

                return ValidationResult.Valid();
            };
        }
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            PrimaryButtonContent = OkButtonText;
            CloseButtonContent = CancelButtonText;
        }

        protected override void OnCloseButtonClick()
        {
            Close(null);
        }

        protected override void OnPrimaryButtonClick()
        {
            Debug.Assert(PrimaryButtonEnable);
            Close((Content as InputDialogContent).Text);
        }

        protected override void OnSecondaryButtonClick()
        {
            throw new NotImplementedException();
        }

        private void OnValidationChanged(bool hasError)
        {
            PrimaryButtonEnable = !hasError;
        }
    }
}