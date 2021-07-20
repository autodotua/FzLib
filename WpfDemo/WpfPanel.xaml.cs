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
using FzLib.WPF.Controls;
using Microsoft.WindowsAPICodePack.FzExtension;
using System.Collections.ObjectModel;

namespace FzLib.WpfDemo
{
    public class WpfPanelViewModel : INotifyPropertyChanged
    {
        public WpfPanelViewModel()
        {
            ButtonCommand = new WpfPanelButtonCommand(this);
        }

        public Panel ExportPanel { get; set; }
        public string ExportPanelFormat { get; set; } = "PNG";
        public string[] ExportPanelFormats { get; set; } = new[] { "JPG", "PNG", "XPS", "打印对话框" };
        public double ExportPanelBlood { get; set; } = 0;
        public double ExportPanelMagnification { get; set; } = 1;
        private System.Drawing.Color dColor = System.Drawing.Color.FromArgb(0xFF, 0xFE, 0x00, 0x00);

        public System.Drawing.Color DColor
        {
            get => dColor;
            set => this.SetValueAndNotify(ref dColor, value, nameof(DColor));
        }

        private Color mColor = Color.FromArgb(0xFF, 0xFE, 0x00, 0x00);

        public Color MColor
        {
            get => mColor;
            set => this.SetValueAndNotify(ref mColor, value, nameof(MColor));
        }

        public ObservableCollection<int> IntList { get; } = new ObservableCollection<int>();

        public event PropertyChangedEventHandler PropertyChanged;

        public WpfPanelButtonCommand ButtonCommand { get; }
    }

    public class WpfPanelButtonCommand : PanelButtonCommandBase
    {
        public WpfPanelButtonCommand(WpfPanelViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public WpfPanelViewModel ViewModel { get; }

        public override void Execute(object parameter)
        {
            Do(async () =>
            {
                switch (parameter as string)
                {
                    case "export":
                    case "exportBlack":
                        var background = parameter.Equals("exportBlack") ? Brushes.Black : null;
                        string path;
                        PanelExport pe = new PanelExport(ViewModel.ExportPanel, ViewModel.ExportPanelBlood, ViewModel.ExportPanelMagnification);
                        switch (ViewModel.ExportPanelFormat)
                        {
                            case "JPG":
                                if ((path = new FileFilterCollection().Add("JPG图片", "jpg").CreateSaveFileDialog().GetFilePath()) != null)
                                {
                                    pe.ExportToJpg(path, 70, background);
                                }
                                break;

                            case "PNG":
                                if ((path = new FileFilterCollection().Add("PNG图片", "png").CreateSaveFileDialog().GetFilePath()) != null)
                                {
                                    pe.ExportToPng(path, background);
                                }
                                break;

                            case "XPS":
                                if ((path = new FileFilterCollection().Add("XPS文档", "xps").CreateSaveFileDialog().GetFilePath()) != null)
                                {
                                    pe.ExportToXps(path, background);
                                }
                                break;

                            default:
                                pe.ExportByPrinting(background);
                                break;
                        }
                        break;

                    case "intListAdd":
                        ViewModel.IntList.Add(ViewModel.IntList.Count);
                        ViewModel.Notify(nameof(ViewModel.IntList));
                        break;

                    case "intListRemove":
                        if (ViewModel.IntList.Count > 0)
                        {
                            ViewModel.IntList.RemoveAt(ViewModel.IntList.Count - 1);
                            ViewModel.Notify(nameof(ViewModel.IntList));
                        }
                        break;

                    default:
                        break;
                }
            });
        }
    }

    /// <summary>
    /// BasicPanel.xaml 的交互逻辑
    /// </summary>
    public partial class WpfPanel : UserControl
    {
        public WpfPanelViewModel ViewModel { get; } = new WpfPanelViewModel();

        public WpfPanel()
        {
            DataContext = ViewModel;
            InitializeComponent();
            ViewModel.ExportPanel = grdPanel;
        }

        private void ExportPanel_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}