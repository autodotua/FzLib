using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using FzLib.Application.Startup;
using FzLib.Avalonia.Dialogs;
using FzLib.Avalonia.Test;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using FzLib.Avalonia.Services;

namespace FzLib.Avalonia.Test;

public partial class App : global::Avalonia.Application
{
    public static IServiceProvider Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        var builder = Host.CreateApplicationBuilder();
        var mainWindow = new MainWindow() { DataContext = new MainViewModel() };
        builder.Services.AddSingleton(s => mainWindow);
        builder.Services.AddDialogService();
        builder.Services.AddDialogService("main", mainWindow);
        builder.Services.AddStartupManager();
        builder.Services.AddStorageProviderService();
        builder.Services.AddClipboardService();
        var host = builder.Build();
        Services = host.Services;
        host.Start();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = Services.GetRequiredService<MainWindow>();
        }


        base.OnFrameworkInitializationCompleted();
    }
}
