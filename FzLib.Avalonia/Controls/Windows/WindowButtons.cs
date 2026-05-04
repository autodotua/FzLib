using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.TextFormatting.Unicode;
using Avalonia.Controls.Primitives;

namespace FzLib.Avalonia.Controls;

public partial class WindowButtons : TemplatedControl
{
    public static readonly StyledProperty<double> CornerRadiusWidthProperty =
        AvaloniaProperty.Register<WindowButtons, double>(nameof(CornerRadiusWidth));

    public static readonly DirectProperty<WindowButtons, bool> IsWindowMaximizedProperty =
            AvaloniaProperty.RegisterDirect<WindowButtons, bool>(nameof(IsWindowMaximized),
            o => o.IsWindowMaximized,
            (o, v) => o.IsWindowMaximized = v);

    public static readonly DirectProperty<WindowButtons, bool> IsWindowNormalProperty =
        AvaloniaProperty.RegisterDirect<WindowButtons, bool>(nameof(IsWindowNormal),
            o => o.IsWindowNormal,
            (o, v) => o.IsWindowNormal = v);

    public static readonly DirectProperty<WindowButtons, CornerRadius> RightTopCornerRadiusProperty =
            AvaloniaProperty.RegisterDirect<WindowButtons, CornerRadius>(nameof(RightTopCornerRadius),
            o => o.RightTopCornerRadius,
            (o, v) => o.RightTopCornerRadius = v);

    private Button btnClose;
    private Button btnMinimize;
    private Button btnResize;
    private bool isMaximized = default;

    private bool isWindowNormal = default;
    private CornerRadius rightTopCornerRadius = default;

    public WindowButtons()
    {
    }

    public double CornerRadiusWidth
    {
        get => this.GetValue(CornerRadiusWidthProperty);
        set => SetValue(CornerRadiusWidthProperty, value);
    }

    public bool IsWindowMaximized
    {
        get => isMaximized;
        set => SetAndRaise(IsWindowMaximizedProperty, ref isMaximized, value);
    }

    public bool IsWindowNormal
    {
        get => isWindowNormal;
        set => SetAndRaise(IsWindowNormalProperty, ref isWindowNormal, value);
    }

    public CornerRadius RightTopCornerRadius
    {
        get => rightTopCornerRadius;
        set => SetAndRaise(RightTopCornerRadiusProperty, ref rightTopCornerRadius, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        btnClose = e.NameScope.Find<Button>("PART_CloseButton") ?? throw new Exception("未找到PART_CloseButton");
        btnResize = e.NameScope.Find<Button>("PART_ResizeButton") ?? throw new Exception("未找到PART_ResizeButton");
        btnMinimize = e.NameScope.Find<Button>("PART_MinimizeButton") ?? throw new Exception("未找到PART_MinimizeButton");

        btnClose.Click += CloseButton_Click;
        btnResize.Click += ResizeButton_Click;
        btnMinimize.Click += MinimizeButton_Click;

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


    private void UpdateIsMaximized(Window win)
    {
        IsWindowNormal = !(IsWindowMaximized = win.WindowState == WindowState.Maximized);
    }
}