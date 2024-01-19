using Avalonia.Controls;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace FzLib.Avalonia.Controls
{
    public abstract class WindowBase : Window, INotifyPropertyChanged
    {
        public WindowBase()
        {
            DataContext = this;
            WindowCreated?.Invoke(this, EventArgs.Empty);
        }

        public static event EventHandler WindowCreated;

        public bool IsClosed { get; private set; }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsClosed = true;
        }

        public void BringToFront()
        {
            if (!IsVisible)
            {
                Show();
            }

            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }

            Activate();
            Topmost = true;  // important
            Topmost = false; // important
            Focus();
        }
    }
}