using Avalonia.Controls;
using Avalonia.Interactivity;
using FzLib.Avalonia.Dialogs;
using FzLib.Avalonia;

namespace FzLib.Avalonia.Test.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        this.GetWindow().ShowOkDialogAsync("title", "message");
        //DialogHost.Show(new TextBlock() { Text = "hello" },this.GetWindow().Content as DialogHost);
    }
}
