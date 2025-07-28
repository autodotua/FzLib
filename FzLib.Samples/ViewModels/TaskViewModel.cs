using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FzLib.Avalonia.Dialogs;

namespace FzLib.Samples.ViewModels;

public partial class TaskViewModel(IDialogService dialogService) : ObservableObject
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

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task DoSthAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(10 * 1000, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            await dialogService.ShowWarningDialogAsync("任务被取消", "任务被取消");
        }
    }
}