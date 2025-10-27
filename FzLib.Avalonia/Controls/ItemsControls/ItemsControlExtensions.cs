using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Templates;

namespace FzLib.Avalonia.Controls;

internal static class ItemsControlExtensions
{
    public static void SetLayout<T>(T owner, ItemsControl control)
        where T : Control, ILayoutChangeable
    {
        Func<Panel> func = owner.Layout switch
        {
            ItemsControlLayout.VerticalStack => () => new StackPanel
            {
                Orientation = Orientation.Vertical,
                Spacing = owner.Spacing
            },
            ItemsControlLayout.HorizontalStack => () => new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = owner.Spacing
            },
            ItemsControlLayout.VerticalWrap => () => new WrapPanel
            {
                Orientation = Orientation.Vertical,
                ItemSpacing = owner.Spacing,
                LineSpacing = owner.Spacing
            },
            ItemsControlLayout.HorizontalWrap => () => new WrapPanel
            {
                Orientation = Orientation.Horizontal,
                ItemSpacing = owner.Spacing,
                LineSpacing = owner.Spacing
            },
            ItemsControlLayout.UniformGrid => () => new UniformGrid()
            {
                ColumnSpacing = owner.Spacing,
                RowSpacing = owner.Spacing,
                Columns = owner.Columns,
                Rows = owner.Rows
            },
            _ => throw new ArgumentOutOfRangeException()
        };
        control.ItemsPanel = new FuncTemplate<Panel>(func);
    }

    public static void SetLayoutSubscribe<T>(T owner, ItemsControl control,
        StyledProperty<ItemsControlLayout> layoutProperty,
        StyledProperty<double> spacingProperty,
        StyledProperty<int> columnsProperty,
        StyledProperty<int> rowsProperty)
        where T : Control, ILayoutChangeable
    {
        owner.GetObservable(layoutProperty).Subscribe(l => SetLayout(owner, control));
        owner.GetObservable(spacingProperty).Subscribe(d => SetLayout(owner, control));
        owner.GetObservable(columnsProperty).Subscribe(i =>
        {
            if (owner.Layout == ItemsControlLayout.UniformGrid)
            {
                SetLayout(owner, control);
            }
        });
        owner.GetObservable(rowsProperty).Subscribe(i =>
        {
            if (owner.Layout == ItemsControlLayout.UniformGrid)
            {
                SetLayout(owner, control);
            }
        });
    }
}