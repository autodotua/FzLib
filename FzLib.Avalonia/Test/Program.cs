using System;
using System.Threading.Tasks;
using Avalonia;
using FzLib.Application;
using FzLib.Avalonia.Test;
using Serilog;
using UnhandledExceptionEventArgs = System.UnhandledExceptionEventArgs;

namespace FzLib.Avalonia.Test;


class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        Log.Information("程序启动");

        UnhandledExceptionCatcher.WithCatcher(() => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args))
            .Catch((e, s) => { Log.Fatal("未捕获的异常", e); })
            .Finally(() => { })
            .Run();
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}
