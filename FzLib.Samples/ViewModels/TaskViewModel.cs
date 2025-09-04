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
    private bool canCancel;

    [ObservableProperty]
    private TimeSpan delay1;

    [ObservableProperty]
    private TimeSpan delay2;

    [ObservableProperty]
    private bool isActive1;

    [ObservableProperty]
    private bool isActive2;

    [ObservableProperty]
    private string message2;

    [ObservableProperty]
    private string title2;

    [RelayCommand]
    private async Task CancelAsync()
    {
        Message2 = "正在取消";
        await Task.Delay(1000);
        IsActive2 = false;
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

    private void ResetLoading2()
    {
        CanCancel = false;
        Message2 = "请稍等";
        Delay2 = TimeSpan.Zero;
    }

    [RelayCommand]
    private async Task ShowLoading1Async()
    {
        Delay1 = TimeSpan.Zero;
        IsActive1 = true;
        await Task.Delay(1000);
        IsActive1 = false;
    }

    [RelayCommand]
    private async Task ShowLoading2Async()
    {
        Delay1 = TimeSpan.FromSeconds(1);
        IsActive1 = true;
        await Task.Delay(2000);
        IsActive1 = false;
    }

    [RelayCommand]
    private async Task ShowLoading3Async()
    {
        Delay1 = TimeSpan.FromSeconds(2);
        IsActive1 = true;
        await Task.Delay(1000);
        IsActive1 = false;
    }

    [RelayCommand]
    private async Task ShowLoading4Async()
    {
        ResetLoading2();
        Title2 = "显示1秒";
        IsActive2 = true;
        await Task.Delay(1000);
        IsActive2 = false;
    }

    [RelayCommand]
    private async Task ShowLoading5Async()
    {
        ResetLoading2();
        Title2 = "延迟显示1秒";
        Delay2 = TimeSpan.FromSeconds(1);
        IsActive2 = true;
        await Task.Delay(2000);
        IsActive2 = false;
    }

    [RelayCommand]
    private async Task ShowLoading6Async()
    {
        ResetLoading2();
        Title2 = "改变提示信息";
        Message2 = "正在处理（1/3）";
        IsActive2 = true;
        await Task.Delay(1000);
        Message2 = "正在处理（2/3）";
        await Task.Delay(1000);
        Message2 = "正在处理（3/3）";
        await Task.Delay(1000);
        IsActive2 = false;
    }

    [RelayCommand]
    private void ShowLoading7()
    {
        ResetLoading2();
        Title2 = "可取消任务";
        Message2 = "正在处理（不会自动停止）";
        CanCancel = true;
        IsActive2 = true;
    }
}