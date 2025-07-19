using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FzLib.Avalonia.Test.ViewModels;

public partial class LoadingViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isActive;

    [ObservableProperty]
    private TimeSpan delay;

    [RelayCommand]
    private async Task ShowLoading1Async()
    {
        Delay = TimeSpan.Zero;
        IsActive = true;
        await Task.Delay(1000);
        IsActive = false;
    }
    
    [RelayCommand]
    private async Task ShowLoading2Async()
    {
        Delay = TimeSpan.FromSeconds(1);
        IsActive = true;
        await Task.Delay(2000);
        IsActive = false;
    }
    
    [RelayCommand]
    private async Task ShowLoading3Async()
    {
        Delay = TimeSpan.FromSeconds(2);
        IsActive = true;
        await Task.Delay(1000);
        IsActive = false;
    }
}