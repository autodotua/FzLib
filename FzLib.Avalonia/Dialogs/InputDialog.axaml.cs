using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
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


    public partial class InputDialog : CommonDialogWindow
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
                  if (typeof(T) == typeof(int))
                  {
                      if (!int.TryParse(text, out _))
                      {
                          throw new ArgumentException("无法转为数字");
                      }
                  }
                  else if (typeof(T) == typeof(short))
                  {
                      if (!short.TryParse(text, out _))
                      {
                          throw new ArgumentException("无法转为数字");
                      }
                  }
                  else if (typeof(T) == typeof(long))
                  {
                      if (!long.TryParse(text, out _))
                      {
                          throw new ArgumentException("无法转为数字");
                      }
                  }
                  else if (typeof(T) == typeof(float))
                  {
                      if (!float.TryParse(text, out _))
                      {
                          throw new ArgumentException("无法转为数字");
                      }
                  }
                  else if (typeof(T) == typeof(double))
                  {
                      if (!double.TryParse(text, out _))
                      {
                          throw new ArgumentException("无法转为数字");
                      }
                  }
                  else if (typeof(T) == typeof(uint))
                  {
                      if (!uint.TryParse(text, out _))
                      {
                          throw new ArgumentException("无法转为数字");
                      }
                  }
                  else if (typeof(T) == typeof(ushort))
                  {
                      if (!ushort.TryParse(text, out _))
                      {
                          throw new ArgumentException("无法转为数字");
                      }
                  }
                  else if (typeof(T) == typeof(ulong))
                  {
                      if (!ulong.TryParse(text, out _))
                      {
                          throw new ArgumentException("无法转为数字");
                      }
                  }
                  else if (typeof(T) == typeof(byte))
                  {
                      if (!byte.TryParse(text, out _))
                      {
                          throw new ArgumentException("无法转为数字");
                      }
                  }
                  else if (typeof(T) == typeof(sbyte))
                  {
                      if (!sbyte.TryParse(text, out _))
                      {
                          throw new ArgumentException("无法转为数字");
                      }
                  }
                  // 添加其他数字类型的处理
                  else
                  {
                      throw new ArgumentException("不支持的数字类型");
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
                    (Content as DialogWrapper).PrimaryButtonEnable = false;
                    break;
                }
            }
        }

        private void Vm_ValidationError(object sender, EventArgs e)
        {
            (Content as DialogWrapper).PrimaryButtonEnable = false;
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (Content as DialogWrapper).PrimaryButtonEnable = true;
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
            (Content as DialogWrapper).PrimaryButtonContent = DialogWrapper.OkButtonText;
            (Content as DialogWrapper).CloseButtonContent = DialogWrapper.CancelButtonText;

            base.OnApplyTemplate(e);
        }
    }
}