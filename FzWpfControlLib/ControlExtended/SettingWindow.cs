using FzLib.Control.Dialog;
using FzLib.Control.Text;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Controls = System.Windows.Controls;

namespace FzLib.Control.ControlExtended
{
    public class SettingWindow<T> : Window
    {
        public T SettingInstance { get; private set; }
        public bool UseGroup { get; private set; }
        private StackPanel mainPanel = new StackPanel();

        public SettingWindow(T setting, bool useGroup = false, string title = "设置", Window owner = null)
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = owner;
            Title = title;
            UseGroup = useGroup;
            SizeToContent = SizeToContent.WidthAndHeight;
            SettingInstance = setting;

            InitializeLayout();
            Load();

        }

        private void InitializeLayout()
        {
            Grid mainGrid = new Grid() { Margin = new Thickness(4) };

            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(8) });
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Content = mainGrid;
            StackPanel stk = new StackPanel() {  Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom };

            //Grid.SetRowSpan(stk, int.MaxValue);
            //Grid.SetColumn(stk, int.MaxValue);
            Grid.SetRow(stk, 2);
            mainGrid.Children.Add(stk);

            Grid.SetRow(mainPanel, 2);
            mainGrid.Children.Add(mainPanel);

            FzLib.Control.FlatStyle.Button btnOK = new FzLib.Control.FlatStyle.Button() { Content = "确定", Padding = new Thickness(12, 2, 12, 2), Margin = new Thickness(4) };
            FzLib.Control.FlatStyle.Button btnApply = new FzLib.Control.FlatStyle.Button() { Content = "应用", Padding = new Thickness(12, 2, 12, 2), Margin = new Thickness(4) };
            FzLib.Control.FlatStyle.Button btnCancel = new FzLib.Control.FlatStyle.Button() { Content = "取消", Padding = new Thickness(12, 2, 12, 2), Margin = new Thickness(4) };
            stk.Children.Add(btnCancel);
            stk.Children.Add(btnApply);
            stk.Children.Add(btnOK);
            btnOK.Click += BtnOkClick;
        }

        private Dictionary<Controls.Control, PropertyInfo> controls;
        public Dictionary<string, Grid> groups = new Dictionary<string, Grid>();

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            if (Save())
            {
                Close();
            }
        }

        private bool Save()
        {
            foreach (var keyValue in controls)
            {
                Controls.Control control = keyValue.Key;
                PropertyInfo prop = keyValue.Value;

                if (prop.PropertyType.Name == "String")
                {
                    string value = (control as TextBox).Text;
                    SettingDisableEmptyAttribute attri = prop.GetCustomAttribute<SettingDisableEmptyAttribute>();
                    if (attri != null)
                    {
                        bool disable = attri.IncludeWhiteSpace ? string.IsNullOrWhiteSpace(value) : string.IsNullOrEmpty(value);
                        if (disable)
                        {
                            DialogHelper.ShowError("设置“" + GetName(prop) + "”的值不可为空", this);
                            return false;
                        }
                    }

                    prop.SetValue(SettingInstance, value);
                }
                else if (prop.PropertyType.Name == "Int32")
                {
                    int? value = (control as NumberTextBox).IntNumber;
                    if (!value.HasValue)
                    {
                        DialogHelper.ShowError("设置“" + GetName(prop) + "”的值不是整数", this);
                        return false;
                    }
                    var attriNum = prop.GetCustomAttribute<LimitedNumberSettingAttribute>();
                    if (attriNum != null)
                    {
                        if (value.Value < attriNum.NumberMin)
                        {
                            DialogHelper.ShowError("设置“" + GetName(prop) + "”的值不应小于" + attriNum.NumberMin, this);
                            return false;
                        }
                        if (value.Value > attriNum.NumberMax)
                        {
                            DialogHelper.ShowError("设置“" + GetName(prop) + "”的值不应大于" + attriNum.NumberMax, this);
                            return false;
                        }
                    }
                    prop.SetValue(SettingInstance, value.Value);
                }
                else if (prop.PropertyType.Name == "Double")
                {
                    double? value = (control as NumberTextBox).DoubleNumber;
                    if (!value.HasValue)
                    {
                        DialogHelper.ShowError("设置“" + GetName(prop) + "”的值不是整数", this);
                        return false;
                    }
                    var attriNum = prop.GetCustomAttribute<LimitedNumberSettingAttribute>();
                    if (attriNum != null)
                    {
                        if (value.Value < attriNum.NumberMin)
                        {
                            DialogHelper.ShowError("设置“" + GetName(prop) + "”的值不应小于" + attriNum.NumberMin, this);
                            return false;
                        }
                        if (value.Value > attriNum.NumberMax)
                        {
                            DialogHelper.ShowError("设置“" + GetName(prop) + "”的值不应大于" + attriNum.NumberMax, this);
                            return false;
                        }
                    }
                    prop.SetValue(SettingInstance, value.Value);
                }
                else if (prop.PropertyType.Name == "Boolean")
                {
                    bool value = (control as CheckBox).IsChecked.Value;

                    prop.SetValue(SettingInstance, value);
                }
                else if (prop.PropertyType.Name == "SolidColorBrush")
                {
                    SolidColorBrush value = (control as FzLib.Control.Picker.ColorPickerTextBox).ColorBrush;

                    prop.SetValue(SettingInstance, value);
                }
                else if (prop.PropertyType.Name == "Color")
                {
                    Color value = (control as FzLib.Control.Picker.ColorPickerTextBox).ColorBrush.Color;

                    prop.SetValue(SettingInstance, value);
                }
            }
            return true;
        }

        private Grid GetGrid(string name)
        {
            
            if (!groups.ContainsKey(name))
            {
                groups.Add(name, InitializeGrid(name, mainPanel));
            }
            return groups[name];
        }

        private Grid InitializeGrid(string groupName,Panel parent)
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(8) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            if (groupName=="")
            {
                //grid.Margin = new Thickness(0, 0, 0, 4);
                mainPanel.Children.Insert(0, grid);
            }
            else
            {
                GroupBox box = new GroupBox() { Header = groupName,Margin=new Thickness(0,4,0,4),Padding=new Thickness(6,4,4,0) };
                box.Content = grid;
                mainPanel.Children.Add(box);
            }
            return grid;
        }
        private void Load()
        {
            controls = new Dictionary<Controls.Control, PropertyInfo>();
            Type type = SettingInstance.GetType();


            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Grid currentGrid = null;
            foreach (var prop in properties)
            {
                if (Attribute.IsDefined(prop, typeof(InvisibleSettingAttribute)))
                {
                    continue;
                }

                var group = prop.GetCustomAttribute<SettingGroupAttribute>();
                if (group == null || string.IsNullOrWhiteSpace(group.GroupName))
                {
                    currentGrid = GetGrid("");
                }
                else
                {
                    currentGrid = GetGrid(group.GroupName);
                }

                string name = GetName(prop);
                int rowCount = currentGrid.RowDefinitions.Count;

                currentGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                currentGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(8) });



                object value = prop.GetValue(SettingInstance);
                Controls.Control valueControl = null;
                if (value is string || value is int || value is double || value is SolidColorBrush || value is Color)
                {
                    TextBlock tbk = null;
                    if (value is string)
                    {

                        tbk = new TextBlock() { Text = name };
                        valueControl = new FzLib.Control.FlatStyle.TextBox() { Text = value as string };
                    }
                    else if (value is int)
                    {
                        tbk = new TextBlock() { Text = name };
                        valueControl = new NumberTextBox() { MatchMode = NumberTextBox.Mode.IntegerNumber };
                        (valueControl as TextBox).Text = ((int)value).ToString();
                    }
                    else if (value is double)
                    {
                        tbk = new TextBlock() { Text = name };
                        valueControl = new NumberTextBox() { MatchMode = NumberTextBox.Mode.All };
                        (valueControl as TextBox).Text = ((double)value).ToString();
                    }
                    else if (value is SolidColorBrush)
                    {
                        tbk = new TextBlock() { Text = name };
                        valueControl = new FzLib.Control.Picker.ColorPickerTextBox() { ColorBrush = value as SolidColorBrush };

                    }
                    else if (value is Color)
                    {
                        tbk = new TextBlock() { Text = name };
                        valueControl = new FzLib.Control.Picker.ColorPickerTextBox() { ColorBrush = new SolidColorBrush((Color)value) };

                    }
                    currentGrid.Children.Add(tbk);
                    Grid.SetRow(tbk, rowCount);
                    currentGrid.Children.Add(valueControl);
                    Grid.SetRow(valueControl, rowCount);
                    Grid.SetColumn(valueControl, 2);

                }
                else if (value is bool)
                {
                    valueControl = new FzLib.Control.FlatStyle.CheckBox() { Content = name, IsChecked = (bool)value };

                    currentGrid.Children.Add(valueControl);
                    Grid.SetRow(valueControl, rowCount);
                    Grid.SetColumnSpan(valueControl, 3);

                }
                controls.Add(valueControl, prop);
            }
            if(mainPanel.Children.Count>2 && groups.ContainsKey(""))
            {
                Grid grid = groups[""] as Grid;
                grid.Margin = new Thickness(10, 0, 0, 0);
            }
        }

        private static string GetName(PropertyInfo prop)
        {
            NormalSettingAttribute attri = prop.GetCustomAttribute<NormalSettingAttribute>();
            string name = attri == null ? prop.Name : attri.Description;
            return name;
        }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class SettingGroupAttribute : Attribute
    {
        public SettingGroupAttribute()
        {
        }
        public SettingGroupAttribute(string name)
        {
            GroupName = name;
        }

        public string GroupName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class InvisibleSettingAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LimitedNumberSettingAttribute : NormalSettingAttribute
    {
        public LimitedNumberSettingAttribute(string description, double min, double max) : base(description)
        {
            if (max <= min)
            {
                throw new ArgumentException("最大值小于等于最小值");
            }
            NumberMin = min;
            NumberMax = max;
        }
        public double NumberMax { get; set; }
        public double NumberMin { get; set; }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingDisableEmptyAttribute : Attribute
    {
        public bool IncludeWhiteSpace { get; set; }
        public SettingDisableEmptyAttribute(bool includeWhiteSpace)
        {
            IncludeWhiteSpace = includeWhiteSpace;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class NormalSettingAttribute : Attribute
    {
        public NormalSettingAttribute(string description)
        {
            Description = description;
        }

        public string Description { get; set; }
    }
}
