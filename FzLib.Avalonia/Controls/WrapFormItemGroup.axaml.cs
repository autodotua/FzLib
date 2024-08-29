using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using FzLib.Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FzLib.Avalonia.Controls;

public partial class WrapFormItemGroup : WrapPanel
{
    public static readonly StyledProperty<double> HorizontalSpacingProperty =
        AvaloniaProperty.Register<WrapFormItemGroup, double>(nameof(HorizontalSpacing), 16);

    public static readonly StyledProperty<Thickness> ItemMarginProperty =
            AvaloniaProperty.Register<WrapFormItemGroup, Thickness>(
            nameof(ItemMargin), new Thickness(0, 0, 16, 8));

    public static readonly StyledProperty<double> LabelWidthProperty =
        FormItem.LabelWidthProperty.AddOwner<WrapFormItemGroup>();

    public static readonly StyledProperty<double> VerticalSpacingProperty =
        AvaloniaProperty.Register<WrapFormItemGroup, double>(nameof(VerticalSpacing), 8);

    public WrapFormItemGroup()
    {
        InitializeComponent();
    }

    public double HorizontalSpacing
    {
        get => GetValue(HorizontalSpacingProperty);
        set => SetValue(HorizontalSpacingProperty, value);
    }

    public Thickness ItemMargin
    {
        get => GetValue(ItemMarginProperty);
        set => SetValue(ItemMarginProperty, value);
    }

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

    public double VerticalSpacing
    {
        get => GetValue(VerticalSpacingProperty);
        set => SetValue(VerticalSpacingProperty, value);
    }
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        CalculatePositions();
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        CalculatePositions();
    }

    private void CalculatePositions()
    {
        double panelWidth = Bounds.Width;
        int currentRow = 0;
        int currentColumn = 0;
        double x = 0; //当前行最右侧的坐标

        List<List<Control>> controlsPerRow = new List<List<Control>>()
        {
            new List<Control>()
        };
        foreach (Control child in Children)
        {
            double controlWidth = child.Bounds.Width;

            if (controlWidth + x + HorizontalSpacing > panelWidth)
            {
                // 如果当前位置超过了面板宽度，则移到下一行
                currentRow++;
                currentColumn = 0;
                x = 0;
                controlsPerRow.Add(new List<Control>());
            }

            x += controlWidth + HorizontalSpacing; //右侧坐标增加
            controlsPerRow[^1].Add(child);
            currentColumn++;
        }

        //根据统计情况，为所有控件设置右边距，为最后一行以外的控件设置下边距
        for (int i = 0; i < controlsPerRow.Count; i++)
        {
            foreach (var control in controlsPerRow[i])
            {
                control.Margin = new Thickness(0, 0, HorizontalSpacing, i == controlsPerRow.Count - 1 ? 0 : VerticalSpacing);
            }
        }
    }
}