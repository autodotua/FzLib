using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using FzLib.Avalonia.Dialogs;
using System;
using System.Linq;

namespace FzLib.Avalonia.Test;


public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string message;

    [ObservableProperty]
    private bool showWindowDialog;
}
public partial class MainWindow : Window
{
    private MainViewModel VM { get; }
    public MainWindow()
    {
        DataContext = VM = new MainViewModel();
        InitializeComponent();
    }
    private async void DialogButton_Click(object sender, RoutedEventArgs e)
    {
        VM.Message = "";
        DialogExtension.ContainerType = VM.ShowWindowDialog ? DialogContainerType.Window : DialogContainerType.Popup;

        switch ((sender as Button).Tag as string)
        {
            case "1":
                await this.ShowOkDialogAsync("标题", "信息正文");
                break;
            case "2":
                await this.ShowOkDialogAsync("标题", "信息正文", string.Concat(Enumerable.Repeat("详细内容", 1000)));
                break;
            case "3":
                await this.ShowWarningDialogAsync("标题", "警告正文");
                break;
            case "4":
                await this.ShowErrorDialogAsync("标题", "错误正文");
                break;
            case "5":
                try
                {
                    _ = 1 / Array.Empty<int>().Length;
                }
                catch (Exception ex)
                {
                    while (await this.ShowErrorDialogAsync("错误信息", ex, true))
                    {

                    }
                }
                break;
            case "6":
                VM.Message = (await this.ShowYesNoDialogAsync("标题", "询问内容")).Value ? "单击“是”" : "单击“否”";
                break;
            case "7":
                switch (await this.ShowYesNoDialogAsync("标题", "询问内容", cancelButon: true))
                {
                    case true:
                        VM.Message = "单击“是”";
                        break;
                    case false:
                        VM.Message = "单击“否”";
                        break;
                    case null:
                        VM.Message = "单击“取消”";
                        break;
                }
                break;

            case "8":
                VM.Message = "输入内容：" + await this.ShowInputTextDialogAsync("标题", "请输入：", "默认值", "水印");
                break;

            case "9":
                VM.Message = "输入内容：" + await this.ShowInputTextDialogAsync("标题", "必须长度>5且不能出现数字：", "默认值", "水印", text =>
                {
                    if (text.Length <= 5)
                    {
                        throw new ArgumentException("长度必须>5");
                    }
                    if ("0123456789".Any(p => text.Contains(p)))
                    {
                        throw new ArgumentException("不能出现数字");
                    }
                });
                break;

            case "10":
                VM.Message = "输入内容：" + await this.ShowInputPasswordDialogAsync("标题", "请输入密码：", "水印");
                break;

            case "11":
                VM.Message = "输入内容：" + await this.ShowInputMultiLinesTextDialogAsync("标题", "请输入多行文本：");
                break;

            case "12":
                VM.Message = "输入内容：" + await this.ShowInputNumberDialogAsync<double>("标题", "请输入数字：");
                break;

            case "13":
                VM.Message = "输入内容：" + await this.ShowInputNumberDialogAsync<int>("标题", "请输入整数：");
                break;

            case "14":
                SelectDialogItem[] items = [
                    new SelectDialogItem("第一条", "详情"),
                    new SelectDialogItem("第二条", "详情"),
                    new SelectDialogItem("第三条"),
                    new SelectDialogItem("第四条", "单击直接触发", async () => await this.ShowOkDialogAsync("单击了第四条")),
                ];
                int? index = await this.ShowSelectItemDialog("标题", items, "提示消息", "额外按钮", async () => await this.ShowOkDialogAsync("单击了额外按钮"));
                VM.Message = index.HasValue ? $"单击了{items[index.Value].Title}" : "没有选择";
                break;

            case "15":
                CheckDialogItem[] checkItems = [
                    new CheckDialogItem("第一条", "详情"),
                    new CheckDialogItem("第二条"),
                    new CheckDialogItem("第三条", "禁用", false, false),
                    new CheckDialogItem("第四条", "默认选择", true, true),
                    new CheckDialogItem("第五条", "禁用", false, true),
                    new CheckDialogItem("第六条"),
                ];
                bool result = await this.ShowCheckItemDialog("标题", checkItems, "需要选择2-4个", 2, 4);
                VM.Message = result ? VM.Message = $"选择了{string.Join('，', checkItems.Where(p => p.IsChecked).Select(p => p.Title))}" : "取消了选择";
                break;
        }
    }
}
