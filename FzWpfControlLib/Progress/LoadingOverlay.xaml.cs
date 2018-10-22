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

namespace FzLib.Control.Progress
{
    /// <summary>
    /// LoadingOverlay.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingOverlay : UserControl
    {
        public LoadingOverlay()
        {
            InitializeComponent();
        }

        public void Show()
        {
            Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            Visibility = Visibility.Collapsed;
        }

        public Brush OverlayColor
        {
            get => (Brush)GetValue(OverlayColorProperty);
            set=>
                SetValue(OverlayColorProperty, value);
            
        }

        public static readonly DependencyProperty OverlayColorProperty =
            DependencyProperty.Register("OverlayColor", 
                typeof(Brush),
                typeof(LoadingOverlay), 
                new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public double OverlayOpacity
        {
            get => grdOverlay.Opacity;
            set
            {
                if(value<0 || value>1)
                {
                    throw new ArgumentOutOfRangeException("透明度应介于0和1之间");
                }
                grdOverlay.Opacity = value;
            }
        }

        public Brush RingColor
        {
            get => pgr.Foreground;
            set => pgr.Foreground = value;
        }
        
        /// <summary>
        /// 若打开自适应，则表示相对尺寸（0-1）；否则则是绝对尺寸。
        /// </summary>
        public double RingSize
        {
            get
            {
                if (Adaptive)
                {
                    return relativeRingSize;
                }
                else
                {
                  return  pgr.ActualWidth;
                }
            }
            set
            {
                if (Adaptive)
                {
                    if(value>1)
                    {
                        value = 1;
                    }
                    if(value<0)
                    {
                        value = 0;
                    }
                    relativeRingSize = value;
                    UserControl_SizeChanged(null,null);
                }
                else
                {
                    pgr.Width = pgr.Height = value;
                }
            }
        }
        private double relativeRingSize = 1;

        public bool Adaptive
        {
            get => adaptive;
            set
            {
                adaptive = value;
                UserControl_SizeChanged(null, null);
            }
        }

        private bool adaptive = true;

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Adaptive)
            {
                double min = Math.Min(ActualHeight, ActualWidth);
                pgr.Width = pgr.Height = relativeRingSize * min;
            }
        }
        
    }
}
