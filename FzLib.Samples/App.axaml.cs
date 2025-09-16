using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using FzLib.Application.Startup;
using FzLib.Avalonia.Dialogs;
using FzLib.Samples;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;
using FzLib.Avalonia.Services;
using FzLib.Samples.ViewModels;
using FzLib.Samples.Views;
using FzLib.Avalonia.Controls;

namespace FzLib.Samples;

public partial class App : global::Avalonia.Application
{
    public static IServiceProvider Services { get; private set; }

    void Preserve()
    {
        
    }
    
    public override void Initialize()
    {
        Preserve();
        _ = typeof(DialogItemBase);
        _ = typeof(SelectDialogItem);
        _ = typeof(CheckDialogItem);
        AvaloniaXamlLoader.Load(this);
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddProgressOverlayService();
        builder.Services.AddDialogService();
        builder.Services.AddDialogService("main",
            () => (ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow);

        builder.Services.AddStartupManager();
        builder.Services.AddStorageProviderService();
        builder.Services.AddClipboardService();

        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<DialogViewModel>();
        builder.Services.AddTransient<FileSystemViewModel>();
        builder.Services.AddTransient<ConverterViewModel>();
        builder.Services.AddTransient<TaskViewModel>();
        builder.Services.AddTransient<JsonViewModel>();

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
            desktop.MainWindow = new MainWindow();
        }


        base.OnFrameworkInitializationCompleted();
    }
}