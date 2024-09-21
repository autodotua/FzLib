using Avalonia.Controls;
using Avalonia.Input;
using FzLib.Avalonia.Controls;
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
}