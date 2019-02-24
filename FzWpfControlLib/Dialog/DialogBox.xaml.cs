using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FzLib.Control.Dialog
{
    /// <summary>
    /// DialogBox.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBox : Window
    {
        

        public MessageBox(DialogType type,Window owner=null)
        {
            InitializeComponent();
            DialogType = type;
            Owner = owner;
        }

        public DialogType DialogType
        {
            set
            {
                UpdateColor(value);
            }
        }

        public void SetMessage(string message)
        {
            tbkMessage.Text = message;
        }
        public void SetDetail(string message)
        {
            if(message=="" && message==null)
            {
                tbkDetail.Visibility = Visibility.Collapsed;
                return;
            }
            tbkDetail.Visibility = Visibility.Visible;
            tbkDetail.Text = message;
        }

        public void AddButton(string text)
        {
            Button btn = new Button()
            {
                Content=text,
                Tag = stk.Children.Count,
            };
            btn.Click += BtnClickEventHandler;
            stk.Children.Add(btn);
        }

        private void BtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            ResultIndex = (int)((sender as Button).Tag);
            ResultText = (sender as Button).Content as string;
            Close();
        }

        public int ResultIndex { get; private set; }
        public string ResultText { get; private set; }

      

       public  readonly static Dictionary<DialogType, SolidColorBrush> TypeToColor = new Dictionary<DialogType, SolidColorBrush>()
        {
            {DialogType.Information,new SolidColorBrush(Color.FromRgb(0xB3,0xFF,0xB6)) },
            {DialogType.Error,new SolidColorBrush(Color.FromRgb(0xFF,0xB3,0xB3)) },
            {DialogType.Warn,new SolidColorBrush(Color.FromRgb(0xFF,0xCC,0x99)) },

        };
        public void UpdateColor(DialogType type)
        {
            UpdateColor(TypeToColor[type]);
        }

        private void UpdateColor(SolidColorBrush color)
        {
            Resources["backgroundBrushColor"] = color;
            Resources["darker1BrushColor"] = new SolidColorBrush(Color.FromScRgb(color.Color.ScA, color.Color.ScR * 0.9f, color.Color.ScG * 0.9f, color.Color.ScB * 0.9f));
            Resources["darker2BrushColor"] = new SolidColorBrush(Color.FromScRgb(color.Color.ScA, color.Color.ScR * 0.8f, color.Color.ScG * 0.8f, color.Color.ScB * 0.8f));
            Resources["darker3BrushColor"] = new SolidColorBrush(Color.FromScRgb(color.Color.ScA, color.Color.ScR * 0.7f, color.Color.ScG * 0.7f, color.Color.ScB * 0.7f));
            Resources["darker4BrushColor"] = new SolidColorBrush(Color.FromScRgb(color.Color.ScA, color.Color.ScR * 0.6f, color.Color.ScG * 0.6f, color.Color.ScB * 0.6f));



        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Topmost = false;
        }




        public static Window DefaultDialogOwner { get; set; } = null;

        public static int ShowPrompt(string message, Window owner = null)
        {
            return ShowMessage(message, DialogType.Information);
        }
        public static int ShowError(string message, Window owner = null)
        {
            return ShowMessage(message, DialogType.Error);
        }
        public static int ShowWarn(string message, Window owner = null)
        {
            return ShowMessage(message, DialogType.Warn);
        }
        public static bool ShowYesNo(string message, Window owner = null)
        {
            return ShowMessage(message, DialogType.Warn, MessageBoxButton.YesNo) == 0;
        }

        public static int ShowException(Exception ex, bool onlyShowMessage = true, Window owner = null)
        {
            return ShowException("程序发生异常", ex, onlyShowMessage, owner);
        }
        public static int ShowException(string text, Exception ex, bool onlyShowMessage = true, Window owner = null)
        {
            return ShowMessage(text + "：" + Environment.NewLine, onlyShowMessage ? ex.Message : ex.ToString(), DialogType.Error, owner);
        }

        public static int ShowMessage(string message, DialogType type, Window owner = null)
        {
            return ShowMessage(message, null, type, owner);
        }
        public static int ShowMessage(string message, string detial, DialogType type, Window owner = null)
        {
            return ShowMessage(message, detial, type, new string[] { "确定" });
        }
        public static int ShowMessage(string message, DialogType type, IEnumerable<string> buttonTexts, Window owner = null)
        {
            return ShowMessage(message, null, type, buttonTexts, owner);
        }
        public static int ShowMessage(string message, string detial, DialogType type, IEnumerable<string> buttonTexts, Window owner = null)
        {
            MessageBox box = new MessageBox(type, owner ?? DefaultDialogOwner);
            box.SetMessage(message);
            box.SetDetail(detial);
            foreach (var i in buttonTexts)
            {
                box.AddButton(i);
            }
            box.ShowDialog();
            return box.ResultIndex;
        }
        public static int ShowMessage(string message, DialogType type, MessageBoxButton buttons, Window owner = null)
        {

            return ShowMessage(message, "", type, buttons, owner);
        }

        public static int ShowMessage(string message, string detail, DialogType type, MessageBoxButton buttons, Window owner = null)
        {
            //DialogBox box = new DialogBox(type, owner ?? DefautDialogOwner);
            List<string> str = new List<string>();
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    str.Add("确定");
                    break;
                case MessageBoxButton.OKCancel:
                    str.Add("确定");
                    str.Add("关闭");
                    break;
                case MessageBoxButton.YesNo:
                    str.Add("是");
                    str.Add("否");
                    break;
                case MessageBoxButton.YesNoCancel:
                    str.Add("是");
                    str.Add("否");
                    str.Add("取消");
                    break;
            }
            return ShowMessage(message, detail, type, str, owner);
        }
    }

    public enum DialogType
    {
        Information,
        Error,
        Warn
    }





}
