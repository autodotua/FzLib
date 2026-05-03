using System;
using System.Threading.Tasks;
using Avalonia;
using FzLib.Application;
using FzLib.Samples;
using Serilog;
using UnhandledExceptionEventArgs = System.UnhandledExceptionEventArgs;

namespace FzLib.Samples;


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


        //FzLib.Application.UnhandledExceptionCatcher
        UnhandledExceptionCatcher.WithCatcher(() => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args))
            .Catch((e, s) => { Log.Fatal(e, "未捕获的异常"); })
            .Finally(() => { Log.Information("程序结束"); })
            .Run();
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .WithDeveloperTools();
}
