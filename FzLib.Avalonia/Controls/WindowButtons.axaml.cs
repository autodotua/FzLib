using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Controls;

public partial class WindowButtons : StackPanel
{
    public static readonly StyledProperty<bool> IsMaximizedProperty =
     AvaloniaProperty.Register<WindowButtons, bool>(nameof(IsMaximized));

    public bool IsMaximized
    {
        get => GetValue(IsMaximizedProperty);
        set => SetValue(IsMaximizedProperty, value);
    }
    public WindowButtons()
    {
        InitializeComponent();
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
            win.WindowState= win.WindowState==WindowState.Maximized?WindowState.Normal:WindowState.Maximized;
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
            win.PropertyChanged += Win_PropertyChanged;
        }
        else
        {
            throw new NotSupportedException("TopLevel必须是Window");
        }
    }

    private async void Win_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {      
        //最大化时，若不采取措施，按钮会被裁剪掉一部分，这一部分的宽度刚好是Window.OffScreenMargin
        if (e.Property.Name == nameof(Window.WindowState))
        {
            //当使用拖动TitleBar实现最大化和普通化切换的时候，OffScreenMargin。
            //因此加入一个延迟，再下一次渲染时进行判断。
            await Task.Delay(1);
            Window win = sender as Window;
            win.OffScreenMargin.Deconstruct(out _, out double top, out double right, out _);
            if (win.WindowState == WindowState.Maximized && !IsMaximized)
            {
                IsMaximized = true;
                Margin = new Thickness(0, top, right, 0);
            }
            else if (win.WindowState == WindowState.Normal && IsMaximized)
            {
                IsMaximized = false;
                Margin = new Thickness(0);

            }
        }
    }
}