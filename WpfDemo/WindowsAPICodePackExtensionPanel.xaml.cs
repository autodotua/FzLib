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

namespace FzLib.WpfDemo
{
    public class WindowsAPICodePackExtensionPanelViewModel : INotifyPropertyChanged
    {
        public WindowsAPICodePackExtensionPanelViewModel()
        {
            ButtonCommand = new WindowsAPICodePackExtensionPanelButtonCommand(this);
        }

        public bool UnionFilter { get; set; }
        public bool AllFilter { get; set; }
        public string DefaultName { get; set; }
        private string result;

        public string Result
        {
            get => result;
            set => this.SetValueAndNotify(ref result, value, nameof(Result));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public WindowsAPICodePackExtensionPanelButtonCommand ButtonCommand { get; }
    }

    public class WindowsAPICodePackExtensionPanelButtonCommand : PanelButtonCommandBase
    {
        public WindowsAPICodePackExtensionPanelButtonCommand(WindowsAPICodePackExtensionPanelViewModel viewModel)
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
            object result = (parameter as string) switch
            {
                "open" => filter.CreateDialog<CommonOpenFileDialog>().GetFilePath(),
                "opens" => filter.CreateDialog<CommonOpenFileDialog>().GetFilePaths(),
                "opend" => filter.CreateDialog<CommonOpenFileDialog>().GetResult(false),
                "openc" => filter.CreateDialog<CommonOpenFileDialog>()
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
                "save" => filter.CreateDialog<CommonSaveFileDialog>().GetFilePath(),
                "savei" => filter.CreateDialog<CommonSaveFileDialog>().GetInputFilePath(),
                "saved" => filter.CreateDialog<CommonSaveFileDialog>().GetResult(),
                "folder" => new CommonOpenFileDialog().GetFolderPath(),
                _ => throw new ArgumentException()
            };
            ViewModel.Result = result switch
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