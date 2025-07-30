using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Linq;
using System;
using FzLib.Samples;
using FzLib.Avalonia.Dialogs;
using System.Threading.Tasks;
using FzLib.Avalonia.Controls;
using FzLib.Samples.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FzLib.Samples.Views;

public partial class TaskPanel : UserControl
{
    public TaskPanel()
    {
        DataContext = App.Services.GetRequiredService<TaskViewModel>();
        InitializeComponent();
    }
}