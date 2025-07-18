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
using CommunityToolkit.Mvvm.Messaging;
using static FzLib.Avalonia.Test.ViewModels.DialogViewModel;

namespace FzLib.Avalonia.Test.Views;

public partial class DialogPanel : UserControl
{
    public DialogPanel()
    {
        DataContext = App.Services.GetRequiredService<DialogViewModel>();
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<OpenAnotherWindowMessage>(this, (m, _) =>
        {
            (TopLevel.GetTopLevel(this) as Window).WindowState = WindowState.Minimized;
            Window anotherWindow = new Window() { Title = "ÁíÒ»¸ö´°¿Ú", Content = new Grid() };
            anotherWindow.Show();
            anotherWindow.WindowState = WindowState.Minimized;
        });
    }
}