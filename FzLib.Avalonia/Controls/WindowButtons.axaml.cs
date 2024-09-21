using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.TextFormatting.Unicode;

namespace FzLib.Avalonia.Controls;

public partial class WindowButtons : StackPanel
{
    public WindowButtons()
    {
        InitializeComponent();
        // RenderOptions.SetEdgeMode(btnMinimize,EdgeMode.Aliased);
        // RenderOptions.SetEdgeMode(btnResize,EdgeMode.Aliased);
        // RenderOptions.SetEdgeMode(btnClose,EdgeMode.Aliased);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        if (TopLevel.GetTopLevel(this) is Window win)
        {
            win.Close();
        }
        else
        {
            throw new NotSupportedException("TopLevel必须是Window");
        }
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        if (TopLevel.GetTopLevel(this) is Window win)
        {
            win.WindowState = win.WindowState == WindowState.Minimized ? WindowState.Normal : WindowState.Minimized;
        }
        else
        {
            throw new NotSupportedException("TopLevel必须是Window");
        }
    }

    private void ResizeButton_Click(object sender, RoutedEventArgs e)
    {
        if (TopLevel.GetTopLevel(this) is Window win)
        {
            win.WindowState = win.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
        else
        {
            throw new NotSupportedException("TopLevel必须是Window");
        }
    }

    private void StackPanel_Loaded(object sender, RoutedEventArgs e)
    {
        if (TopLevel.GetTopLevel(this) is Window win)
        {
            UpdateIsMaximized(win);

            win.PropertyChanged += (s, e2) =>
            {
                if (e2.Property == Window.WindowStateProperty)
                {
                    UpdateIsMaximized(win);
                }
            };
        }
        else
        {
            throw new NotSupportedException("TopLevel必须是Window");
        }
    }

    private void UpdateIsMaximized(Window win)
    {
        Resources["IsNotMaximized"] =
            !(bool)(Resources["IsMaximized"] = win.WindowState == WindowState.Maximized);
    }
}