using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public partial class DialogBox : Window
    {
        

        public DialogBox(DialogType type,Window owner=null)
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
    }

    public enum DialogType
    {
        Information,
        Error,
        Warn
    }





}
