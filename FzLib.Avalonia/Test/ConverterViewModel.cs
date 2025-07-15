using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FzLib.Avalonia.Converters;
using System.Collections.ObjectModel;

namespace FzLib.Avalonia.Test;

public partial class ConverterViewModel : ObservableObject
{
    public ObservableCollection<string> List { get; } = ["项1", "项2"];

    [RelayCommand]
    private void ListAdd()
    {
        List.Add($"项{List.Count + 1}");
    }

    [RelayCommand]
    private void ListRemove()
    {
        if (List.Count > 0)
        {
            List.RemoveAt(List.Count - 1);
        }
    }
}
