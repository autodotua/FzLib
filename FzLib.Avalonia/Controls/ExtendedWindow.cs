using Avalonia.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.VisualTree;

namespace FzLib.Avalonia.Controls;

public abstract class ExtendedWindow : Window
{
    public new static readonly DirectProperty<ExtendedWindow, Bitmap> IconProperty =
        AvaloniaProperty.RegisterDirect<ExtendedWindow, Bitmap>(
            nameof(Icon),
            o => o.Icon,
            (o, v) => o.Icon = v);

    private Bitmap icon;

    protected ExtendedWindow()
    {
        CornerRadius = new CornerRadius(2);
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

    protected double ShadowWidth { get; set; } = 8;

    private bool UseCustomStyle()
    {
        return true;
        //Is Windows 10
        return OperatingSystem.IsWindows()
               && Environment.OSVersion.Version.Major == 10
               && Environment.OSVersion.Version.Build < 22000;
    }

    public static readonly StyledProperty<bool> CustomTitleBarProperty =
        AvaloniaProperty.Register<ExtendedWindow, bool>(
            nameof(CustomTitleBar));

    public bool CustomTitleBar
    {
        get => GetValue(CustomTitleBarProperty);
        set => SetValue(CustomTitleBarProperty, value);
    }

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

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (UseCustomStyle())
        {
            var titleBar = this.GetVisualDescendants().FirstOrDefault(p => p.Name == "PART_TitleBar") as Grid;
            if (titleBar == null)
            {
                return;
            }
            new WindowDragHelper(titleBar).EnableDrag();
                
            titleBar.DoubleTapped += (s, e) =>
            {
                WindowState = WindowState switch
                {
                    WindowState.Normal => WindowState.Maximized,
                    _ => WindowState.Normal,
                };
            };
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
            if (OperatingSystem.IsWindowsVersionAtLeast(10, build: 22000)
                || !OperatingSystem.IsWindows())
            {
                Resources["ExtendedWindowShadowRadius"] = 0;
                Resources["ExtendedWindowShadowThickness"] = new Thickness(0);
                Resources["ExtendedWindowCornerRadius"] = 0;
            }
            else
            {
                Resources["ExtendedWindowShadowRadius"] = ShadowWidth;
                Resources["ExtendedWindowShadowThickness"] = new Thickness(ShadowWidth);
                Resources["ExtendedWindowCornerRadius"] = CornerRadius;
            }
        }
    }
}