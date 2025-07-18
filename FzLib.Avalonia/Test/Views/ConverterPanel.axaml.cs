using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Linq;
using System;
using FzLib.Avalonia.Test;
using FzLib.Avalonia.Dialogs;
using System.Threading.Tasks;
using FzLib.Avalonia.Test.ViewModels;

namespace FzLib.Avalonia.Test.Views;

public partial class ConverterPanel : UserControl
{
    public ConverterPanel()
    {
        DataContext = new ConverterViewModel();
        InitializeComponent();
    }
}