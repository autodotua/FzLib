using FzLib.UI.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FzLib.UI.Dialog
{
    public class DialogWindowBase : ExtendedWindow
    {
        public DialogWindowBase(Window owner)
        {
            Owner = owner ?? Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            if (Owner == null)
            {
                Owner = Application.Current.MainWindow;
            }
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.ToolWindow;
            SizeToContent = SizeToContent.WidthAndHeight;
            ShowInTaskbar = false;
        }
    }
}
