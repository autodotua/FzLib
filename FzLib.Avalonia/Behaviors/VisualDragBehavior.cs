using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using System;

namespace FzLib.Avalonia.Behaviors;

/// <summary>
/// 为 Visual 控件提供拖动功能，并限制在其父容器范围内。
/// </summary>
public class VisualDragBehavior : Behavior<InputElement>
{
    private Point? startPoint;
    private TranslateTransform? transform;

    /// <summary>
    /// 要被拖动的控件（可以是任何 Visual）
    /// </summary>
    public static readonly StyledProperty<Visual> TargetProperty =
        AvaloniaProperty.Register<VisualDragBehavior, Visual>(nameof(Target));

    /// <summary>
    /// 被拖动控件的父容器，默认为 Target 的父节点。
    /// </summary>
    public static readonly StyledProperty<Visual> ParentContainerProperty =
        AvaloniaProperty.Register<VisualDragBehavior, Visual>(nameof(ParentContainer));

    public Visual? Target
    {
        get => GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    public Visual? ParentContainer
    {
        get => GetValue(ParentContainerProperty);
        set => SetValue(ParentContainerProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null)
            throw new InvalidOperationException("AssociatedObject is null");

        AssociatedObject.PointerPressed += OnPointerPressed;
        AssociatedObject.PointerMoved += OnPointerMoved;
        AssociatedObject.PointerReleased += OnPointerReleased;

        if (Target == null)
            throw new InvalidOperationException("Target (被拖动的控件) 未设置");

        // 设置初始 Transform
        if (Target.RenderTransform is not TranslateTransform translate)
        {
            transform = new TranslateTransform(0, 0);
            Target.RenderTransform = transform;
        }
        else
        {
            transform = translate;
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (AssociatedObject != null)
        {
            AssociatedObject.PointerPressed -= OnPointerPressed;
            AssociatedObject.PointerMoved -= OnPointerMoved;
            AssociatedObject.PointerReleased -= OnPointerReleased;
        }

        startPoint = null;
        transform = null;
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source != sender || Target == null || transform == null)
            return;

        var container = ParentContainer ?? Target.GetVisualParent();
        if (container == null)
            return;

        var point = e.GetPosition(container);
        startPoint = new Point(point.X - transform.X, point.Y - transform.Y);
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!startPoint.HasValue || Target == null || transform == null)
            return;

        var container = ParentContainer ?? Target.GetVisualParent();
        if (container == null)
            return;

        var current = e.GetPosition(container);
        var move = current - startPoint.Value;

        var bounds = Target.Bounds;
        var parentBounds = container.Bounds;

        double x = move.X;
        double y = move.Y;

        // 左边界限制
        if (x + bounds.Left < 0)
            x = -bounds.Left;

        // 上边界限制
        if (y + bounds.Top < 0)
            y = -bounds.Top;

        // 右边界限制
        if (x + bounds.Right > parentBounds.Width)
            x = parentBounds.Width - bounds.Right;

        // 下边界限制
        if (y + bounds.Bottom > parentBounds.Height)
            y = parentBounds.Height - bounds.Bottom;

        transform.X = x;
        transform.Y = y;
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        startPoint = null;
    }
}
