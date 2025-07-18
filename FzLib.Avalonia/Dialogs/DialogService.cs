using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Dialogs
{

    public class DialogService : IDialogService
    {
        public TopLevel DefaultTopLevel { get; set; } = null;

        public async Task<TopLevel> GetActiveTopLevelAsync(CancellationToken cancellationToken = default)
        {
            if (DefaultTopLevel != null)
            {
                return DefaultTopLevel;
            }
            return global::Avalonia.Application.Current.ApplicationLifetime switch
            {
                IClassicDesktopStyleApplicationLifetime desktopLifetime => await GetDesktopActiveWindowAsync(desktopLifetime, cancellationToken),
                ISingleViewApplicationLifetime singleView => TopLevel.GetTopLevel(singleView.MainView) ?? throw new InvalidOperationException("无法获取单视图的TopLevel"),
                _ => throw new InvalidOperationException("不支持的应用程序生命周期类型")
            };
        }

        public async Task<bool> ShowCheckItemDialog(string title, IList<CheckDialogItem> items, string message = null, int minCheckCount = 0, int maxCheckCount = int.MaxValue)
        {
            var topLevel = await GetActiveTopLevelAsync();
            return await DialogExtension.ShowCheckItemDialog(topLevel, title, items, message, minCheckCount, maxCheckCount);
        }

        public async Task<bool> ShowErrorDialogAsync(string title, string message = null, string detail = null, bool retryButton = false)
        {
            var topLevel = await GetActiveTopLevelAsync();
            return await DialogExtension.ShowErrorDialogAsync(topLevel, title, message, detail, retryButton);
        }

        public async Task<bool> ShowErrorDialogAsync(string title, Exception ex, bool retryButton = false)
        {
            var topLevel = await GetActiveTopLevelAsync();
            return await DialogExtension.ShowErrorDialogAsync(topLevel, title, ex, retryButton);
        }

        public async Task<string> ShowInputMultiLinesTextDialogAsync(string title, string message, int minLines = 3, int maxLines = 10, string defaultText = null, string watermark = null, Action<string> validation = null)
        {
            var topLevel = await GetActiveTopLevelAsync();
            return await DialogExtension.ShowInputMultiLinesTextDialogAsync(topLevel, title, message, minLines, maxLines, defaultText, watermark, validation);
        }

        public async Task<T?> ShowInputNumberDialogAsync<T>(string title, string message, string watermark = null) where T : struct, INumber<T>
        {
            var topLevel = await GetActiveTopLevelAsync();
            return await DialogExtension.ShowInputNumberDialogAsync<T>(topLevel, title, message, watermark);
        }

        public async Task<T?> ShowInputNumberDialogAsync<T>(string title, string message, T defaultValue, string watermark = null) where T : struct, INumber<T>
        {
            var topLevel = await GetActiveTopLevelAsync();
            return await DialogExtension.ShowInputNumberDialogAsync<T>(topLevel, title, message, defaultValue, watermark);
        }

        public async Task<string> ShowInputPasswordDialogAsync(string title, string message, string watermark = null, Action<string> validation = null)
        {
            var topLevel = await GetActiveTopLevelAsync();
            return await DialogExtension.ShowInputPasswordDialogAsync(topLevel, title, message, watermark, validation);
        }

        public async Task<string> ShowInputTextDialogAsync(string title, string message, string defaultText = null, string watermark = null, Action<string> validation = null)
        {
            var topLevel = await GetActiveTopLevelAsync();
            return await DialogExtension.ShowInputTextDialogAsync(topLevel, title, message, defaultText, watermark, validation);
        }

        public async Task ShowOkDialogAsync(string title, string message = null, string detail = null)
        {
            var topLevel = await GetActiveTopLevelAsync();
            await DialogExtension.ShowOkDialogAsync(topLevel, title, message, detail);
        }

        public async Task<int?> ShowSelectItemDialog(string title, IList<SelectDialogItem> items, string message = null, object buttonContent = null, Action buttonCommand = null)
        {
            var topLevel = await GetActiveTopLevelAsync();
            return await DialogExtension.ShowSelectItemDialog(topLevel, title, items, message, buttonContent, buttonCommand);
        }

        public async Task ShowWarningDialogAsync(string title, string message = null, string detail = null)
        {
            var topLevel = await GetActiveTopLevelAsync();
            await DialogExtension.ShowWarningDialogAsync(topLevel, title, message, detail);
        }

        public async Task<bool?> ShowYesNoDialogAsync(string title, string message = null, string detail = null, bool cancelButon = false)
        {
            var topLevel = await GetActiveTopLevelAsync();
            return await DialogExtension.ShowYesNoDialogAsync(topLevel, title, message, detail, cancelButon);
        }

        private async Task<TopLevel> GetDesktopActiveWindowAsync(IClassicDesktopStyleApplicationLifetime lifetime,
                                                                                                            CancellationToken cancellationToken)
        {
            // 首先尝试获取活动窗口
            var activeWindow = lifetime.Windows.FirstOrDefault(w => w.IsActive);
            if (activeWindow != null)
            {
                return activeWindow;
            }

            // 如果没有活动窗口，等待窗口激活事件
            var tcs = new TaskCompletionSource<TopLevel>();

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