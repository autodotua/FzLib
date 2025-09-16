using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Linq;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FzLib.Samples;
using FzLib.Avalonia.Dialogs;
using System.Threading.Tasks;
using FzLib.Avalonia.Controls;
using FzLib.Samples.ViewModels;
using FzLib.Text;
using Microsoft.Extensions.DependencyInjection;

namespace FzLib.Samples.Views;

public partial class JsonPanel : UserControl
{
    public JsonPanel()
    {
        DataContext = App.Services.GetRequiredService<JsonViewModel>();
        InitializeComponent();
    }
}

