using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Linq;
using System;
using FzLib.Samples;
using FzLib.Avalonia.Dialogs;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FzLib.Samples.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using static FzLib.Samples.ViewModels.DialogViewModel;

namespace FzLib.Samples.Views;

public partial class DialogPanel : UserControl
{
    public DialogPanel()
    {
        DataContext = App.Services.GetRequiredService<DialogViewModel>();
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<OpenAnotherWindowMessage>(this, (m, _) =>
        {
            (TopLevel.GetTopLevel(this) as Window).WindowState = WindowState.Minimized;
            Window anotherWindow = new Window() { Title = "��һ������", Content = new Grid() };
            anotherWindow.Show();
            anotherWindow.WindowState = WindowState.Minimized;
        });
    }
}