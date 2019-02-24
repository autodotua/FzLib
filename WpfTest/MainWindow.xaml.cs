using FzLib.Control.Dialog;
using FzLib.Control.FlatStyle;
using FzLib.Program.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace WpfTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        TrayIcon icon;
        public MainWindow()
        {
            InitializeComponent();
            WindowHeader.CreatTitle(this);
             icon = new TrayIcon(WpfTest.Properties.Resources.icon, "ads");
            icon.Show();
            DpiChanged += (p1, p2) =>
              {
                  System.Windows.MessageBox.Show(p2.NewDpi.ToString());
              };
            icon.ReShowWhenDisplayChanged = true;
       
        }
        public class VisualBase : Visual
        {
            protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
            {
                base.OnDpiChanged(oldDpi, newDpi);
            }
        }
        public new event DpiChangedEventHandler DpiChanged;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SnakeBar bar = new SnakeBar(this);
            bar.ShowMessage("你好");
            icon.Hide();
            icon.Show();
        }
    }
}
