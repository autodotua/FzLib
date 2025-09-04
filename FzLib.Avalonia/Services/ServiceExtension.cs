using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using FzLib.Avalonia.Controls;
using FzLib.Avalonia.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Services
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddClipboardService(this IServiceCollection services)
        {
            services.AddSingleton<IClipboardService, ClipboardService>();
            return services;
        }

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

        public static IServiceCollection AddProgressOverlayService(this IServiceCollection services)
        {
            return services.AddSingleton<IProgressOverlayService, ProgressOverlayService>();
        }

        public static IServiceCollection AddStorageProviderService(this IServiceCollection services)
        {
            services.AddSingleton<IStorageProviderService, StorageProviderService>();
            return services;
        }

        public static async Task<TopLevel> GetActiveTopLevelAsync(CancellationToken cancellationToken = default) => global::Avalonia.Application.Current.ApplicationLifetime switch
        {
            IClassicDesktopStyleApplicationLifetime desktopLifetime => await GetDesktopActiveWindowAsync(desktopLifetime, cancellationToken),
            ISingleViewApplicationLifetime singleView => TopLevel.GetTopLevel(singleView.MainView) ?? throw new InvalidOperationException("无法获取单视图的TopLevel"),
            _ => throw new InvalidOperationException("不支持的应用程序生命周期类型")
        };

        public static TopLevel GetMainTopLevel()
        {
            if (global::Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                return desktopLifetime.MainWindow;
            }
            else if (global::Avalonia.Application.Current.ApplicationLifetime is ISingleViewApplicationLifetime singleViewLifetime)
            {
                return TopLevel.GetTopLevel(singleViewLifetime.MainView);
            }
            else
            {
                return null;
            }
        }

        private static async Task<TopLevel> GetDesktopActiveWindowAsync(IClassicDesktopStyleApplicationLifetime lifetime,
                                                                                                         CancellationToken cancellationToken)
        {
            // 首先尝试获取活动窗口
            var activeWindow = lifetime.Windows.FirstOrDefault(w => w.IsActive);
            if (activeWindow != null)
            {
                return activeWindow;
            }

            // 如果没有活动窗口，等待窗口激活事件
            var tcs = new TaskCompletionSource<global::Avalonia.Controls.TopLevel>();

            void Handler(object sender, EventArgs e)
            {
                if (lifetime.Windows.FirstOrDefault(w => w.IsActive) is { } window)
                {
                    tcs.TrySetResult(window);
                }
            }

            try
            {
                // 监听所有窗口的激活事件
                foreach (var window in lifetime.Windows)
                {
                    window.Activated += Handler;
                }

                // 设置超时和取消
                cancellationToken.Register(() => tcs.TrySetResult(null));

                // 再次检查避免竞态条件
                activeWindow = lifetime.Windows.FirstOrDefault(w => w.IsActive);
                if (activeWindow != null)
                {
                    return activeWindow;
                }

                return await tcs.Task;
            }
            finally
            {
                foreach (var window in lifetime.Windows)
                {
                    window.Activated -= Handler;
                }
            }
        }
    }
}
