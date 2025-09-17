using Avalonia.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.VisualTree;

namespace FzLib.Avalonia.Controls;

public abstract class ExtendedWindow : Window
{
    public static readonly StyledProperty<bool> CustomTitleBarProperty =
        AvaloniaProperty.Register<ExtendedWindow, bool>(
            nameof(CustomTitleBar));

    public new static readonly DirectProperty<ExtendedWindow, Bitmap> IconProperty =
            AvaloniaProperty.RegisterDirect<ExtendedWindow, Bitmap>(
            nameof(Icon),
            o => o.Icon,
            (o, v) => o.Icon = v);

    public static readonly StyledProperty<IBrush> TitleBarBackgroundProperty =
        AvaloniaProperty.Register<ExtendedWindow, IBrush>(
            nameof(TitleBarBackground), Brushes.Transparent);

    public static readonly StyledProperty<object> TitleBarFooterProperty =
            AvaloniaProperty.Register<ExtendedWindow, object>(
            nameof(TitleBarFooter));

    private Bitmap icon;

    protected ExtendedWindow()
    {
        CornerRadius = new CornerRadius(2);
    }

    public bool CustomTitleBar
    {
        get => GetValue(CustomTitleBarProperty);
        set => SetValue(CustomTitleBarProperty, value);
    }

    public new Bitmap Icon
    {
        get => icon;
        set
        {
            SetAndRaise(IconProperty, ref icon, value);
            base.Icon = value == null ? null : new WindowIcon(value);
        }
    }

    public bool IsClosed { get; private set; }

    public IBrush TitleBarBackground
    {
        get => GetValue(TitleBarBackgroundProperty);
        set => SetValue(TitleBarBackgroundProperty, value);
    }

    public object TitleBarFooter
    {
        get => GetValue(TitleBarFooterProperty);
        set => SetValue(TitleBarFooterProperty, value);
    }

    protected double ShadowWidth { get; set; } = 8;

    protected override Type StyleKeyOverride
    {
        get
        {
            if (UseCustomStyle())
            {
                return typeof(ExtendedWindow);
            }
            else
            {
                return typeof(Window);
            }
        }
    }

    public void BringToFront()
    {
        if (!IsVisible)
        {
            Show();
        }

        if (WindowState == WindowState.Minimized)
        {
            WindowState = WindowState.Normal;
        }

        Activate();
        Topmost = true; // important
        Topmost = false; // important
        Focus();
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        IsClosed = true;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        UpdateMargins();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var titleBar = e.NameScope.Find<Grid>("PART_TitleBar");
        if (titleBar == null)
        {
            return;
        }
        
        
        titleBar.DoubleTapped += (s, e) =>
        {
            if (e.Source == s)//避免按住标题栏上的按钮时误拖动
            {
                WindowState = WindowState switch
                {
                    WindowState.Normal => WindowState.Maximized,
                    _ => WindowState.Normal,
                };
            }
        };
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == WindowStateProperty)
        {
            Debug.WriteLine($"WindowState:{change.NewValue}");
        }

        if (change.Property == OffScreenMarginProperty)
        {
            Debug.WriteLine($"OffScreenMarginProperty:{OffScreenMargin}");
            UpdateMargins();
        }

        if (change.Property == WindowDecorationMarginProperty)
        {
            Debug.WriteLine($"WindowDecorationMarginProperty:{WindowDecorationMargin}");
        }
    }

    private void UpdateMargins()
    {
        if (!UseCustomStyle())
        {
            return;
        }

        if (WindowState == WindowState.Maximized)
        {
            Resources["ExtendedWindowShadowRadius"] = 0d;
            Resources["ExtendedWindowShadowThickness"] = OffScreenMargin;
            Resources["ExtendedWindowCornerRadius"] = new CornerRadius(0);
        }
        else
        {
            //如果不是Windows10 22000，或者不是Windows，则不显示阴影
            if (OperatingSystem.IsWindowsVersionAtLeast(10, build: 22000)
                || !OperatingSystem.IsWindowsVersionAtLeast(10)
           )
            {
                Resources["ExtendedWindowShadowRadius"] = 0d;
                Resources["ExtendedWindowShadowThickness"] = new Thickness(0);
                Resources["ExtendedWindowCornerRadius"] = new CornerRadius(0);
            }
            else
            {
                Resources["ExtendedWindowShadowRadius"] = ShadowWidth;
                Resources["ExtendedWindowShadowThickness"] = new Thickness(ShadowWidth);
                Resources["ExtendedWindowCornerRadius"] = CornerRadius;
            }
        }
    }

    protected virtual bool UseCustomStyle()
    {
        return OperatingSystem.IsWindows();
    }
}