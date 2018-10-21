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
using FzLib.Wpf.Control;

namespace FzLib.Wpf.Control.FlatStyle
{
    /// <summary>
    /// Button.xaml 的交互逻辑
    /// </summary>
    public partial class Button : System.Windows.Controls.Button
    {
        public Button()
        {
            InitializeComponent();
        }

        public int DisableTimeAfterClicking { get; set; }

        protected async override void OnClick()
        {
            base.OnClick();
            if (DisableTimeAfterClicking > 0)
            {
                IsEnabled = false;
                await Task.Delay(DisableTimeAfterClicking);
                IsEnabled = true;
            }
        }

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);

            set => SetValue(CornerRadiusProperty, value);
        }

        private static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius",
            typeof(double),
            typeof(Button),
            new PropertyMetadata(1d));

    }
}
