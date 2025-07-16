using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FzLib.Avalonia.Converters;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace FzLib.Avalonia.Test;

public partial class ConverterViewModel : ObservableObject
{
    [ObservableProperty]
    private int alignmentIndex = 0;

    [NotifyPropertyChangedFor(nameof(File))]
    [ObservableProperty]
    private string fileName;

    [ObservableProperty]
    private object nullableValue;

    [ObservableProperty]
    private ObservableCollection<string> stringList = ["a", "b"];

    [ObservableProperty]
    private TimeSpan timeSpan = new TimeSpan(12, 34, 56);
    public FileInfo File => FileName == null ? null : new FileInfo(FileName);
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
