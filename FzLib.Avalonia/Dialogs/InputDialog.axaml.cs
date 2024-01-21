using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;


namespace FzLib.Avalonia.Dialogs
{
    public partial class InputDialogViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private string watermark;

        [ObservableProperty]
        private bool multiLines;

        [ObservableProperty]
        private char passwordChar = '\0';

        [ObservableProperty]
        private double minHeight;

        [ObservableProperty]
        private int maxLines;

        internal string text;

        public string Text
        {
            get => text;
            set
            {
                OnPropertyChanging(nameof(text));
                try
                {
                    foreach (var v in Validations)
                    {
                        v?.Invoke(value);
                    }
                }
                catch (Exception ex)
                {
                    ValidationError?.Invoke(this, EventArgs.Empty);
                    throw new ValidationException(ex.Message);
                }
                text = value;
                OnPropertyChanged(nameof(text));
            }
        }

        public List<Action<string>> Validations { get; } = new List<Action<string>>();

        public event EventHandler ValidationError;

        public class ValidationException(string message) : Exception(message)
        {
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
                  if(!T.TryParse(text,CultureInfo.InvariantCulture,out _))
                  {
                      throw new ArgumentException("无法转为数字");
                  }
              };
        }

        internal InputDialog(InputDialogViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
            vm.PropertyChanged += Vm_PropertyChanged;
            vm.ValidationError += Vm_ValidationError;
            foreach (var v in vm.Validations)
            {
                try
                {
                    v?.Invoke(vm.Text);
                }
                catch
                {
                    PrimaryButtonEnable = false;
                    break;
                }
            }
        }

        private void Vm_ValidationError(object sender, EventArgs e)
        {
            PrimaryButtonEnable = false;
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PrimaryButtonEnable = true;
        }

        private void DialogWindow_Loaded(object sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        {
            txt.Focus();
            if (!string.IsNullOrEmpty((DataContext as InputDialogViewModel).Text))
            {
                txt.SelectAll();
            }
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
            Close((DataContext as InputDialogViewModel).text);
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