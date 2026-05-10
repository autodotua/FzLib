using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using System;
using Avalonia.VisualTree;

namespace FzLib.Avalonia.Behaviors;

public class WindowDragBehavior : Behavior<InputElement>
{
    private InputElement element;
    private Point? startPoint;
    private Window window;
    public bool DoubleTapToMaximize { get; set; } = false;

    protected override void OnAttached()
    {
        base.OnAttached();

        element = AssociatedObject ?? throw new InvalidOperationException("AssociatedObject is null");

        element.AttachedToVisualTree += OnAttachedToVisualTree;
        element.DetachedFromVisualTree += OnDetachedFromVisualTree;
        if (element.IsAttachedToVisualTree())
        {
            OnAttachedToVisualTree(null, null);
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (element != null)
        {
            element.AttachedToVisualTree -= OnAttachedToVisualTree;
            element.DetachedFromVisualTree -= OnDetachedFromVisualTree;
            DetachPointerEvents();
        }

        window = null;
        element = null;
    }

    private void AttachPointerEvents()
    {
        if (element == null)
        {
            return;
        }

        element.PointerPressed += OnPointerPressed;
        element.PointerMoved += OnPointerMoved;
        element.PointerReleased += OnPointerReleased;
        element.DoubleTapped += OnDoubleTapped;
    }

    private void DetachPointerEvents()
    {
        if (element == null)
        {
            return;
        }

        element.PointerPressed -= OnPointerPressed;
        element.PointerMoved -= OnPointerMoved;
        element.PointerReleased -= OnPointerReleased;
        element.DoubleTapped -= OnDoubleTapped;
    }

    private void OnAttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
    {
        if (element == null)
        {
            return;
        }

        window = TopLevel.GetTopLevel(element) as Window;
        if (window != null)
        {
            AttachPointerEvents();
        }
    }

    private void OnDetachedFromVisualTree(object sender, VisualTreeAttachmentEventArgs e)
    {
        DetachPointerEvents();
        window = null;
    }
    private void OnDoubleTapped(object sender, TappedEventArgs e)
    {
        if (element == null || window == null || e.Source != sender || !DoubleTapToMaximize)
        {
            return;
        }
        if (window.WindowState == WindowState.Maximized)
        {
            window.WindowState = WindowState.Normal;
        }
        else
        {
            window.WindowState = WindowState.Maximized;
        }
    }
    private void OnPointerMoved(object sender, PointerEventArgs e)
    {
        if (!startPoint.HasValue || window == null)
        {
            return;
        }

        var current = e.GetPosition(null);
        var offset = current - startPoint.Value;

        var position = window.Position;
        window.Position = new PixelPoint(position.X + (int)offset.X, position.Y + (int)offset.Y);
    }

    private void OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (element == null || window == null || e.Source != sender)
        {
            return;
        }

        if (e.Pointer.Type == PointerType.Mouse)
        {
            window.BeginMoveDrag(e);
        }
        else
        {
            startPoint = e.GetPosition(null);
        }
    }
    private void OnPointerReleased(object sender, PointerReleasedEventArgs e)
    {
        startPoint = null;
    }
}