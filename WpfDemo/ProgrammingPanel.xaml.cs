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
using FzLib.Cryptography;
using System;
using System.Security.Cryptography;
using System.Collections;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.FzExtension;
using ModernWpf.FzExtension.CommonDialog;
using System.IO;
using FzLib.DataStorage.Serialization;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace FzLib.WpfDemo
{
    public class ProgrammingPanelViewModel : INotifyPropertyChanged
    {
        public ProgrammingPanelViewModel()
        {
            ButtonCommand = new ProgrammingPanelButtonCommand(this);
        }

        public ProgrammingPanelButtonCommand ButtonCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ProgrammingPanelButtonCommand : PanelButtonCommandBase
    {
        public ProgrammingPanelButtonCommand(ProgrammingPanelViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public ProgrammingPanelViewModel ViewModel { get; }

        public override void Execute(object parameter)
        {
            Do(() =>
            {
                switch (parameter as string)
                {
                }
            });
        }
    }

    /// <summary>
    /// BasicPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ProgrammingPanel : UserControl
    {
        public ProgrammingPanelViewModel ViewModel { get; } = new ProgrammingPanelViewModel();

        public ProgrammingPanel()
        {
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}