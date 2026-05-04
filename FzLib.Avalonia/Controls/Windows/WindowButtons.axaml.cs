using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.TextFormatting.Unicode;

namespace FzLib.Avalonia.Controls;

public partial class WindowButtons : StackPanel
{
    public static readonly StyledProperty<double> CornerRadiusWidthProperty =
        AvaloniaProperty.Register<WindowButtons, double>(nameof(CornerRadiusWidth));

    public static readonly DirectProperty<WindowButtons, bool> IsMaximizedProperty =
            AvaloniaProperty.RegisterDirect<WindowButtons, bool>(nameof(IsMaximized),
            o => o.IsMaximized,
            (o, v) => o.IsMaximized = v);

    public static readonly DirectProperty<WindowButtons, CornerRadius> RightTopCornerRadiusProperty =
        AvaloniaProperty.RegisterDirect<WindowButtons, CornerRadius>(nameof(RightTopCornerRadius),
            o => o.RightTopCornerRadius,
            (o, v) => o.RightTopCornerRadius = v);

    private bool isMaximized = default;

    private CornerRadius rightTopCornerRadius = default;

    public WindowButtons()
    {
        InitializeComponent();
    }

    public double CornerRadiusWidth
    {
        get => this.GetValue(CornerRadiusWidthProperty);
        set => SetValue(CornerRadiusWidthProperty, value);
    }

    public bool IsMaximized
    {
        get => isMaximized;
        set => SetAndRaise(IsMaximizedProperty, ref isMaximized, value);
    }

    public CornerRadius RightTopCornerRadius
    {
        get => rightTopCornerRadius;
        set => SetAndRaise(RightTopCornerRadiusProperty, ref rightTopCornerRadius, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == CornerRadiusWidthProperty)
        {
            var v = (double)change.NewValue;
            RightTopCornerRadius = new CornerRadius(0, v, 0, 0);
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        if (TopLevel.GetTopLevel(this) is Window win)
        {
            win.Close();
        }
        else
        {
            throw new NotSupportedException("TopLevel必须是Window");
        }
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        if (TopLevel.GetTopLevel(this) is Window win)
        {
            win.WindowState = win.WindowState == WindowState.Minimized ? WindowState.Normal : WindowState.Minimized;
        }
        else
        {
            throw new NotSupportedException("TopLevel必须是Window");
        }
    }

    private void ResizeButton_Click(object sender, RoutedEventArgs e)
    {
        if (TopLevel.GetTopLevel(this) is Window win)
        {
            win.WindowState = win.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
        else
        {
            throw new NotSupportedException("TopLevel必须是Window");
        }
    }

    private void StackPanel_Loaded(object sender, RoutedEventArgs e)
    {
        if (TopLevel.GetTopLevel(this) is Window win)
        {
            UpdateIsMaximized(win);

            win.PropertyChanged += (s, e2) =>
            {
                if (e2.Property == Window.WindowStateProperty)
                {
                    UpdateIsMaximized(win);
                }
            };
        }
        else
        {
            throw new NotSupportedException("TopLevel必须是Window");
        }
    }
    private void UpdateIsMaximized(Window win)
    {
        IsMaximized = win.WindowState == WindowState.Maximized;
    }
}