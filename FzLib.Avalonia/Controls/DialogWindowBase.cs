using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FzLib.Avalonia.Controls
{
    public class DialogWindowBase : WindowBase
    {
        public DialogWindowBase(Window owner)
        {
            Owner = owner;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CanResize = false;

            ShowInTaskbar = false;
            KeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
            Owner.Closed += (s, e) => Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            (Owner as Window)?.Activate();
        }
    }
}