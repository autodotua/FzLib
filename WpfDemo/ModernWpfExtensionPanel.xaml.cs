using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static FzLib.INotifyPropertyChangedExtension;
using System.Numerics;
using System.Linq;
using ModernWpf.FzExtension.CommonDialog;
using System;
using System.Threading;
using ModernWpf.FzExtension;
using System.Threading.Tasks;

namespace FzLib.WpfDemo
{
    public class ModernWpfExtensionPanelViewModel : INotifyPropertyChanged
    {
        public ModernWpfExtensionPanelViewModel()
        {
            DialogButtonCommand = new ModernWpfExtensionDialogPanelButtonCommand(this);
            OverlayButtonCommand = new ModernWpfExtensionOverlayPanelButtonCommand(this);
        }

        private string selectR;

        public string SelectR
        {
            get => selectR;
            set => this.SetValueAndNotify(ref selectR, value, nameof(SelectR));
        }

        private string checkR;

        public string CheckR
        {
            get => checkR;
            set => this.SetValueAndNotify(ref checkR, value, nameof(CheckR));
        }

        private string inputR;

        public string InputR
        {
            get => inputR;
            set => this.SetValueAndNotify(ref inputR, value, nameof(InputR));
        }

        private ProgressRingOverlay ringOverlay;

        public ProgressRingOverlay RingOverlay
        {
            get => ringOverlay;
            set => this.SetValueAndNotify(ref ringOverlay, value, nameof(RingOverlay));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ModernWpfExtensionDialogPanelButtonCommand DialogButtonCommand { get; }
        public ModernWpfExtensionOverlayPanelButtonCommand OverlayButtonCommand { get; }
    }

    public class ModernWpfExtensionOverlayPanelButtonCommand : PanelButtonCommandBase
    {
        public ModernWpfExtensionOverlayPanelButtonCommand(ModernWpfExtensionPanelViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public ModernWpfExtensionPanelViewModel ViewModel { get; }

        public override void Execute(object parameter)
        {
            Do(async () =>
            {
                switch (parameter as string)
                {
                    case "3s":
                        ViewModel.RingOverlay.Show();
                        await Task.Delay(3000);
                        ViewModel.RingOverlay.Hide();
                        break;

                    default:
                        break;
                }
            });
        }
    }

    public class ModernWpfExtensionDialogPanelButtonCommand : PanelButtonCommandBase
    {
        public ModernWpfExtensionDialogPanelButtonCommand(ModernWpfExtensionPanelViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public ModernWpfExtensionPanelViewModel ViewModel { get; }

        public async override void Execute(object parameter)
        {
            switch (parameter as string)
            {
                case "message":
                    await CommonDialog.ShowOkDialogAsync("这是标题", "这是消息内容");
                    break;

                case "detail":
                    await CommonDialog.ShowOkDialogAsync("这是标题", "这是消息内容", string.Concat(Enumerable.Repeat("这里可以有很长的详情", 10)));
                    break;

                case "yesno":
                    await CommonDialog.ShowYesNoDialogAsync("这是标题", "这是消息内容", string.Concat(Enumerable.Repeat("这里可以有很长的详情", 10)));
                    break;

                case "error":
                    await CommonDialog.ShowErrorDialogAsync("简单的错误信息");
                    break;

                case "errorD":
                    await CommonDialog.ShowErrorDialogAsync("详细的错误信息错误信息", "错误描述", "错误详情");
                    break;

                case "exception":
                    try
                    {
                        var temp = 1 / new List<int>().Count;
                    }
                    catch (Exception ex)
                    {
                        await CommonDialog.ShowErrorDialogAsync(new Exception("抛出一个错误", ex), "错误描述");
                    }
                    break;

                case "select":
                    {
                        int i = await CommonDialog.ShowSelectItemDialogAsync("选择一个项目", new SelectDialogItem[]
                             {
                        new SelectDialogItem("第一项"),
                        new SelectDialogItem("第二项"),
                        new SelectDialogItem("第三项","第三项有描述"),
                        new SelectDialogItem("第四项","第四项有描述"),
                             });
                        ViewModel.SelectR = $"选择了第{i}项";
                    }
                    break;

                case "selectA":
                    await CommonDialog.ShowSelectItemDialogAsync("选择一个项目", new SelectDialogItem[]
                         {
                        new SelectDialogItem("第一项",null,()=> ViewModel.SelectR = $"选择了第一项"),
                        new SelectDialogItem("第二项",null,()=> ViewModel.SelectR = $"选择了第二项"),
                        new SelectDialogItem("第三项","第三项有描述",()=> ViewModel.SelectR = $"选择了第三项"),
                        new SelectDialogItem("第四项","第四项有描述",()=> ViewModel.SelectR = $"选择了第四项"),
                         });
                    break;

                case "selectE":
                    await CommonDialog.ShowSelectItemDialogAsync("选择一个项目", new SelectDialogItem[]
                         {
                        new SelectDialogItem("第一项",null,()=> ViewModel.SelectR = $"选择了第一项"),
                        new SelectDialogItem("第二项",null,()=> ViewModel.SelectR = $"选择了第二项"),
                         }, "另一个按钮", () => ViewModel.SelectR = "点击了另一个按钮");
                    break;

                case "check":
                    {
                        var result = await CommonDialog.ShowCheckBoxDialogAsync("选择项目", new CheckDialogItem[]
                             {
                        new CheckDialogItem("第一项"),
                        new CheckDialogItem("第二项","不可操作且默认选中",false,true),
                        new CheckDialogItem("第三项","默认选中",true,true),
                        new CheckDialogItem("第四项","第四项有描述"),
                             }, false);
                        ViewModel.CheckR = result == null ? "没有选择" : $"选择了 " + string.Join('、', result.Select(p => p.Title));
                    }
                    break;

                case "checkO":
                    {
                        var result = await CommonDialog.ShowCheckBoxDialogAsync("选择项目", new CheckDialogItem[]
                             {
                        new CheckDialogItem("第一项"),
                        new CheckDialogItem("第二项"),
                             }, true);
                        ViewModel.CheckR = result == null ? "没有选择" : $"选择了 " + string.Join('、', result.Select(p => p.Title));
                    }
                    break;

                case "input":
                    ViewModel.InputR = await CommonDialog.ShowInputDialogAsync("请输入文本", "默认内容") ?? "取消输入";
                    break;

                case "inputI":
                    ViewModel.InputR = (await CommonDialog.ShowIntInputDialogAsync("请输入整数", 123))?.ToString() ?? "取消输入";
                    break;

                case "inputD":
                    ViewModel.InputR = (await CommonDialog.ShowDoubleInputDialogAsync("请输入小数", 123.456))?.ToString() ?? "取消输入";
                    break;
            }
        }
    }

    /// <summary>
    /// BasicPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ModernWpfExtensionPanel : UserControl
    {
        public ModernWpfExtensionPanelViewModel ViewModel { get; } = new ModernWpfExtensionPanelViewModel();

        public ModernWpfExtensionPanel()
        {
            DataContext = ViewModel;
            InitializeComponent();
            ViewModel.RingOverlay = loading;
        }
    }
}