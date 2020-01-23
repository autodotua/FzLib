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
    /// CheckBox.xaml 的交互逻辑
    /// </summary>
    public partial class CheckBox : System.Windows.Controls.CheckBox
    {
        public CheckBox()
        {
            InitializeComponent();
            if(!updatedColor)
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


            updatedColor = true;
        }
    }
}
