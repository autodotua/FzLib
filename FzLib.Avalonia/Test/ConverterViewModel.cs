using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FzLib.Avalonia.Converters;
using System.Collections.ObjectModel;

namespace FzLib.Avalonia.Test;

public partial class ConverterViewModel : ObservableObject
{
    [ObservableProperty]
    private object nullableValue;

    public ObservableCollection<string> List { get; } = ["项1", "项2"];

    [ObservableProperty]
    private ObservableCollection<string> stringList = ["a", "b"];

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
    [RelayCommand]
    private void SwitchNull(bool value)
    {
        if (value)
        {
            NullableValue = new object();
        }
        else
        {
            NullableValue = null;
        }
    }
}
