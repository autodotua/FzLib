using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Linq;
using System;
using FzLib.Avalonia.Test;
using FzLib.Avalonia.Dialogs;
using System.Threading.Tasks;
using FzLib.Avalonia.Controls;
using FzLib.Avalonia.Test.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FzLib.Avalonia.Test.Views;

public partial class TaskPanel : UserControl
{
    public TaskPanel()
    {
        DataContext = App.Services.GetRequiredService<TaskViewModel>();
        InitializeComponent();
    }
}