using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Avalonia;
using Avalonia.Data;


namespace FzLib.Avalonia.Dialogs
{
    internal class InputDialogContent : ContentControl
    {
        public InputDialogContent()
        {
            this.GetObservable(TextProperty).Subscribe(OnTextChanged);
        }

        private void OnTextChanged(string text)
        {
            if (Validations == null || Validations.Count == 0)
            {
                return;
            }

            foreach (var validation in Validations.Where(p => p != null))
            {
                var ex = validation(text);
                if (!ex.IsValid)
                {
                    HasError = true;
                    ErrorMessage = ex.ErrorMessage;
                    return;
                }
            }

            HasError = false;
        }

        protected override Type StyleKeyOverride { get; } = typeof(InputDialogContent);

        public static readonly StyledProperty<string> TitleProperty =
            AvaloniaProperty.Register<InputDialogContent, string>(nameof(Title));

        public static readonly StyledProperty<string> MessageProperty =
            AvaloniaProperty.Register<InputDialogContent, string>(nameof(Message));

        public static readonly StyledProperty<string> WatermarkProperty =
            AvaloniaProperty.Register<InputDialogContent, string>(nameof(Watermark));

        public static readonly StyledProperty<bool> MultiLinesProperty =
            AvaloniaProperty.Register<InputDialogContent, bool>(nameof(MultiLines));

        public static readonly StyledProperty<char> PasswordCharProperty =
            AvaloniaProperty.Register<InputDialogContent, char>(nameof(PasswordChar), '\0');

        public static readonly StyledProperty<int> MinLinesProperty =
            AvaloniaProperty.Register<InputDialogContent, int>(nameof(MinLines));

        public static readonly StyledProperty<int> MaxLinesProperty =
            AvaloniaProperty.Register<InputDialogContent, int>(nameof(MaxLines));

        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<InputDialogContent, string>(
                nameof(Text), defaultBindingMode: BindingMode.TwoWay);

        public static readonly DirectProperty<InputDialogContent, List<Func<string, ValidationResult>>>
            ValidationsProperty =
                AvaloniaProperty.RegisterDirect<InputDialogContent, List<Func<string, ValidationResult>>>(
                    nameof(Validations),
                    o => o.Validations);

        private bool hasError;

        public static readonly DirectProperty<InputDialogContent, bool> HasErrorProperty =
            AvaloniaProperty.RegisterDirect<InputDialogContent, bool>(
                nameof(HasError), o => o.HasError, (o, v) => o.HasError = v);

        private string errorMessage;

        public static readonly DirectProperty<InputDialogContent, string> ErrorMessageProperty =
            AvaloniaProperty.RegisterDirect<InputDialogContent, string>(
                nameof(ErrorMessage), o => o.ErrorMessage, (o, v) => o.ErrorMessage = v);

        public string ErrorMessage
        {
            get => errorMessage;
            set => SetAndRaise(ErrorMessageProperty, ref errorMessage, value);
        }

        public bool HasError
        {
            get => hasError;
            set => SetAndRaise(HasErrorProperty, ref hasError, value);
        }

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

        public string Watermark
        {
            get => GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public bool MultiLines
        {
            get => GetValue(MultiLinesProperty);
            set => SetValue(MultiLinesProperty, value);
        }

        public char PasswordChar
        {
            get => GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }

        public int MaxLines
        {
            get => GetValue(MaxLinesProperty);
            set => SetValue(MaxLinesProperty, value);
        }

        public int MinLines
        {
            get => GetValue(MinLinesProperty);
            set => SetValue(MinLinesProperty, value);
        }

        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public List<Func<string, ValidationResult>> Validations { get; init; }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            var txt = e.NameScope.Find<TextBox>("PART_TextBox");
            txt.Focus();
            txt.SelectAll();
        }
    }


    public partial class InputDialog : DialogHost
    {
        internal static readonly Func<string, ValidationResult> NotNullValidation = text =>
        {
            return string.IsNullOrWhiteSpace(text) ? ValidationResult.Error("输入为空") : ValidationResult.Valid();
        };

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

        private void OnValidationChanged(bool hasError)
        {
            PrimaryButtonEnable = !hasError;
        }


        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            PrimaryButtonContent = OkButtonText;
            CloseButtonContent = CancelButtonText;
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

        protected override void OnCloseButtonClick()
        {
            Close(null);
        }
    }
}