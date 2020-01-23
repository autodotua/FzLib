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
    /// UneditableComboBox.xaml 的交互逻辑
    /// </summary>
    public partial class UneditableComboBox : ComboBox
    {
        TextBlock tbk;
        public UneditableComboBox()
        {
            InitializeComponent();
                UpdateColor(new SolidColorBrush(Colors.White));
            tbk = GetTemplateChild("tbkHeader") as TextBlock;

        }

        public new bool IsEditable
        {
            get => false;
        }
        
        public void Load(IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
        public void Load(IEnumerable<object> items)
        {
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
        public void Load(IEnumerable<int> items)
        {
            foreach (var item in items)
            {
                Items.Add(item.ToString());
            }
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
    }
}
