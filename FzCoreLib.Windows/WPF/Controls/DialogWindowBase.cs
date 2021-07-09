using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FzLib.WPF.Controls
{
    public class DialogWindowBase : WindowBase
    {
        public DialogWindowBase(Window owner)
        {
            Owner = owner;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.ToolWindow;
            ShowInTaskbar = false;
            KeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
            Owner.Closing += (s, e) => Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            (Owner as Window)?.Activate();
        }
    }
}