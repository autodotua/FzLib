using Avalonia.Controls;
using FzLib.Application.Startup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;

namespace FzLib.Avalonia.Dialogs
{
    public static class DialogServiceExtensions
    {
        public static IServiceCollection AddDialogService(this IServiceCollection services)
        {
            return services.AddDialogService(null);
        }

        public static IServiceCollection AddDialogService(this IServiceCollection services, string key, global::Avalonia.Controls.TopLevel defaultTopLevel)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            services.AddKeyedSingleton<IDialogService>(key, (provider, k) =>
            {
                var dialogService = new DialogService
                {
                    DefaultTopLevel = defaultTopLevel
                };
                return dialogService;
            });
            return services;
        }

        public static IServiceCollection AddDialogService(this IServiceCollection services, global::Avalonia.Controls.TopLevel defaultTopLevel)
        {
            services.AddSingleton<IDialogService>(provider =>
            {
                var dialogService = new DialogService
                {
                    DefaultTopLevel = defaultTopLevel
                };
                return dialogService;
            });
            return services;
        }
    }
}