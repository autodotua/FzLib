using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using FzLib.Avalonia.Controls;
using System;
using System.Linq;

namespace FzLib.Avalonia.Controls;

public partial class StackFormItemGroup : StackPanel
{
    /// <summary>
    /// 当LabelWidth为NaN时，自动调整标签宽度时右侧的空隙宽度。设置值为负数或NaN表示禁用自动调整。
    /// </summary>
    public static readonly StyledProperty<double> AutoUnifyLabelWidthsMarginRightProperty =
        AvaloniaProperty.Register<StackFormItemGroup, double>(nameof(AutoUnifyLabelWidthsMarginRight), 8);

    /// <summary>
    /// 统一的标签宽度。若为NaN，表示自动调整。
    /// </summary>
    public static readonly StyledProperty<double> LabelWidthProperty =
        FormItem.LabelWidthProperty.AddOwner<StackFormItemGroup>();

    private double oldOpacity = 1;

    private bool hasAdjustWidth = false;

    public StackFormItemGroup()
    {
        Spacing = 8;
        SetValue(OrientationProperty, Orientation.Vertical);
        InitializeComponent();
        oldOpacity = Opacity;
        Opacity = 0;
    }

    /// <summary>
    /// 当LabelWidth为NaN时，自动调整标签宽度时右侧的空隙宽度。设置值为负数或NaN表示禁用自动调整。
    /// </summary>
    public double AutoUnifyLabelWidthsMarginRight
    {
        get => GetValue(AutoUnifyLabelWidthsMarginRightProperty);
        set => SetValue(AutoUnifyLabelWidthsMarginRightProperty, value);
    }

    /// <summary>
    /// 统一的标签宽度。若为NaN，表示自动调整。
    /// </summary>
    public double LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }

    public new Orientation Orientation
    {
        get => base.Orientation;
        set => throw new Exception("不支持修改Orientation");
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (IsVisible)
        {
            AdjustLabelWidth();
        }
        //在构造函数的地方隐藏了，这里调整好了再显示，不然画面会闪过一两帧错位的表单……
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == IsVisibleProperty)
        {
            if (true.Equals(change.NewValue))
            {
                AdjustLabelWidth();
            }
        }
    }

    private void AdjustLabelWidth()
    {
        try
        {
            if (hasAdjustWidth || !double.IsNaN(LabelWidth) || !(AutoUnifyLabelWidthsMarginRight > 0))
            {
                return;
            }

            hasAdjustWidth = true;
            double maxWidth = 0;
            foreach (var child in Children.OfType<FormItem>())
            {
                var label = child.GetVisualDescendants().FirstOrDefault(p => p.Name == "PART_LabelText");
                if (label == null)
                {
                    return;
                }

                maxWidth = Math.Max(label.Bounds.Width, maxWidth);
            }

            if (maxWidth > 0)
            {
                LabelWidth = maxWidth + AutoUnifyLabelWidthsMarginRight;
            }
        }
        finally
        {
            Opacity = oldOpacity;
        }
    }
}