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
using Avalonia.Controls.Chrome;
using Avalonia.LogicalTree;

namespace FzLib.Avalonia.Controls;

public abstract class ExtendedWindow : Window
{
    public static readonly StyledProperty<bool> CustomTitleBarProperty =
        AvaloniaProperty.Register<ExtendedWindow, bool>(
            nameof(CustomTitleBar));

    public static readonly StyledProperty<CornerRadius> DefaultCornerRadiusProperty =
        AvaloniaProperty.Register<ExtendedWindow, CornerRadius>(nameof(DefaultCornerRadius), new CornerRadius(4));

    /// <summary>
    /// DefaultShadowWidth StyledProperty definition
    /// </summary>
    public static readonly StyledProperty<double> DefaultShadowWidthProperty =
        AvaloniaProperty.Register<ExtendedWindow, double>(nameof(DefaultShadowWidth), 4);

    public new static readonly DirectProperty<ExtendedWindow, Bitmap> IconProperty =
                AvaloniaProperty.RegisterDirect<ExtendedWindow, Bitmap>(
            nameof(Icon),
            o => o.Icon,
            (o, v) => o.Icon = v);

    public static readonly StyledProperty<BoxShadows> ShadowProperty =
        AvaloniaProperty.Register<ExtendedWindow, BoxShadows>(nameof(Shadow), BoxShadows.Parse("0 0 4 0 #88000000"));

    public static readonly DirectProperty<ExtendedWindow, Thickness> ShadowThicknessProperty =
        AvaloniaProperty.RegisterDirect<ExtendedWindow, Thickness>(nameof(ShadowThickness),
            o => o.ShadowThickness);

    public static readonly DirectProperty<ExtendedWindow, double> ShadowWidthProperty =
        AvaloniaProperty.RegisterDirect<ExtendedWindow, double>(nameof(ShadowWidth),
            o => o.ShadowWidth);

    public static readonly StyledProperty<object> TitleBarFooterProperty =
        AvaloniaProperty.Register<ExtendedWindow, object>(
            nameof(TitleBarFooter));

    //private Border bdBorder;
    //private Border bdContainer;
    //private Border bdShadow;
    private Bitmap icon;

    private Thickness shadowThickness = new Thickness(4);

    private double shadowWidth = 4;

    public ExtendedWindow()
    {
    }

    public bool CustomTitleBar
    {
        get => GetValue(CustomTitleBarProperty);
        set => SetValue(CustomTitleBarProperty, value);
    }

    public CornerRadius DefaultCornerRadius
    {
        get => this.GetValue(DefaultCornerRadiusProperty);
        set => SetValue(DefaultCornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the DefaultShadowWidth property. This StyledProperty 
    /// indicates ....
    /// </summary>
    public double DefaultShadowWidth
    {
        get => this.GetValue(DefaultShadowWidthProperty);
        set => SetValue(DefaultShadowWidthProperty, value);
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

    public BoxShadows Shadow
    {
        get => this.GetValue(ShadowProperty);
        set => SetValue(ShadowProperty, value);
    }

    public Thickness ShadowThickness
    {
        get => shadowThickness;
        private set => SetAndRaise(ShadowThicknessProperty, ref shadowThickness, value);
    }

    /// <summary>
    /// Gets or sets the ShadowWidth property. This DirectProperty 
    /// indicates ....
    /// </summary>
    public double ShadowWidth
    {
        get => shadowWidth;
        private set => SetAndRaise(ShadowWidthProperty, ref shadowWidth, value);
    }

    public object TitleBarFooter
    {
        get => GetValue(TitleBarFooterProperty);
        set => SetValue(TitleBarFooterProperty, value);
    }

    protected override Type StyleKeyOverride => UseCustomStyle() ? typeof(ExtendedWindow) : typeof(Window);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        UpdateMargins();
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        IsClosed = true;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == WindowStateProperty)
        {
            Debug.WriteLine($"WindowState:{change.NewValue}");

            UpdateMargins();
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

    protected virtual bool UseCustomChrome()
    {
        //仅在Windows 10使用自定义窗口边框，Windows7和11不使用
        //return OperatingSystem.IsWindowsVersionAtLeast(10)
        //       && !OperatingSystem.IsWindowsVersionAtLeast(10, build: 22000);

        //20260529更新：Avalonia 12.0.4修复了Win10没有边框阴影的问题，所以此处永远返回false
        return false;
    }

    protected virtual bool UseCustomStyle()
    {
        //仅在Windows上使用自定义样式
        return OperatingSystem.IsWindows();
    }
    private void UpdateMargins()
    {
        if (!UseCustomStyle())
        {
            return;
        }

        if (WindowState == WindowState.Maximized || !UseCustomChrome())
        {
            //最大化，不显示阴影
            ShadowWidth = 0;
            ShadowThickness = default;
            CornerRadius = default;
        }
        else
        {
            ShadowWidth = DefaultShadowWidth;
            ShadowThickness = new Thickness(DefaultShadowWidth);
            CornerRadius = DefaultCornerRadius;
        }
    }
}