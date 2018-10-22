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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FzLib.Control.FlatStyle
{
    /// <summary>
    /// ListBox.xaml 的交互逻辑
    /// </summary>
    public partial class ListBox : System.Windows.Controls.ListBox
    {
        public ListBox()
        {
            InitializeComponent();
            if (!updatedColor)
            {
                UpdateColor(new SolidColorBrush(Colors.White));
            }
        }


        public new SolidColorBrush Background
        {
            get => Resources["back"] as SolidColorBrush;
            set => UpdateColor(value);
        }
        private bool updatedColor = false;
        private void UpdateColor(SolidColorBrush value)
        {
            Resources["back"] = value;
            DarkerBrushConverter.GetDarkerColor(value, out SolidColorBrush darker1, out SolidColorBrush darker2, out SolidColorBrush darker3, out SolidColorBrush darker4);
            Resources["darker1"] = darker1;
            Resources["darker2"] = darker2;
            Resources["darker3"] = darker3;
            Resources["darker4"] = darker4;

            updatedColor = true;
        }



        public delegate void ListBoxItemMouseRightButtonUpEventDelegate(object sender, MouseButtonEventArgs e);
        public event ListBoxItemMouseRightButtonUpEventDelegate ListBoxItemMouseRightButtonUpEvent;
        private void ListBoxItemMouseRightButtonUpEventHandler(object sender, MouseButtonEventArgs e)
        {
            ListBoxItemMouseRightButtonUpEvent?.Invoke(sender, e);
        }
        public delegate void ListBoxItemPreviewKeyDownEventDelegate(object sender, KeyEventArgs e);
        public event ListBoxItemPreviewKeyDownEventDelegate ListBoxItemPreviewKeyDownEvent;
        private void ListBoxItemPreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {
            ListBoxItemPreviewKeyDownEvent?.Invoke(sender, e);
        }
        public delegate void ListBoxItemPreviewMouseLeftDoubleClickEventDelegate(object sender, MouseButtonEventArgs e);
        public event ListBoxItemPreviewMouseLeftDoubleClickEventDelegate ListBoxItemPreviewMouseLeftDoubleClickEvent;
        public delegate void ListBoxItemPreviewMouseRightDoubleClickEventDelegate(object sender, MouseButtonEventArgs e);
        public event ListBoxItemPreviewMouseRightDoubleClickEventDelegate ListBoxItemPreviewMouseRightDoubleClickEvent;
        private void ListBoxItemPreviewMouseDoubleClickEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ListBoxItemPreviewMouseLeftDoubleClickEvent?.Invoke(sender, e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                ListBoxItemPreviewMouseRightDoubleClickEvent?.Invoke(sender, e);
            }
        }
    }
}
