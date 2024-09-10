using Avalonia.Controls;
using Avalonia.Interactivity;
using FzLib.Avalonia.Dialogs;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Test;
public partial class MainWindow : Window
{
    private MainViewModel VM { get; }
    public MainWindow()
    {
        DataContext = VM = new MainViewModel();
        InitializeComponent();
    }

    private void RefreshFormPanelButton_Click(object sender, RoutedEventArgs e)
    {
        formPanelContainer.Content = new FormPanel();
    }
}
