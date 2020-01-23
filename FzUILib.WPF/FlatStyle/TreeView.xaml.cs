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

namespace FzLib.UI.FlatStyle
{
    /// <summary>
    /// TreeView.xaml 的交互逻辑
    /// </summary>
    public partial class TreeView : System.Windows.Controls.TreeView
    {
        public TreeView()
        {
            InitializeComponent();
        }


        public event KeyEventHandler ItemPreviewKeyDown;
        public event KeyEventHandler ItemPreviewDeleteKeyDown;

        public event MouseButtonEventHandler ItemPreviewMouseLeftButtonDoubleClick;
        public event MouseButtonEventHandler ItemPreviewMouseRightButtonDoubleClick;
        public event MouseButtonEventHandler ItemPreviewMouseDoubleClick;

        public event MouseButtonEventHandler ItemPreviewMouseUp;
        public event MouseButtonEventHandler ItemPreviewMouseLeftButtonUp;
        public event MouseButtonEventHandler ItemPreviewMouseRightButtonUp;
        public event MouseButtonEventHandler ItemPreviewMouseDown;
        public event MouseButtonEventHandler ItemPreviewMouseLeftButtonDown;
        public event MouseButtonEventHandler ItemPreviewMouseRightButtonDown;

        private void LvwItemPreviewMouseDoubleClickEventHandler(object sender, MouseButtonEventArgs e)
        {
            ItemPreviewMouseDoubleClick?.Invoke(sender, e);
            if (e.ChangedButton == MouseButton.Left)
            {
                ItemPreviewMouseLeftButtonDoubleClick?.Invoke(sender, e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                ItemPreviewMouseRightButtonDoubleClick?.Invoke(sender, e);
            }
        }

        private void ListViewItem_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ItemPreviewMouseUp?.Invoke(sender, e);
            if (e.ChangedButton == MouseButton.Left)
            {
                ItemPreviewMouseLeftButtonUp?.Invoke(sender, e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                ItemPreviewMouseRightButtonUp?.Invoke(sender, e);
            }
        }

        private void ListViewItemMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            ItemPreviewMouseDown?.Invoke(sender, e);
            if (e.ChangedButton == MouseButton.Left)
            {
                ItemPreviewMouseLeftButtonDown?.Invoke(sender, e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                ItemPreviewMouseRightButtonDown?.Invoke(sender, e);
            }
        }

        private void LvwItemPreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {
            ItemPreviewKeyDown?.Invoke(sender, e);
            if (e.Key == Key.Delete)
            {
                ItemPreviewDeleteKeyDown?.Invoke(sender, e);
            }
        }


    }
}
