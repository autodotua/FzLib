using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;

namespace FzLib.Program.Startup
{
    public static class StartupManagerExtensions
    {
        public static IServiceCollection TryAddStartupManager(this IServiceCollection services)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                services.AddStartupManager();
            }

            return services;
        }

        public static IServiceCollection AddStartupManager(this IServiceCollection services)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                services.AddSingleton<IStartupManager, WindowsStartupManager>();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                     RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                services.AddSingleton<IStartupManager, UnixStartupManager>();
            }
            else
            {
                throw new PlatformNotSupportedException("The platform is not supported.");
            }

            return services;
        }


        public static void EnableStartup(this IStartupManager startupManager, string arguments = "")
        {
            startupManager.EnableStartup(GetAppName(), Process.GetCurrentProcess().MainModule.FileName, arguments);
        }

        private static string GetAppName()
        {
            return AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "");
        }

        public static void DisableStartup(this IStartupManager startupManager)
        {
            startupManager.DisableStartup(GetAppName());
        }

        public static bool IsStartupEnabled(this IStartupManager startupManager)
        {
            return startupManager.IsStartupEnabled(GetAppName());
        }
    }
}