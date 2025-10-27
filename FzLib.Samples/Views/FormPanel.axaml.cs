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
using Microsoft.Extensions.DependencyInjection;

namespace FzLib.Samples.Views;

public partial class FormPanel : UserControl
{
    public FormPanel()
    {
        DataContext = App.Services.GetRequiredService<FormViewModel>();
        InitializeComponent();
    }
}