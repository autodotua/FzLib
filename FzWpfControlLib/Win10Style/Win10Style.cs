using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Windows.UI.ViewManagement;

namespace FzLib.Control.Win10Style
{
    #region Windows

    public class ModernWindow : MahApps.Metro.Controls.MetroWindow
    {
        public ModernWindow()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls");
            //CommonResources.InitializeControlResouces(Resources, "VS/Window");
            WindowTitleBrush = new SolidColorBrush(Common.GetSystemThemeColor());
            Binding binding = new Binding() { Path = new PropertyPath("WindowTitleBrush"), RelativeSource = new RelativeSource(RelativeSourceMode.Self) };
            SetBinding(GlowBrushProperty, binding);
            WindowButtonCommands = new WindowButtonCommands();
            OverrideDefaultWindowCommandsBrush = Brushes.White;
            BorderThickness = new Thickness(0);
            NonActiveWindowTitleBrush = Brushes.White;


            UISettings ui = new UISettings();
            ui.ColorValuesChanged += (p1, p2) =>
            {
                if (WindowTitleBrushBindingSystemThemeColor)
                {
                    var color = ui.GetColorValue(UIColorType.Accent);
                    Dispatcher.Invoke(() => WindowTitleBrush = new SolidColorBrush(new Color() { A = color.A, R = color.R, G = color.G, B = color.B }));

                }
            };

        }


        public bool WindowTitleBrushBindingSystemThemeColor { get; set; } = true;

        public bool Win10StyleCommandsButton
        {
            get => WindowButtonCommands is WindowButtonCommands;
            set
            {
                if (value)
                {
                    WindowButtonCommands = new WindowButtonCommands() { ParentWindow = this };
                }
                else
                {
                    WindowButtonCommands = new MahApps.Metro.Controls.WindowButtonCommands();
                }
            }
        }
    }

    public class WindowCommands : MahApps.Metro.Controls.WindowCommands
    {
        //public WindowCommands()
        //{
        //    CommonResources.InitializeControlResoucesInThemes(Resources, "WindowCommands");
        //    Style = FindResource("MahApps.Metro.Styles.WindowCommands.Win10") as Style;
        //}
    }
    public class WindowButtonCommands : MahApps.Metro.Controls.WindowButtonCommands
    {
        public WindowButtonCommands()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls");
            CommonResources.InitializeControlResoucesInThemes(Resources, "WindowButtonCommands");
            Style = FindResource("MahApps.Metro.Styles.WindowButtonCommands.Win10") as Style;
        }
    }

    #endregion

    #region Buttons

    public class FlatButton : Button
    {
        public FlatButton()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls.Buttons");
            Style = FindResource("MetroFlatButton") as Style;

        }
    }
    public class CircleButton : Button
    {
        public CircleButton()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls.Buttons");
            Style = FindResource("MahApps.Metro.Styles.MetroCircleButtonStyle") as Style;
        }
    }
    public class DropDownButton : MahApps.Metro.Controls.DropDownButton
    {
        public DropDownButton()
        {
            // CommonResources.InitializeControlResouces(Resources, "Controls.Buttons");
            // Style = FindResource("MahApps.Metro.Styles.MetroCircleButtonStyle") as Style;
        }
    }
    public class SplitButton : MahApps.Metro.Controls.SplitButton
    {
        public SplitButton()
        {
            // CommonResources.InitializeControlResouces(Resources, "Controls.Buttons");
            // Style = FindResource("MahApps.Metro.Styles.MetroCircleButtonStyle") as Style;
        }
    }
    public class ModernSwitch : ToggleSwitch
    {

        //public ModernSwitch()
        //{
        //    CommonResources.InitializeControlResouces(Resources, "Controls.ToggleSwitch");
        //    Style = FindResource("MetroToggleSwitch") as Style;
        //}
    }
    public class Switch : ToggleSwitch
    {
        public Switch()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls");
            Style = FindResource("MahApps.Metro.Styles.ToggleSwitch.Win10") as Style;
        }
    }
    #endregion

    #region Picker

    public class TimePicker : MahApps.Metro.Controls.TimePicker
    {
        public TimePicker()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls");
            TextBoxHelper.SetWatermark(this, "选择时间");

        }
    }
    public class DatePicker : System.Windows.Controls.DatePicker
    {
        public DatePicker()
        {

            CommonResources.InitializeControlResouces(Resources, "Controls");
            Style = FindResource("MetroDatePicker") as Style;
            TextBoxHelper.SetWatermark(this, "选择日期");
        }
    }
    public class DateTimePicker : MahApps.Metro.Controls.DateTimePicker
    {
        public DateTimePicker()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls");
            MahApps.Metro.Controls.TextBoxHelper.SetWatermark(this, "选择日期和时间");
        }
    }

    public class HotKeyBox : MahApps.Metro.Controls.HotKeyBox
    {
        public HotKeyBox()
        {
        }
    }

    #endregion


    #region Menu
    public class ContextMenu : System.Windows.Controls.ContextMenu
    {
        public ContextMenu()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls.ContextMenu");
            Style = FindResource("MetroContextMenu") as Style;
        }
    }
    public class MenuItem : System.Windows.Controls.MenuItem
    {
        public MenuItem()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls.ContextMenu");
            Style = FindResource("MetroMenuItem") as Style;
        }

        public MenuItem(object header)
        {
            CommonResources.InitializeControlResouces(Resources, "Controls.ContextMenu");
            Style = FindResource("MetroMenuItem") as Style;
            Header = header;
        }
    }

    #endregion


    #region Slider
    public class RangeSlider : MahApps.Metro.Controls.RangeSlider
    {
        public RangeSlider()
        {
            CommonResources.InitializeControlResoucesInThemes(Resources, "RangeSlider");
            Style = FindResource("MahApps.Metro.Styles.RangeSlider.Win10") as Style;
        }
    }


    public class ModernSlider : Slider
    {
        public ModernSlider()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls.Slider");
            Style = FindResource("MahApps.Metro.Styles.Slider") as Style;
        }
    }
    public class Slider : System.Windows.Controls.Slider
    {
        public Slider()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls.Slider");
            Style = FindResource("MahApps.Metro.Styles.Slider.Win10") as Style;
        }
    }
    public class FlatSlider : Slider
    {
        public FlatSlider()
        {
            CommonResources.InitializeControlResouces(Resources, "FlatSlider");
            Style = FindResource("FlatSlider") as Style;
        }
    }

    #endregion

    #region Tab
    public class ModernAnimatedSingleRowTabControl : MetroAnimatedSingleRowTabControl
    {

        public ModernAnimatedSingleRowTabControl()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls.AnimatedTabControl");
            //Style = FindResource("MetroTabControl") as Style;
            //var resource = new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.AnimatedSingleRowTabControl.xaml") };
            //Style = resource.Cast<object>().FirstOrDefault(p => p is Style) as Style;
        }

    }

    public class ModernExpander : Expander
    {
        public ModernExpander()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls.Expander");
            Style = FindResource("MetroExpander") as Style;

        }
    }

    public class ModernTabItem : TabItem
    {
        public ModernTabItem()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls.TabControl");
            Style = FindResource("MetroTabItem") as Style;

        }

    }
    #endregion

    #region ComboBox
    //public class ModernComboBox: ComboBox
    //{
    //    public ModernComboBox()
    //    {
    //        CommonResources.InitializeControlResouces(Resources, "Controls");
    //        Style = FindResource("MetroComboBox") as Style;

    //    }
    //}
    //public class ModernComboBoxItem : ComboBoxItem
    //{
    //    public ModernComboBoxItem()
    //    {
    //        CommonResources.InitializeControlResouces(Resources, "Controls");
    //        Style = FindResource("MetroComboBoxItem") as Style;

    //    }
    //}

    #endregion


    #region List
    public class AzureDataGrid : System.Windows.Controls.DataGrid
    {
        public AzureDataGrid()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls.DataGrid");
            Style = FindResource("AzureDataGrid") as Style;

        }
    }

    public class ModernListView : System.Windows.Controls.ListView
    {
        public ModernListView()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls");
            Style = FindResource("MetroListView") as Style;

        }
    }
    public class ModernListBox : System.Windows.Controls.ListBox
    {
        public ModernListBox()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls.ListBox");
            Style = FindResource("MetroListBox") as Style;

        }
    }

    public class ModernComboBox : System.Windows.Controls.ComboBox
    {
        public ModernComboBox()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls");
            Style = FindResource("VirtualisedMetroComboBox") as Style;

        }
    }

    //public class ModernListBox : ListBox
    //{
    //    public ModernListBox()
    //    {
    //        CommonResources.InitializeControlResouces(Resources, "Controls");
    //        Style = FindResource("MetroListBox") as Style;

    //    }
    //}
    //public class ModernListBoxItem : ListBoxItem
    //{
    //    public ModernListBoxItem()
    //    {
    //        CommonResources.InitializeControlResouces(Resources, "Controls");
    //        Style = FindResource("MetroListBoxItem") as Style;
    //        ItemHelper.SetSelectedForegroundBrush(this, Brushes.White);
    //    }
    //}

    #endregion

    public class ModernTextBox : TextBox
    {
        public ModernTextBox()
        {
            CommonResources.InitializeControlResouces(Resources, "Controls");
            Style = FindResource("MetroTextBox") as Style;

        }
    }

}
