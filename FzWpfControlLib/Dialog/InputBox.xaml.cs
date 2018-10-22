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
using static FzLib.Control.Dialog.DialogBox;

namespace FzLib.Control.Dialog
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
            Button btn = new Button()
            {
                Content = text,
                Tag = stk.Children.Count ,
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
                        DialogHelper.ShowError("输入的文本不符合要求！");
                        return;
                    }
                }
            }
            ResultIndex = (int)((sender as Button).Tag);
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
        public bool AllowEmpty { get => allowEmpty; set => allowEmpty = value; }

        private bool allowEmpty=false;

        private void TextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            if(defaultButtonIndex!=-1)
            {
                if(!allowEmpty)
                {
                    (stk.Children[defaultButtonIndex] as Button).IsEnabled = txt.Text != "";
                }
            }
        }
    }
    
}
