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

    public static readonly StyledProperty<IBrush> TitleBarBackgroundProperty =
                    AvaloniaProperty.Register<ExtendedWindow, IBrush>(
            nameof(TitleBarBackground), Brushes.Transparent);

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
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var titleBar = e.NameScope.Find<Grid>("PART_TitleBar");
        //bdShadow = e.NameScope.Find<Border>("PART_Shadow");
        //bdBorder = e.NameScope.Find<Border>("PART_Border");
        //bdContainer = e.NameScope.Find<Border>("PART_ContainerBorder");

        UpdateMargins();

        if (titleBar == null)
        {
            return;
        }


        titleBar.DoubleTapped += (s, e) =>
        {
            if (e.Source == s) //避免按住标题栏上的按钮时误拖动
            {
                WindowState = WindowState switch
                {
                    WindowState.Normal => WindowState.Maximized,
                    _ => WindowState.Normal,
                };
            }
        };
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
        return OperatingSystem.IsWindowsVersionAtLeast(10)
               && !OperatingSystem.IsWindowsVersionAtLeast(10, build: 22000);
    }

    protected virtual bool UseCustomStyle()
    {
        return OperatingSystem.IsWindows();
    }
    private void UpdateMargins()
    {
        if (!UseCustomStyle())
        {
            return;
        }

        if (WindowState == WindowState.Maximized)
        {
            //最大化，不显示阴影
            ShadowWidth = 0;
            ShadowThickness = default;
            CornerRadius = default;
        }
        else
        {
            if (UseCustomChrome())
            {
                //仅对Windows10显示阴影

                //阴影
                //bdShadow.BoxShadow = BoxShadows.Parse($"0 0 {ShadowWidth} 0 #88000000");
                ////内容向内收缩
                //bdContainer.Margin = bdShadow.Margin = new Thickness(ShadowWidth);
                ////显示圆角
                //bdShadow.CornerRadius = CornerRadius;
                //bdContainer.CornerRadius = CornerRadius;
                ////显示边框
                //bdBorder.IsVisible = true;
                ////边框显示在内容之外
                //BorderThickness.Deconstruct(out var l, out var t, out var r, out var b);
                //bdBorder.Margin = new Thickness(ShadowWidth - l, ShadowWidth - t, ShadowWidth - r, ShadowWidth - b);


                ShadowWidth = DefaultShadowWidth;
                ShadowThickness = new Thickness(DefaultShadowWidth);
                CornerRadius = DefaultCornerRadius;

            }
            //else
            //{
            //    bdShadow.BoxShadow = default;
            //    bdContainer.Margin = bdShadow.Margin = default;
            //    bdShadow.CornerRadius = default;
            //    bdContainer.CornerRadius = default;
            //    bdBorder.IsVisible = false;
            //}
        }
    }
}