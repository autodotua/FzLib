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

namespace FzLib.Wpf.Control.FlatStyle
{
    /// <summary>
    /// Slider.xaml 的交互逻辑
    /// </summary>
    public partial class HorizontalSlider : System.Windows.Controls.Slider
    {
        public HorizontalSlider()
        {
            InitializeComponent();
            Background = new SolidColorBrush(Colors.White);
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
            Resources["darker3"] = darker3;
            Resources["darker4"] = darker4;
        }

        

        public new Orientation Orientation
        {
            set => throw new Exception("不允许设置方向");
        }

        private static readonly DependencyProperty TextProperty =
  DependencyProperty.Register("Text",
      typeof(string),
      typeof(HorizontalSlider),
      new PropertyMetadata(""));

        private string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }


        public static readonly DependencyProperty TextWidthProperty =
    DependencyProperty.Register("TextWidth",
        typeof(double),
        typeof(HorizontalSlider),
        new PropertyMetadata(32.0));

        public double TextWidth
        {
            get => (double)GetValue(TextWidthProperty);
            set
            {
                if (value < 0)
                {
                    throw new Exception("文字宽度小于0");
                }
                SetValue(TextWidthProperty, value);
            }
        }

        public static readonly DependencyProperty TextVisibilityProperty =
   DependencyProperty.Register("TextVisibility",
       typeof(Visibility),
       typeof(HorizontalSlider),
       new PropertyMetadata(Visibility.Visible));

        public Visibility TextVisibility
        {
            get => (Visibility)GetValue(TextVisibilityProperty);
            set
            {
                SetValue(TextVisibilityProperty, value);
            }
        }
        


        public static readonly DependencyProperty TextMarginProperty =
   DependencyProperty.Register("TextMargin",
       typeof(Thickness),
       typeof(HorizontalSlider),
       new PropertyMetadata(new Thickness(4,0,0,0)));

        public Thickness TextMargin
        {
            get => (Thickness)GetValue(TextMarginProperty);
            set
            {
                SetValue(TextMarginProperty, value);
            }
        }
        public static readonly DependencyProperty TextAlignmentProperty =
   DependencyProperty.Register("TextAlignment",
       typeof(TextAlignment),
       typeof(HorizontalSlider),
       new PropertyMetadata(TextAlignment.Right));

        public TextAlignment TextHorizontalAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty);
            set
            {
                SetValue(TextAlignmentProperty, value);
            }
        }

        public Func<double, string> TextConvert { get => textConvert; set => textConvert = value; }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Text = TextConvert(Value);
        }

        private Func<double, string> textConvert = p => p.ToString() ;

        private void LoadedEventHandler(object sender, RoutedEventArgs e)
        {
            slider_ValueChanged(null, null);
        }
    }
}
