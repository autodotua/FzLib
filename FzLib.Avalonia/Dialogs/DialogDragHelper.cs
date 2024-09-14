using Avalonia;
using Avalonia.Controls;
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
            if (container is Window window)
            {
                new WindowDragHelper(window).EnableDrag();
            }
            else if (container is Control control)
            {
                new ControlDragHelper(control).EnableDrag();
            }
            else
            {
                throw new ArgumentException("不支持的类型");
            }
        }
    }

    internal class ControlDragHelper
    {
        private Point? startPoint = default;

        public ControlDragHelper(Control control)
        {
            Control = control;
        }

        public void EnableDrag()
        {
            Control.PointerPressed += Container_PointerPressed;
            ;
            Control.PointerMoved += Container_PointerMoved;
            Control.PointerReleased += Container_PointerReleased;

            Control.RenderTransform = new TranslateTransform(0, 0);
        }

        public InputElement Control { get; }

        private void Container_PointerMoved(object sender, PointerEventArgs e)
        {
            if (!startPoint.HasValue)
            {
                return;
            }

            var parent = Control.Parent as Visual ?? throw new Exception("找不到控件的父级");
            var point = e.GetPosition(parent);
            var move = point - startPoint.Value;
            double x = move.X;
            double y = move.Y;

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
            if (x + Control.Bounds.Right > parent.Bounds.Width)
            {
                x = parent.Bounds.Width - Control.Bounds.Right;
            }

            //限制下边界
            if (y + Control.Bounds.Bottom > parent.Bounds.Height)
            {
                y = parent.Bounds.Height - Control.Bounds.Bottom;
            }

            var translate = Control.RenderTransform as TranslateTransform;

            translate.X = x;
            translate.Y = y;
        }

        private void Container_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (sender == e.Source || e.Source is Panel)
            {
                var point = e.GetPosition(Control.Parent as Visual);
                var translate = Control.RenderTransform as TranslateTransform;
                startPoint = new Point(point.X - translate.X, point.Y - translate.Y);
            }
        }

        private void Container_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            startPoint = null;
        }
    }

    internal class WindowDragHelper
    {
        public WindowDragHelper(Window window)
        {
            Window = window;
        }

        public void EnableDrag()
        {
            Window.PointerPressed += Container_PointerPressed;
        }

        public Window Window { get; }

        private void Container_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (sender == e.Source || e.Source is Panel)
            {
                Window.BeginMoveDrag(e);
            }
        }
    }
}