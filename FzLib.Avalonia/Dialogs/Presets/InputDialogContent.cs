using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Data;


namespace FzLib.Avalonia.Dialogs
{
    internal class InputDialogContent : ContentControl
    {

        public static readonly DirectProperty<InputDialogContent, string> ErrorMessageProperty =
            AvaloniaProperty.RegisterDirect<InputDialogContent, string>(
                nameof(ErrorMessage), o => o.ErrorMessage, (o, v) => o.ErrorMessage = v);

        public static readonly DirectProperty<InputDialogContent, bool> HasErrorProperty =
            AvaloniaProperty.RegisterDirect<InputDialogContent, bool>(
                nameof(HasError), o => o.HasError, (o, v) => o.HasError = v);

        public static readonly StyledProperty<int> MaxLinesProperty =
            AvaloniaProperty.Register<InputDialogContent, int>(nameof(MaxLines));

        public static readonly StyledProperty<string> MessageProperty =
            AvaloniaProperty.Register<InputDialogContent, string>(nameof(Message));

        public static readonly StyledProperty<int> MinLinesProperty =
            AvaloniaProperty.Register<InputDialogContent, int>(nameof(MinLines));

        public static readonly StyledProperty<bool> MultiLinesProperty =
            AvaloniaProperty.Register<InputDialogContent, bool>(nameof(MultiLines));

        public static readonly StyledProperty<char> PasswordCharProperty =
            AvaloniaProperty.Register<InputDialogContent, char>(nameof(PasswordChar), '\0');

        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<InputDialogContent, string>(
                nameof(Text), defaultBindingMode: BindingMode.TwoWay);

        public static readonly StyledProperty<string> TitleProperty =
            AvaloniaProperty.Register<InputDialogContent, string>(nameof(Title));

        public static readonly DirectProperty<InputDialogContent, List<Func<string, ValidationResult>>>
            ValidationsProperty =
                AvaloniaProperty.RegisterDirect<InputDialogContent, List<Func<string, ValidationResult>>>(
                    nameof(Validations),
                    o => o.Validations);

        public static readonly StyledProperty<string> WatermarkProperty =
            AvaloniaProperty.Register<InputDialogContent, string>(nameof(Watermark));

        private string errorMessage;

        private bool hasError;

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

        public int MaxLines
        {
            get => GetValue(MaxLinesProperty);
            set => SetValue(MaxLinesProperty, value);
        }

        public string Message
        {
            get => GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public int MinLines
        {
            get => GetValue(MinLinesProperty);
            set => SetValue(MinLinesProperty, value);
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

        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public List<Func<string, ValidationResult>> Validations { get; init; }

        public string Watermark
        {
            get => GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        protected override Type StyleKeyOverride { get; } = typeof(InputDialogContent);

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            this.GetObservable(TextProperty).Subscribe(OnTextChanged);

            var txt = e.NameScope.Find<TextBox>("PART_TextBox");
            txt.Focus();
            txt.SelectAll();
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
    }
}