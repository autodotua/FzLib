using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static FzLib.UI.Dialog.MessageBox;

namespace FzLib.UI.Dialog
{
    /// <summary>
    /// DialogBox.xaml 的交互逻辑
    /// </summary>
    public partial class InputBox : Window
    {
        string regex;

        public InputBox(string message, Window owner , SolidColorBrush color=null,string defaultText="",string regex=".*")
        {
            InitializeComponent();
            Owner = owner;
            tbkMessage.Text = message;
            if (color != null)
            {
                UpdateColor(color);
            }
            txt.Text = defaultText;
            txt.SelectAll();
            this.regex = regex;
            txt.Focus();
        }

        //private void BtnOkClickEventHandler(object sender, RoutedEventArgs e)
        //{
        //    if(!Regex.IsMatch(txt.Text,regex))
        //    {
        //        DialogHelper.ShowError("输入的文本不符合要求！");
        //        return;
        //    }

        //    ResultText = txt.Text;
        //    DialogResult = true;
        //    Close();
        //}
        
        public string ResultText { get; private set; }
        public int ResultIndex { get; private set; }

        int defaultButtonIndex = -1;
        int applyRegexButtonIndex = -1;

        public void AddButton(string text, bool defaultButton = false, bool applyRegex = false)
        {
            FlatStyle.Button btn = new FlatStyle.Button()
            {
                Content = text,
                Tag = stk.Children.Count,
                Padding = new Thickness(14, 4, 14, 4),
                Margin = new Thickness(4, 2, 4, 2),
            };
            btn.Click += BtnClickEventHandler;
            if (defaultButton)
            {
                defaultButtonIndex = stk.Children.Count;
            }
            if (applyRegex)
            {
                applyRegexButtonIndex = stk.Children.Count;
            }
            stk.Children.Add(btn);
   
        }

        private void BtnClickEventHandler(object sender, RoutedEventArgs e)
        {

            if(applyRegexButtonIndex!=-1)
            {
                if(stk.Children[applyRegexButtonIndex] ==sender)
                {
                    if (!Regex.IsMatch(txt.Text, regex))
                    {
                        MessageBox.ShowError("输入的文本不符合要求！",this);
                        return;
                    }
                }
            }
            ResultIndex = (int)(sender as Button).Tag;
            ResultText = txt.Text;
            Close();
        }

        private void UpdateColor(SolidColorBrush color)
        {
            Resources["backgroundBrushColor"] = color;
            Resources["darker1BrushColor"] = new SolidColorBrush(Color.FromScRgb(color.Color.ScA, color.Color.ScR * 0.9f, color.Color.ScG * 0.9f, color.Color.ScB * 0.9f));
            Resources["darker2BrushColor"] = new SolidColorBrush(Color.FromScRgb(color.Color.ScA, color.Color.ScR * 0.8f, color.Color.ScG * 0.8f, color.Color.ScB * 0.8f));
            Resources["darker3BrushColor"] = new SolidColorBrush(Color.FromScRgb(color.Color.ScA, color.Color.ScR * 0.7f, color.Color.ScG * 0.7f, color.Color.ScB * 0.7f));
            Resources["darker4BrushColor"] = new SolidColorBrush(Color.FromScRgb(color.Color.ScA, color.Color.ScR * 0.6f, color.Color.ScG * 0.6f, color.Color.ScB * 0.6f));

        }


        private void WindowPreviewMouseLeftButtonDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void WindowPreviewMouseDoubleClickEventHandler(object sender, MouseButtonEventArgs e)
        {
            Topmost = false;
        }

        private void BtnCancelClickEventHandler(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool isKeyDown = false;

        private void TxtPreviewKeyUpEventHandler(object sender, KeyEventArgs e)
        {
            if (isKeyDown && e.Key==Key.Enter)
            {
                isKeyDown = false;
                //BtnOkClickEventHandler(null, null);
                if(defaultButtonIndex!=-1)
                {
                    (stk.Children[defaultButtonIndex]).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
        }

        private void TxtPreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {
            isKeyDown = true;
        }

        public bool AcceptsReturn
        {
            get => txt.AcceptsReturn;
            set => txt.AcceptsReturn = value;
        }

        public TextWrapping TextWrapping
        {
            get => txt.TextWrapping;
            set => txt.TextWrapping = value;
        }
        public bool AllowEmpty { get; set; } = false;

        private void TextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            JudgeButtonsEnable();
            //if (defaultButtonIndex != -1)
            //{
            //    if(!AllowEmpty)
            //    {
            //        (stk.Children[defaultButtonIndex] as Button).IsEnabled = txt.Text != "";
            //    }
            //}
        }

        private void JudgeButtonsEnable()
        {
            int index = 0;
            foreach (Button button in stk.Children)
            {
                bool enable = true;
                if (index == defaultButtonIndex)
                {
                    if (!AllowEmpty && string.IsNullOrEmpty(txt.Text))
                    {
                        enable = false;
                    }
                    if (applyRegexButtonIndex == index)
                    {
                        if (!Regex.IsMatch(txt.Text, regex))
                        {
                            enable = false;
                        }
                    }
                    button.IsEnabled = enable;
                    index++;
                }
            }
        }

        public static WindowOwner DefaultOwner { get; set; } = new WindowOwner();


        public static bool GetInput(string message, out string text, SolidColorBrush color = null, string defaultText = "", string regex = ".*", bool allowEmpty = true, Window owner = null)
        {
            var box = new InputBox(message, owner ?? DefaultOwner.Owner, color, defaultText, regex) { AllowEmpty = allowEmpty };
            box.AddButton("确定", true, true);
            box.AddButton("取消");
            box.ShowDialog();
            text = box.ResultText;
            if (box.ResultIndex == 0)
            {
                return true;
            }
            return false;
        }

        private void DialogLoaded(object sender, RoutedEventArgs e)
        {
            JudgeButtonsEnable();
        }
    }
    
}
