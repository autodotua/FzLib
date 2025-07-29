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


namespace FzLib.Avalonia.Dialogs
{
    internal class InputDialogContent : ContentControl
    {
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
                nameof(Text),
                coerce: OnTextCoerce);

        public static readonly DirectProperty<InputDialogContent, List<Action<string>>> ValidationsProperty =
            AvaloniaProperty.RegisterDirect<InputDialogContent, List<Action<string>>>(
                nameof(Validations),
                o => o.Validations);

        public event EventHandler ValidationError;

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

        public List<Action<string>> Validations { get; init; } = new List<Action<string>>();

        private static string OnTextCoerce(AvaloniaObject obj, string value)
        {
            if (obj is InputDialogContent control)
            {
                try
                {
                    foreach (var validation in control.Validations)
                    {
                        validation?.Invoke(value);
                    }

                    return value;
                }
                catch (Exception ex)
                {
                    control.ValidationError?.Invoke(control, EventArgs.Empty);
                    throw new ValidationException(ex.Message);
                }
            }

            return value;
        }

        public class ValidationException : Exception
        {
            public ValidationException(string message) : base(message)
            {
            }

            public override string ToString()
            {
                return Message;
            }
        }
    }


    public partial class InputDialog : DialogHost
    {
        internal static readonly Action<string> NotNullValidation = text =>
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("输入为空");
            }
        };

        internal static Action<string> GetNumberValidation<T>() where T : INumber<T>
        {
            return text =>
            {
                if (!T.TryParse(text, CultureInfo.InvariantCulture, out _))
                {
                    throw new ArgumentException("无法转为数字");
                }
            };
        }

        public InputDialog(string title, string message, string defaultText = null,
            bool multiLines = false, int minLines = 1, int maxLines = 10,
            string watermark = null, char passwordChar = '\0',
            IEnumerable<Action<string>> validations = null)
        {
            Title = title;
            var content = new InputDialogContent
            {
                Message = message,
                Text = defaultText,
                MultiLines = multiLines,
                Watermark = watermark,
                PasswordChar = passwordChar,
                MinLines = minLines,
                MaxLines = maxLines,
                Validations = validations?.ToList() ?? new List<Action<string>>()
            };
            Content = content;
            content.ValidationError += OnValidationError;
            foreach (var v in content.Validations)
            {
                try
                {
                    v?.Invoke(content.Text);
                }
                catch
                {
                    PrimaryButtonEnable = false;
                    break;
                }
            }
        }

        private void OnValidationError(object sender, EventArgs e)
        {
            PrimaryButtonEnable = false;
        }


        private void DialogWindow_Loaded(object sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        {
            // txt.Focus();
            // if (!string.IsNullOrEmpty((DataContext as InputDialogViewModel).Text))
            // {
            //     txt.SelectAll();
            // }
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            PrimaryButtonContent = DialogHost.OkButtonText;
            CloseButtonContent = DialogHost.CancelButtonText;

            base.OnApplyTemplate(e);
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