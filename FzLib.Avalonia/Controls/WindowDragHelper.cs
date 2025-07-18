using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace FzLib.Avalonia.Controls;

public class WindowDragHelper
{
    private Point? startPoint = default;
    public WindowDragHelper(InputElement thumb)
    {
        Thumb = thumb ?? throw new ArgumentNullException(nameof(thumb));
        Window = global::Avalonia.Controls.TopLevel.GetTopLevel(thumb) as Window ?? throw new ArgumentException("TopLevel不是Window");
    }

    public InputElement Thumb { get; }
    public Window Window { get; }

    public void EnableDrag()
    {
        Thumb.PointerPressed += Container_PointerPressed;
        Thumb.PointerMoved += Container_PointerMoved;
        Thumb.PointerReleased += Container_PointerReleased;
    }
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
        if (e.Source == sender)
        {
            if (e.Pointer.Type == PointerType.Mouse)
            {
                Window.BeginMoveDrag(e);
                return;
            }

            //触摸和非Windows系统有问题：https://github.com/AvaloniaUI/Avalonia/issues/8429
            startPoint = e.GetPosition(null);
        }
    }

    private void Container_PointerReleased(object sender, PointerReleasedEventArgs e)
    {
        startPoint = null;
    }
}