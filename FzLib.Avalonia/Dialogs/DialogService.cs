using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using FzLib.Avalonia.Services;
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

        private Task<TopLevel> GetActiveTopLevelAsync()
        {
            if(DefaultTopLevel != null)
            {
                return Task.FromResult(DefaultTopLevel);
            }
            return TopLevelExtension.GetActiveTopLevelAsync(CancellationToken.None);
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

     
    }
}