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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using Microsoft.WindowsAPICodePack.FzExtension;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;
using TaskDialog = Microsoft.WindowsAPICodePack.Dialogs.TaskDialog;

namespace FzLib.WpfDemo
{
    public class WindowsAPICodePackExtensionPanelViewModel : INotifyPropertyChanged
    {
        public WindowsAPICodePackExtensionPanelViewModel()
        {
            FileButtonCommand = new WindowsAPICodePackExtensionPanelFileButtonCommand(this);
            TaskDialogButtonCommand = new WindowsAPICodePackExtensionPanelTDButtonCommand(this);
        }

        public bool UnionFilter { get; set; }
        public bool AllFilter { get; set; }
        public string DefaultName { get; set; }
        private string fileResult;

        public string FileResult
        {
            get => fileResult;
            set => this.SetValueAndNotify(ref fileResult, value, nameof(FileResult));
        }

        private string taskResult;

        public string TaskResult
        {
            get => taskResult;
            set => this.SetValueAndNotify(ref taskResult, value, nameof(TaskResult));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public WindowsAPICodePackExtensionPanelFileButtonCommand FileButtonCommand { get; }
        public WindowsAPICodePackExtensionPanelTDButtonCommand TaskDialogButtonCommand { get; }
    }

    public class WindowsAPICodePackExtensionPanelTDButtonCommand : PanelButtonCommandBase
    {
        public WindowsAPICodePackExtensionPanelTDButtonCommand(WindowsAPICodePackExtensionPanelViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public WindowsAPICodePackExtensionPanelViewModel ViewModel { get; }

        public override void Execute(object parameter)
        {
            ViewModel.TaskResult = "";

            switch (parameter as string)
            {
                case "msg":
                    new TaskDialog().SetText("Text").SetInstructionText("InstructionText").Show();
                    break;

                case "detail":
                    new TaskDialog().SetText("Text").SetInstructionText("InstructionText").SetDetail("这里是详细信息").Show();
                    break;

                case "yesno":
                    ViewModel.TaskResult = new TaskDialog().SetText("Text").SetInstructionText("InstructionText").ShowYesNo().ToString();
                    break;

                case "yesnocancel":
                    ViewModel.TaskResult = new TaskDialog().SetText("Text").SetInstructionText("InstructionText").ShowYesNo(true)?.ToString() ?? "取消";
                    break;

                case "check":
                    bool isChecked = new TaskDialog().SetText("Text").SetInstructionText("InstructionText")
                                   .SetCheckBox("CheckBox描述").ShowAndGetCheckBoxChecked();
                    ViewModel.TaskResult = $"选择框状态：" + isChecked.ToString();
                    break;

                case "select":
                    new TaskDialog().SetText("Text").SetInstructionText("InstructionText")
                        .AddCommandLink("选项1", "这是选项1", l => ViewModel.TaskResult = "选择了选项1")
                        .AddCommandLink("选项2", "这是选项2", l => ViewModel.TaskResult = "选择了选项2")
                        .Show();
                    break;

                case "buttons":
                    new TaskDialog().SetText("Text").SetInstructionText("InstructionText")
                        .AddButton("按钮1", b => ViewModel.TaskResult = "选择了按钮1")
                        .AddButton("按钮2", b => ViewModel.TaskResult = "选择了按钮2")
                        .AddButton("不会关闭的按钮", b => b.Enabled = false, false)
                        .Show();
                    break;

                case "error":
                    new TaskDialog()
                        .ShowError("错误信息", details: "可选错误详情");
                    break;

                case "exception":
                    try
                    {
                        int a = 1 / new List<int>().Count;
                    }
                    catch (Exception ex)
                    {
                        new TaskDialog().ShowError(ex);
                    }
                    break;
            }
        }
    }

    public class WindowsAPICodePackExtensionPanelFileButtonCommand : PanelButtonCommandBase
    {
        public WindowsAPICodePackExtensionPanelFileButtonCommand(WindowsAPICodePackExtensionPanelViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public WindowsAPICodePackExtensionPanelViewModel ViewModel { get; }

        public override void Execute(object parameter)
        {
            var filter = new FileFilterCollection().Add("配置文件", "ini", "xml", "json").Add("文本文件", "txt");
            if (ViewModel.AllFilter)
            {
                filter.AddAll();
            }
            if (ViewModel.UnionFilter)
            {
                filter.AddUnion();
            }
            Window tempWin = null;
            if (parameter.Equals("openw"))
            {
                tempWin = new Window();
                tempWin.Show();
            }
            object result = (parameter as string) switch
            {
                "open" => filter.CreateOpenFileDialog().GetFilePath(),
                "opens" => filter.CreateOpenFileDialog().GetFilePaths(),
                "opend" => filter.CreateOpenFileDialog().GetResult(false),
                "openc" => filter.CreateOpenFileDialog()
                .SetDefault("C:\\Windows", "默认名称.ini", "ini")
                .Apply(d =>
                {
                    d.Title = "自定义标题";
                    d.ShowHiddenItems = true;
                    d.ShowPlacesList = true;
                    d.AllowNonFileSystemItems = true;
                    d.Controls.Add(new CommonFileDialogButton("自定义按钮"));
                    d.Controls.Add(new CommonFileDialogCheckBox("自定义多选框"));
                    d.Controls.Add(new CommonFileDialogComboBox("自定义组合框"));
                    d.Controls.Add(new CommonFileDialogMenu("自定义菜单"));
                })
                .GetResult(false),
                "openw" => filter.CreateOpenFileDialog().SetParent(tempWin).GetFilePath(),
                "save" => filter.CreateSaveFileDialog().GetFilePath(),
                "savei" => filter.CreateSaveFileDialog().GetInputFilePath(),
                "saved" => filter.CreateSaveFileDialog().GetResult(),
                "folder" => new CommonOpenFileDialog().GetFolderPath(),
                _ => throw new ArgumentException()
            };
            ViewModel.FileResult = result switch
            {
                string str => str,
                IEnumerable<string> strs => string.Join("， ", strs),
                CommonOpenFileDialog dialog => $"文件名={dialog.FileName}，筛选器索引={dialog.SelectedFileTypeIndex}",
                CommonSaveFileDialog dialog => $"文件名={dialog.FileName}，输入文件名={dialog.ReadInputFilePath()}，筛选器索引={dialog.SelectedFileTypeIndex}",
                _ => "取消选择"
            };
        }
    }

    /// <summary>
    /// BasicPanel.xaml 的交互逻辑
    /// </summary>
    public partial class WindowsAPICodePackExtensionPanel : UserControl
    {
        public WindowsAPICodePackExtensionPanelViewModel ViewModel { get; } = new WindowsAPICodePackExtensionPanelViewModel();

        public WindowsAPICodePackExtensionPanel()
        {
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}