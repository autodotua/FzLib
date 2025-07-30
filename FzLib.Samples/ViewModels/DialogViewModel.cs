using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FzLib.Avalonia.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System;
using System.Threading.Tasks;
using FzLib.Samples.Views;
using FzLib.Avalonia.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace FzLib.Samples.ViewModels;

public partial class DialogViewModel(IDialogService dialogService) : ObservableObject
{
    [ObservableProperty]
    private DialogContainerType containerType;

    [ObservableProperty]
    private string message;

    public IDialogService DialogService { get; } = dialogService;

    [RelayCommand]
    private async Task OpenDialog1()
    {
        DialogService.ContainerType = ContainerType;
        await DialogService.ShowOkDialogAsync("普通消息", "信息正文");
    }


    [RelayCommand]
    private async Task OpenDialog2()
    {
        DialogService.ContainerType = ContainerType;
        await DialogService.ShowOkDialogAsync("大量内容", string.Concat(Enumerable.Repeat("信息正文", 100)),
            string.Concat(Enumerable.Repeat("详细内容", 1000)));
    }

    [RelayCommand]
    private async Task OpenDialog3()
    {
        DialogService.ContainerType = ContainerType;
        await DialogService.ShowWarningDialogAsync("警告", "警告正文");
    }

    [RelayCommand]
    private async Task OpenDialog4()
    {
        DialogService.ContainerType = ContainerType;
        await DialogService.ShowErrorDialogAsync("错误", "错误正文");
    }

    [RelayCommand]
    private async Task OpenDialog5()
    {
        try
        {
            _ = 1 / Array.Empty<int>().Length;
        }
        catch (Exception ex)
        {
            while (await DialogService.ShowErrorDialogAsync("错误信息", ex, true))
            {
            }
        }
    }

    [RelayCommand]
    private async Task OpenDialog6()
    {
        DialogService.ContainerType = ContainerType;
        Message = (await DialogService.ShowYesNoDialogAsync("询问", "询问内容")).Value ? "单击“是”" : "单击“否”";
    }

    [RelayCommand]
    private async Task OpenDialog7()
    {
        DialogService.ContainerType = ContainerType;
        switch (await DialogService.ShowYesNoDialogAsync("可取消询问", "询问内容", cancelButon: true))
        {
            case true:
                Message = "单击“是”";
                break;
            case false:
                Message = "单击“否”";
                break;
            case null:
                Message = "单击“取消”";
                break;
        }
    }

    [RelayCommand]
    private async Task OpenDialog8()
    {
        DialogService.ContainerType = ContainerType;
        Message = "输入内容：" + await DialogService.ShowInputTextDialogAsync("一般输入", "请输入：", "默认值", "水印");
    }

    [RelayCommand]
    private async Task OpenDialog9()
    {
        DialogService.ContainerType = ContainerType;
        Message = "输入内容：" + await DialogService.ShowInputTextDialogAsync("验证输入", "必须长度>5且不能出现数字：", "默认值", "水印", text =>
        {
            if (text.Length <= 5)
            {
                return ValidationResult.Error("长度必须>5");
            }

            if ("0123456789".Any(text.Contains))
            {
                return ValidationResult.Error("不能出现数字");
            }

            return ValidationResult.Valid();
        });
    }
    
    
    [RelayCommand]
    private async Task OpenDialog10()
    {
        DialogService.ContainerType = ContainerType;
        Message = "输入内容：" + await DialogService.ShowInputPasswordDialogAsync("密码输入", "请输入密码：", "水印");
    }

    [RelayCommand]
    private async Task OpenDialog11()
    {
        DialogService.ContainerType = ContainerType;
        Message = "输入内容：" + await DialogService.ShowInputMultiLinesTextDialogAsync("多行输入", "请输入多行文本：");
    }

    [RelayCommand]
    private async Task OpenDialog12()
    {
        DialogService.ContainerType = ContainerType;
        Message = "输入内容：" + await DialogService.ShowInputNumberDialogAsync<double>("数字输入", "请输入数字：");
    }

    [RelayCommand]
    private async Task OpenDialog13()
    {
        DialogService.ContainerType = ContainerType;
        Message = "输入内容：" + await DialogService.ShowInputNumberDialogAsync<int>("整数输入", "请输入整数：");
    }

    [RelayCommand]
    private async Task OpenDialog14()
    {
        DialogService.ContainerType = ContainerType;
        SelectDialogItem[] items =
        [
            new SelectDialogItem("第一条", "详情"),
            new SelectDialogItem("第二条", "详情"),
            new SelectDialogItem("第三条"),
            new SelectDialogItem("第四条", "单击直接触发", async () => await DialogService.ShowOkDialogAsync("单击了第四条")),
        ];
        int? index = await DialogService.ShowSelectItemDialog("单条选择", items, "提示消息", "额外按钮",
            async () => await DialogService.ShowOkDialogAsync("单击了额外按钮"));
        Message = index.HasValue ? $"单击了{items[index.Value].Title}" : "没有选择";
    }

    [RelayCommand]
    private async Task OpenDialog15()
    {
        DialogService.ContainerType = ContainerType;
        CheckDialogItem[] checkItems =
        [
            new CheckDialogItem("第一条", "详情"),
            new CheckDialogItem("第二条"),
            new CheckDialogItem("第三条", "禁用", false, false),
            new CheckDialogItem("第四条", "默认选择", true, true),
            new CheckDialogItem("第五条", "禁用", false, true),
            new CheckDialogItem("第六条"),
        ];
        bool result = await DialogService.ShowCheckItemDialog("多条选择", checkItems, "需要选择2-4个", 2, 4);
        Message = result
            ? $"选择了{string.Join('，', checkItems.Where(p => p.IsChecked).Select(p => p.Title))}"
            : "取消了选择";
    }

    [RelayCommand]
    private async Task OpenDialog16()
    {
        DialogService.ContainerType = ContainerType;
        await DialogService.ShowCustomDialogAsync(new ComboBoxDialog());
    }

    [RelayCommand]
    private async Task OpenDialog17()
    {
        DialogService.ContainerType = ContainerType;
        await DialogService.ShowOkDialogAsync("超长正文", string.Concat(Enumerable.Repeat("很长很长的信息正文", 10)));
    }

    [RelayCommand]
    private async Task OpenDialog19()
    {
        WeakReferenceMessenger.Default.Send(new OpenAnotherWindowMessage());
        await Task.Delay(100);
        await DialogService.ShowOkDialogAsync("标题", "来自IDialogService的信息");
    }
    
    
    public class OpenAnotherWindowMessage
    {
    }

    [RelayCommand]
    private async Task OpenDialog20()
    {
        WeakReferenceMessenger.Default.Send(new OpenAnotherWindowMessage());
        await Task.Delay(100);
        await App.Services.GetRequiredKeyedService<IDialogService>("main").ShowOkDialogAsync("标题", "仅在主窗口显示");
    }

}