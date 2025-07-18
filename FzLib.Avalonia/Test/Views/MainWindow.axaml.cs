using Avalonia.Controls;
using Avalonia.Interactivity;
using FzLib.Avalonia.Dialogs;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Styling;
using FzLib.Avalonia.Controls;

namespace FzLib.Avalonia.Test.Views;
public partial class MainWindow : ExtendedWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void RefreshFormPanelButton_Click(object sender, RoutedEventArgs e)
    {
        formPanelContainer.Content = new FormPanel();
    }

    private void Button_OnClick(object sender, RoutedEventArgs e)
    {
        if (App.Current.RequestedThemeVariant == ThemeVariant.Light)
        {
            App.Current.RequestedThemeVariant=ThemeVariant.Dark;
        }
        else
        {
            App.Current.RequestedThemeVariant=ThemeVariant.Light;
        }
    }
}
