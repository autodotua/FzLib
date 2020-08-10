using FzLib.UI.Dialog;
using FzLib.UI.Extension;
using FzLib.UI.FlatStyle;
using FzLib.Program.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Diagnostics;

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
            // icon = new TrayIcon(WpfTest.Properties.Resources.icon, "ads");
            //icon.Show();
            //DpiChanged += (p1, p2) =>
            //  {
            //      System.Windows.MessageBox.Show(p2.NewDpi.ToString());
            //  };
            //icon.ReShowWhenDisplayChanged = true;

        }

        public class A
        {
            public string AA { get; set; } = "A";
            public string BB { get; set; } = "B";
        }
        //public new event DpiChangedEventHandler DpiChanged;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
          //  PanelExport pe = new PanelExport(grd);
          //new ImageBrush(pe.GetBitmap());
            //SnakeBar bar = new SnakeBar(this);
            //bar.ShowMessage("你好");
            //icon.Hide();
            //icon.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileSystemDialog.GetSaveFile(new FileFilterCollection()
                .Add("文本", "txt")
                .Add("图片","png"),ensureExtension:true);
        }
    }
}
