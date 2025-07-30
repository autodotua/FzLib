using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Linq;
using System;
using FzLib.Samples;
using FzLib.Avalonia.Dialogs;
using System.Threading.Tasks;
using FzLib.Samples.ViewModels;

namespace FzLib.Samples.Views;

public partial class ConverterPanel : UserControl
{
    public ConverterPanel()
    {
        DataContext = new ConverterViewModel();
        InitializeComponent();
    }
}