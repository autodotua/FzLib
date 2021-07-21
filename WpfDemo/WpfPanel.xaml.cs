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
using System.Windows.Media.Animation;
using FzLib.WPF;

namespace FzLib.WpfDemo
{
    public enum EnumWithDescription
    {
        [Description("上")]
        Up,

        [Description("下")]
        Down,

        [Description("左")]
        Left,

        [Description("右")]
        Right
    }

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

        public Array DayOfWeeks => Enum.GetValues(typeof(DayOfWeek));

        public Array EnumWithDescriptions => Enum.GetValues(typeof(EnumWithDescription));
        public Array DateTimeKinds => Enum.GetValues(typeof(DateTimeKind));

        private long fileLength = 12345;

        public long FileLength
        {
            get => fileLength;
            set => this.SetValueAndNotify(ref fileLength, value, nameof(FileLength));
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

        private async void AnimationButton_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
            Storyboard storyboard = new Storyboard();
            new DoubleAnimation(200, TimeSpan.FromSeconds(1))
                .SetInOutCubicEase()
                .StopWhenComplete()
                .SetStoryboard(WidthProperty, rect)
                .AddTo(storyboard);
            new DoubleAnimation(200, TimeSpan.FromSeconds(1))
                .SetInOutCubicEase()
                .StopWhenComplete()
                .SetStoryboard("(Rectangle.RenderTransform).(TranslateTransform.X)", rect)
                .AddTo(storyboard);
            new ColorAnimation(Colors.Green, TimeSpan.FromSeconds(1))
                .StopWhenComplete()
                .SetStoryboard("Fill.(SolidColorBrush.Color)", rect)
                .AddTo(storyboard);
            await storyboard.BeginAsync();
            IsEnabled = true;
        }

        private async void Animation2Button_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
            Storyboard storyboard = new Storyboard();
            new DoubleAnimation(200, TimeSpan.FromSeconds(1))
                .SetInOutCubicEase()
                .EnableAutoReverse()
                .SetRepeat(3)//重复3遍
                .SetStoryboard(WidthProperty, rect)
                .AddTo(storyboard);
            new DoubleAnimation(200, TimeSpan.FromSeconds(1))
                .SetInOutCubicEase()
                .SetRepeat(TimeSpan.FromSeconds(10))//重复到10s时结束
                .EnableAutoReverse()
                .SetStoryboard("(Rectangle.RenderTransform).(TranslateTransform.X)", rect)
                .AddTo(storyboard);
            new ColorAnimation(Colors.Green, TimeSpan.FromSeconds(1))
                .EnableAutoReverse()
                .SetRepeat(3)
                .SetStoryboard("Fill.(SolidColorBrush.Color)", rect)
                .AddTo(storyboard);
            await storyboard.BeginAsync();
            IsEnabled = true;
        }

        private async void Animation3Button_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            await new DoubleAnimation(200, TimeSpan.FromSeconds(1))
                      .SetInOutCubicEase()
                      .EnableAutoReverse()
                      .SetRepeat(3)//重复3遍
                      .BeginAsync(rect, WidthProperty);

            IsEnabled = true;
        }
    }
}