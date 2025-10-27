using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FzLib.Avalonia.Dialogs;
using FzLib.Samples;
using FzLib.Samples.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FzLib.Samples.Views;

public partial class IdentityPanel : UserControl
{
    public IdentityPanel()
    {
        DataContext = App.Services.GetRequiredService<IdentityViewModel>();
        InitializeComponent();
    }
}