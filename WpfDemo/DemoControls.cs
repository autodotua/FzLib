using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace FzLib.WpfDemo
{
    public class DemoVerticleStackPanel : SimpleStackPanel
    {
        public DemoVerticleStackPanel()
        {
            Spacing = 12;
            Margin = new System.Windows.Thickness(8);
            Orientation = Orientation.Vertical;
        }
    }

    public class DemoHorizontalStackPanel : SimpleStackPanel
    {
        public DemoHorizontalStackPanel()
        {
            Spacing = 8;
            Orientation = Orientation.Horizontal;
        }
    }

    public class DemoWrapPanel : WrapPanel
    {
        public DemoWrapPanel()
        {
            ItemWidth = 128;
        }
    }

    public class DemoTextBox : TextBox
    {
        public DemoTextBox()
        {
            Width = 120;
        }
    }

    public class DemoComboBox : ComboBox
    {
        public DemoComboBox()
        {
            Width = 120;
        }
    }

    public class DemoNumberTextBox : NumberBox
    {
        public DemoNumberTextBox()
        {
            Width = 120;
        }
    }

    public class DemoScrollViewer : ScrollViewer
    {
    }
}