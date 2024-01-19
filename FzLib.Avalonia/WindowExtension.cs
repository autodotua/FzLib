using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using System.Windows;

namespace FzLib.Avalonia
{
    public static class WindowExtension
    {
        public static void BringToFront(this Window window)
        {
            if (!window.IsVisible)
            {
                window.Show();
            }

            if (window.WindowState == WindowState.Minimized)
            {
                window.WindowState = WindowState.Normal;
            }

            window.Activate();
            window.Topmost = true;  // important
            window.Topmost = false; // important
            window.Focus();
        }

        
        public static Window GetWindow<T>(this Visual visual)
        {
            return visual.GetVisualRoot() as Window;
        }
        public static T GetSpecialWindow<T>(this Visual visual) where T : Window
        {
            return visual.GetVisualRoot() as T;
        }
    }
}