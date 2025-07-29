using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using FzLib.Avalonia.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using static FzLib.Avalonia.Dialogs.MessageDialog.MessageDialogButtonDefinition;

namespace FzLib.Avalonia.Dialogs
{
    public class DialogService : IDialogService
    {
        public DialogService()
        {
        }

        public DialogService(TopLevel owner)
        {
            DefaultOwner = owner;
        }

        public DialogContainerType ContainerType { get; set; }

        public TopLevel DefaultOwner { get; set; } = null;

        private Task<TopLevel> GetActiveTopLevelAsync()
        {
            if (ContainerType == DialogContainerType.ModelessWindow)
            {
                return Task.FromResult<TopLevel>(null);
            }

            if (DefaultOwner != null)
            {
                return Task.FromResult(DefaultOwner);
            }

            return TopLevelExtension.GetActiveTopLevelAsync(CancellationToken.None);
        }

        #region 信息

        public async Task<bool> ShowErrorDialogAsync(string title, string message = null, string detail = null,
            bool retryButton = false)
        {
            return await new MessageDialog(retryButton ? RetryCancel : OK, title, message, detail,
                    MessageDialogContent.ErrorIcon, Brushes.Red)
                .ShowDialog<bool?>(ContainerType, await GetActiveTopLevelAsync()) == true;
        }

        public Task<bool> ShowErrorDialogAsync(string title, Exception ex, bool retryButton = false)
        {
            return ShowErrorDialogAsync(title, ex.Message, ex.ToString(), retryButton);
        }

        public async Task ShowOkDialogAsync(string title, string message = null, string detail = null)
        {
            await new MessageDialog(OK, title, message, detail)
                .ShowDialog(ContainerType, await GetActiveTopLevelAsync());
        }

        public async Task ShowWarningDialogAsync(string title, string message = null, string detail = null)
        {
            await new MessageDialog(OK, title, message, detail,
                    MessageDialogContent.WarningIcon, SolidColorBrush.Parse("#ffb900"))
                .ShowDialog(ContainerType, await GetActiveTopLevelAsync());
        }

        public async Task<bool?> ShowYesNoDialogAsync(string title, string message = null, string detail = null,
            bool cancelButon = false)
        {
            return await new MessageDialog(cancelButon ? YesNoCancel : YesNo, title, message, detail,
                    MessageDialogContent.QuestionIcon, SolidColorBrush.Parse("#ffb900"))
                .ShowDialog<bool?>(ContainerType, await GetActiveTopLevelAsync());
        }

        #endregion

        #region 输入

        public async Task<string> ShowInputMultiLinesTextDialogAsync(
            string title,
            string message,
            int minLines = 3,
            int maxLines = 10,
            string defaultText = null,
            string watermark = null,
            Action<string> validation = null)
        {
            return await new InputDialog(title, message, defaultText,
                    true, minLines, maxLines,
                    validations: [validation, InputDialog.NotNullValidation])
                .ShowDialog<string>(ContainerType, await GetActiveTopLevelAsync());
        }

        public async Task<T?> ShowInputNumberDialogAsync<T>(
            string title,
            string message,
            string watermark = null) where T : struct, INumber<T>
        {
            return await ShowInputNumberDialogAsync<T>(title, message, false, default, watermark);
        }

        public async Task<T?> ShowInputNumberDialogAsync<T>(
            string title,
            string message,
            T defaultValue,
            string watermark = null) where T : struct, INumber<T>
        {
            return await ShowInputNumberDialogAsync<T>(title, message, true, defaultValue, watermark);
        }

        public async Task<string> ShowInputPasswordDialogAsync(
            string title,
            string message,
            string watermark = null,
            Action<string> validation = null)
        {
            return await new InputDialog(title, message, passwordChar: '*', watermark: watermark,
                    validations: [validation, InputDialog.NotNullValidation])
                .ShowDialog<string>(ContainerType, await GetActiveTopLevelAsync());
        }

        public async Task<string> ShowInputTextDialogAsync(
            string title,
            string message,
            string defaultText = null,
            string watermark = null,
            Action<string> validation = null)
        {
            return await new InputDialog(title, message, defaultText, watermark: watermark,
                    validations: [validation, InputDialog.NotNullValidation])
                .ShowDialog<string>(ContainerType, await GetActiveTopLevelAsync());
        }

        private async Task<T?> ShowInputNumberDialogAsync<T>(
            string title,
            string message,
            bool hasDefaultValue,
            T defaultValue,
            string watermark = null) where T : struct, INumber<T>
        {
            var result = await new InputDialog(title, message, hasDefaultValue ? defaultValue.ToString() : null,
                    watermark: watermark,
                    validations: [InputDialog.NotNullValidation, InputDialog.GetNumberValidation<T>()])
                .ShowDialog<string>(ContainerType, await GetActiveTopLevelAsync());

            return result == null ? null : T.Parse(result, CultureInfo.InvariantCulture);
        }

        #endregion

        #region 选择

        public async Task<bool> ShowCheckItemDialog(string title, IList<CheckDialogItem> items, string message = null,
            int minCheckCount = 0, int maxCheckCount = int.MaxValue)
        {
            return await new CheckBoxDialog(title, message, items, minCheckCount, maxCheckCount)
                .ShowDialog<bool>(ContainerType, await GetActiveTopLevelAsync());
        }

        public async Task<int?> ShowSelectItemDialog(string title, IList<SelectDialogItem> items, string message = null,
            object buttonContent = null, Action buttonCommand = null)
        {
            return await new SelectItemDialog(title, message, items)
                .ShowDialog<int?>(ContainerType, await GetActiveTopLevelAsync());
        }

        #endregion

        #region 自定义

        public async Task ShowCustomDialogAsync(DialogHost dialog)
        {
            await dialog.ShowDialog(ContainerType, await GetActiveTopLevelAsync());
        }

        public async Task<T> ShowCustomDialogAsync<T>(DialogHost dialog)
        {
            return await dialog.ShowDialog<T>(ContainerType, await GetActiveTopLevelAsync());
        }

        #endregion
    }
}