using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace FzLib.Avalonia.Controls;

public class VisualDragHelper
{
    private Point? startPoint = default;

    public VisualDragHelper(InputElement thumb, Visual control, Visual controlParent)
    {
        Thumb = thumb ?? throw new ArgumentNullException(nameof(thumb));
        Control = control ?? throw new ArgumentNullException(nameof(control));
        ControlParent = controlParent ?? throw new ArgumentNullException(nameof(controlParent));
    }

    public Visual Control { get; }

    public Visual ControlParent { get; }

    public InputElement Thumb { get; }

    public void EnableDrag()
    {
        Thumb.PointerPressed += Container_PointerPressed;
        Thumb.PointerMoved += Container_PointerMoved;
        Thumb.PointerReleased += Container_PointerReleased;

        Control.RenderTransform = new TranslateTransform(0, 0);
    }
    private void Container_PointerMoved(object sender, PointerEventArgs e)
    {
        if (!startPoint.HasValue)
        {
            return;
        }

        var point = e.GetPosition(ControlParent);
        Debug.WriteLine(point);
        var move = point - startPoint.Value;
        double x = move.X;
        double y = move.Y;
        Debug.WriteLine("{0},{1}", x, y);

        //限制左边界
        if (x + Control.Bounds.Left < 0)
        {
            x = -Control.Bounds.Left;
        }

        //限制上边界
        if (y + Control.Bounds.Top < 0)
        {
            y = -Control.Bounds.Top;
        }

        //限制右边界
        if (x + Control.Bounds.Right > ControlParent.Bounds.Width)
        {
            x = ControlParent.Bounds.Width - Control.Bounds.Right;
        }

        //限制下边界
        if (y + Control.Bounds.Bottom > ControlParent.Bounds.Height)
        {
            y = ControlParent.Bounds.Height - Control.Bounds.Bottom;
        }

        var translate = Control.RenderTransform as TranslateTransform;

        Debug.WriteLine("{0},{1}", x, y);
        translate.X = x;
        translate.Y = y;
    }

    private void Container_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (e.Source == sender)
        {
            var point = e.GetPosition(ControlParent);
            var translate = Control.RenderTransform as TranslateTransform;
            startPoint = new Point(point.X - translate.X, point.Y - translate.Y);
        }
    }

    private void Container_PointerReleased(object sender, PointerReleasedEventArgs e)
    {
        startPoint = null;
    }
}