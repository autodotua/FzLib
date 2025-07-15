using CommunityToolkit.Mvvm.ComponentModel;
using FzLib.Avalonia.Dialogs;

namespace FzLib.Avalonia.Test;
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string message;

    [ObservableProperty]
    private DialogContainerType containerType;
}
