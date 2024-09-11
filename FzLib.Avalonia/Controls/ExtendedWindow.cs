using Avalonia.Controls;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.VisualTree;

namespace FzLib.Avalonia.Controls;

public abstract class ExtendedWindow : Window
{
    protected override Type StyleKeyOverride => typeof(ExtendedWindow);

    protected ExtendedWindow()
    {
        CornerRadius = new CornerRadius(2);
    }

    public bool IsClosed { get; private set; }

    private Bitmap bitmapIcon;

    public static readonly DirectProperty<ExtendedWindow, Bitmap> BitmapIconProperty =
        AvaloniaProperty.RegisterDirect<ExtendedWindow, Bitmap>(
            nameof(BitmapIcon), o => o.BitmapIcon, (o, v) => o.BitmapIcon = v);

    public Bitmap BitmapIcon
    {
        get => bitmapIcon;
        set => SetAndRaise(BitmapIconProperty, ref bitmapIcon, value);
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        IsClosed = true;
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

    protected override void OnInitialized()
    {
        base.OnInitialized();
        SetShadows(WindowState);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var titleBar = this.GetVisualDescendants().FirstOrDefault(p => p.Name == "PART_TitleBar") as Grid;
        if (titleBar != null)
        {
            titleBar.PointerPressed += (s, e) => this.BeginMoveDrag(e);
            titleBar.DoubleTapped += (s, e) =>
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
                else if (WindowState == WindowState.Normal)
                {
                    WindowState = WindowState.Maximized;
                }
            };
        }
    }

    public double ShadowWidth { get; set; } = 8;

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == WindowStateProperty)
        {
            SetShadows((WindowState)change.NewValue);
        }
    }

    private void SetShadows(WindowState state)
    {
        if (state == WindowState.Maximized)
        {
            Resources["ExtendedWindowShadowRadius"] = 0d;
            Resources["ExtendedWindowShadowThickness"] = new Thickness(ShadowWidth);
            Resources["ExtendedWindowCornerRadius"] = new CornerRadius(0);
            Resources["ExtendedWindowInverseShadowThickness"] =
                new Thickness(-ShadowWidth, -ShadowWidth, -ShadowWidth, 0);
        }
        else
        {
            Resources["ExtendedWindowShadowRadius"] = ShadowWidth;
            Resources["ExtendedWindowShadowThickness"] = new Thickness(ShadowWidth);
            Resources["ExtendedWindowCornerRadius"] = CornerRadius;
            Resources["ExtendedWindowInverseShadowThickness"] = new Thickness();
        }
    }
}