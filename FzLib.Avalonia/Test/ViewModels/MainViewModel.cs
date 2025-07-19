using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FzLib.Avalonia.Dialogs;

namespace FzLib.Avalonia.Test.ViewModels;

public partial class MainViewModel(IDialogService dialogService) : ObservableObject
{
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