using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace FzLib.Avalonia.Controls;

public class RadioButtonGroup : ListBox, ILayoutChangeable
{
    public static readonly StyledProperty<int> ColumnsProperty = AvaloniaProperty.Register<RadioButtonGroup, int>(
        nameof(Columns));

    public static readonly StyledProperty<ItemsControlLayout> LayoutProperty =
        AvaloniaProperty.Register<RadioButtonGroup, ItemsControlLayout>(
            nameof(Layout), ItemsControlLayout.HorizontalStack);

    public static readonly StyledProperty<int> RowsProperty = AvaloniaProperty.Register<RadioButtonGroup, int>(
        nameof(Rows));

    public static readonly StyledProperty<double> SpacingProperty = AvaloniaProperty.Register<RadioButtonGroup, double>(
        nameof(Spacing));

    public RadioButtonGroup()
    {
        SelectionMode = SelectionMode.Single | SelectionMode.AlwaysSelected;
        ItemsControlExtensions.SetLayoutSubscribe(this, this, LayoutProperty, SpacingProperty, ColumnsProperty, RowsProperty);
    }

    public int Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public ItemsControlLayout Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    public int Rows
    {
        get => GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    protected override Type StyleKeyOverride { get; } = typeof(RadioButtonGroup);
}