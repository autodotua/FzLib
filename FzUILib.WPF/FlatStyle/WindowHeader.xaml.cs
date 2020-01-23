using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;

namespace FzLib.UI.FlatStyle
{
    /// <summary>
    /// Header.xaml 的交互逻辑
    /// </summary>
    public partial class WindowHeader : UserControl, INotifyPropertyChanged
    {
        public static readonly double marginTop = 32;
        public static WindowHeader CreatTitle(Window window, bool autoEvents = true)
        {
            WindowHeader header = new WindowHeader();
            header.ParentWindow = window;

            Panel content = window.Content as Panel;
            content.Children.Insert(0, header);
            content.Margin = new Thickness(0, marginTop, 0, 0);
            header.VerticalAlignment = VerticalAlignment.Top;
            header.Margin = new Thickness(0, -marginTop, 0, 0);
            header.HeaderText = window.Title;
            if (content is Grid)
            {
                Grid.SetColumnSpan(header, int.MaxValue);
            }

            header.autoEvents = autoEvents;
            return header;
        }


        private bool autoEvents;
        private Window parentWindow;

        public Window ParentWindow
        {
            get => parentWindow;
            set
            {
                parentWindow = value;
                //WindowChrome chrome = new WindowChrome();
                //chrome.CaptionHeight = 0;
                //WindowChrome.SetWindowChrome(value, chrome);
                value.WindowStyle = WindowStyle.None;
                value.AllowsTransparency = true;
            }
        }
        private WindowHeader()
        {
            InitializeComponent();

            Background = Brushes.White;


        }
        public new SolidColorBrush Background
        {
            get => Resources["back"] as SolidColorBrush;
            set => UpdateColor(value);
        }
        private void UpdateColor(SolidColorBrush value)
        {
            Resources["back"] = value;
            DarkerBrushConverter.GetDarkerColor(value, out SolidColorBrush darker1, out SolidColorBrush darker2, out SolidColorBrush darker3, out SolidColorBrush darker4);
            Resources["darker1"] = darker1;
            Resources["darker2"] = darker2;

        }


        #region 标题栏
        /// <summary>
        /// 单击菜单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMenuClick(object sender, RoutedEventArgs e)
        {
            MenuButtonClick?.Invoke(sender, e);
        }
        public event RoutedEventHandler MenuButtonClick;
        public bool IsMenuButtonVisible
        {
            get => Resources["menuVisible"].Equals(Visibility.Visible);
            set
            {
                Resources["menuVisible"] = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        double mouseDownY = 1000;
        /// <summary>
        /// 鼠标左键在标题栏上按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderPreviewMouseLeftButtonDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (ParentWindow.WindowState == WindowState.Maximized)
            {
                mouseDownY = e.MouseDevice.GetPosition(sender as Button).Y;
            }
            else
            {
                ParentWindow.DragMove();
            }
        }
        /// <summary>
        /// 双击标题栏事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderMouseDoubleClickEventHandler(object sender, MouseButtonEventArgs e)
        {
            ParentWindow.WindowState = (ParentWindow.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
            e.Handled = true;
        }

        /// <summary>
        /// 单击关闭按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCloseClickEventHandler(object sender, RoutedEventArgs e)
        {
            if (autoEvents)
            {
                parentWindow.Close();
            }
            CloseButtonClick?.Invoke(sender, e);
        }

        public event RoutedEventHandler CloseButtonClick;
        /// <summary>
        /// 单击最大化按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMaxmizeClickEventHandler(object sender, RoutedEventArgs e)
        {
            if (autoEvents)
            {
                if (ParentWindow.WindowState == WindowState.Maximized)
                {
                    ParentWindow.WindowState = WindowState.Normal;
                }
                else
                {
                    ParentWindow.WindowState = WindowState.Maximized;

                }
            }
        }


        /// <summary>
        /// 单击最小化按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMinimizeClickEventHandler(object sender, RoutedEventArgs e)
        {
            if (autoEvents)
            {

                ParentWindow.WindowState = WindowState.Minimized;
            }
            MinimizeButtonClick?.Invoke(sender, e);
        }
        public event RoutedEventHandler MinimizeButtonClick;

        /// <summary>
        /// 鼠标是否在专辑图上按下了
        /// </summary>
        private string headerText = "标题";
        private ImageSource albumImageSource;

        public event PropertyChangedEventHandler PropertyChanged;


        public string HeaderText
        {
            get => headerText;
            set
            {
                headerText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HeaderText"));
            }
        }
        public double HeaderTextMaxWidth { get; set; }

        public ImageSource AlbumImageSource
        {
            get => albumImageSource;
            set
            {
                albumImageSource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AlbumImageSource"));
            }
        }
        #endregion




        double dpi;
        bool isMoving = false;
        Point rawPoint;
        double rawTop;
        double rawLeft;
        private void Button_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
            {
                return;
            }
            Point mousePosition = FzLib.Device.Mouse.Position;
            if (isMoving && btn.IsMouseOver)
            {
                ParentWindow.Top = rawTop + (mousePosition.Y - rawPoint.Y) / dpi;
                ParentWindow.Left = rawLeft + (mousePosition.X - rawPoint.X) / dpi;
                // Debug.WriteLine(ParentWindow.Top + "        " + mousePosition.Y + "       " + rawPoint.Y + "         " + rawTop + "       " + PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice.M11);
            }


            if (ParentWindow.WindowState == WindowState.Maximized && btn.IsMouseCaptured && e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                if (mousePosition.Y - mouseDownY > 8)
                {
                    ParentWindow.Top = 0;
                    ParentWindow.WindowState = WindowState.Normal;
                    rawPoint = FzLib.Device.Mouse.Position;
                    rawTop = ParentWindow.Top;
                    rawLeft = ParentWindow.Left;
                    dpi = VisualTreeHelper.GetDpi(ParentWindow).DpiScaleX;
                    isMoving = true;
                }
            }
        }

        private void Button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            isMoving = false;
        }
    }
}
