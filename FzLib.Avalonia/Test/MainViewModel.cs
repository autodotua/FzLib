using CommunityToolkit.Mvvm.ComponentModel;

namespace FzLib.Avalonia.Test;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string message;

    [ObservableProperty]
    private bool showWindowDialog;
}
