using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Linq;
using System;
using FzLib.Avalonia.Test;
using FzLib.Avalonia.Dialogs;
using System.Threading.Tasks;
using FzLib.Avalonia.Controls;

namespace FzLib.Avalonia.Test.Views;

public partial class LoadingPanel : UserControl
{
    public LoadingPanel()
    {
        InitializeComponent();
    }
    private async void LoadingButton_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as Button).Tag as string)
        {
            case "1":
                var cts = LoadingOverlay.ShowLoading(this);
                await Task.Delay(1000);
                cts.Cancel();
                break;
            case "2":
                var cts2 = LoadingOverlay.ShowLoading(this, TimeSpan.FromSeconds(1));
                await Task.Delay(2000);
                cts2.Cancel();
                break;
            case "3":
                var cts3 = LoadingOverlay.ShowLoading(this, TimeSpan.FromSeconds(2));
                await Task.Delay(100);
                cts3.Cancel();
                break;
        }

    }
}