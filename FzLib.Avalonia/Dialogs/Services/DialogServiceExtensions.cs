using Avalonia.Controls;
using FzLib.Application.Startup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace FzLib.Avalonia.Dialogs
{
    public static class DialogServiceExtensions
    {
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CheckDialogItem))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(SelectDialogItem))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CheckDialogItem))]
        public static IServiceCollection AddDialogService(this IServiceCollection services)
        {
            return services.AddDialogService(null);
        }

        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CheckDialogItem))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(SelectDialogItem))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CheckDialogItem))]
        public static IServiceCollection AddDialogService(this IServiceCollection services, string key, Func<TopLevel> getTopLevel)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            services.AddKeyedSingleton<IDialogService>(key, (provider, k) =>
            {
                var dialogService = new DialogService
                {
                    DefaultOwner = getTopLevel()
                };
                return dialogService;
            });
            return services;
        }

        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CheckDialogItem))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(SelectDialogItem))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CheckDialogItem))]
        public static IServiceCollection AddDialogService(this IServiceCollection services, global::Avalonia.Controls.TopLevel defaultTopLevel)
        {
            services.AddSingleton<IDialogService>(provider =>
            {
                var dialogService = new DialogService
                {
                    DefaultOwner = defaultTopLevel
                };
                return dialogService;
            });
            return services;
        }
    }
}