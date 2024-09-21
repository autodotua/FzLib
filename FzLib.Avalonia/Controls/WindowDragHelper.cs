using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace FzLib.Avalonia.Controls;

public class WindowDragHelper
{
    private Point? startPoint = default;
    public Control Thunmb { get; }
    public WindowDragHelper(Control thumb)
    {
        Thunmb = thumb;
        Window = TopLevel.GetTopLevel(thumb) as Window ?? throw new ArgumentException("TopLevel不是Window");
    }

    public void EnableDrag()
    {
        Thunmb.PointerPressed += Container_PointerPressed;
        Thunmb.PointerMoved += Container_PointerMoved;
        Thunmb.PointerReleased += Container_PointerReleased;
    }

    public Window Window { get; }

    private void Container_PointerMoved(object sender, PointerEventArgs e)
    {
        if (!startPoint.HasValue)
        {
            return;
        }

        var point = e.GetPosition(null);
        var move = point - startPoint.Value;
        int x = (int)move.X;
        int y = (int)move.Y;


        var p = Window.Position;
        Window.Position = new PixelPoint(p.X + x, p.Y + y);
    }

    private void Container_PointerPressed(object sender, PointerPressedEventArgs e)
    {

        if (e.Pointer.Type == PointerType.Mouse)
        {
            //触摸和非Windows系统有问题：https://github.com/AvaloniaUI/Avalonia/issues/8429
            Window.BeginMoveDrag(e);
            return;
        }

        if (sender == Thunmb)
        {
            startPoint = e.GetPosition(null);
        }
    }

    private void Container_PointerReleased(object sender, PointerReleasedEventArgs e)
    {
        startPoint = null;
    }
}