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
        Loaded+=(s,e) =>
        Button_Click(null, null);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        this.GetWindow().ShowOkDialogAsync("title", "messagelongmessagelongmessagelongmessagelongmessagelongmessagelongmessagelongmessagelong");
        //DialogHost.Show(new TextBlock() { Text = "hello" },this.GetWindow().Content as DialogHost);
    }
}
