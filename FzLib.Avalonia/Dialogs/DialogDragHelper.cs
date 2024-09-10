using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FzLib.Avalonia.Dialogs
{
    internal static class DialogDragHelperExtension
    {
        public static void EnableDrag(this InputElement container)
        {
            new DialogDragHelper(container).EnableDrag();
        }
    }
    internal class DialogDragHelper
    {
        private Point? startPoint = default;

        public DialogDragHelper(InputElement container)
        {
            Container = container;
        }

        public void EnableDrag()
        {
            Container.PointerPressed += Container_PointerPressed; ;
            Container.PointerMoved += Container_PointerMoved;
            Container.PointerReleased += Container_PointerReleased;

            Container.RenderTransform = new TranslateTransform(0, 0);
        }

        public InputElement Container { get; }

        private void Container_PointerMoved(object sender, PointerEventArgs e)
        {
            if (!startPoint.HasValue)
            {
                return;
            }
            var parent = Container.Parent as Visual;
            var point = e.GetPosition(parent);
            var move = point - startPoint.Value;
            double x = move.X;
            double y = move.Y;

            //限制左边界
            if (x + Container.Bounds.Left < 0)
            {
                x = -Container.Bounds.Left;
            }

            //限制上边界
            if (y + Container.Bounds.Top < 0)
            {
                y = -Container.Bounds.Top;
            }

            //限制右边界
            if (x +  Container.Bounds.Right > parent.Bounds.Width)
            {
                x = parent.Bounds.Width -Container.Bounds.Right;
            }

            //限制下边界
            if(y+Container.Bounds.Bottom>parent.Bounds.Height)
            {
                y = parent.Bounds.Height - Container.Bounds.Bottom;
            }

            var translate = Container.RenderTransform as TranslateTransform;

            translate.X = x;
            translate.Y = y;
        }

        private void Container_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            var point = e.GetPosition(Container.Parent as Visual);
            var translate = Container.RenderTransform as TranslateTransform;
            startPoint = new Point(point.X - translate.X, point.Y - translate.Y);
        }

        private void Container_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            startPoint = null;
        }
    }
}
