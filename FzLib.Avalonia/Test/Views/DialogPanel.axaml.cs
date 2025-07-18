using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Linq;
using System;
using FzLib.Avalonia.Test;
using FzLib.Avalonia.Dialogs;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FzLib.Avalonia.Test.ViewModels;

namespace FzLib.Avalonia.Test.Views;

public partial class DialogPanel : UserControl
{
    public DialogPanel()
    {
        DataContext = App.Services.GetRequiredService<DialogViewModel>();
        InitializeComponent();
    }
}