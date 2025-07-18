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
        public DialogContainerType ContainerType { get; set; }

        public TopLevel DefaultTopLevel { get; set; } = null;

        private Task<TopLevel> GetActiveTopLevelAsync()
        {
            if (DefaultTopLevel != null)
            {
                return Task.FromResult(DefaultTopLevel);
            }
            return TopLevelExtension.GetActiveTopLevelAsync(CancellationToken.None);
        }

        #region 信息
        public async Task<bool> ShowErrorDialogAsync(string title, string message = null, string detail = null, bool retryButton = false)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                Icon = MessageDialog.ErrorIcon,
                IconBrush = Brushes.Red
            }, retryButton ? RetryCancel : OK);
            return await dialog.ShowDialog<bool?>(ContainerType, await GetActiveTopLevelAsync()) == true;
        }

        public async Task<bool> ShowErrorDialogAsync(string title, Exception ex, bool retryButton = false)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = ex.Message,
                Detail = ex.ToString(),
                Icon = MessageDialog.ErrorIcon,
                IconBrush = Brushes.Red
            }, retryButton ? RetryCancel : OK);
            return await dialog.ShowDialog<bool?>(ContainerType, await GetActiveTopLevelAsync()) == true;
        }

        public async Task ShowOkDialogAsync(string title, string message = null, string detail = null)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                //Icon = MessageDialog.InfoIcon,
                //IconBrush = grid.Foreground
            }, OK);
            await dialog.ShowDialog(ContainerType, await GetActiveTopLevelAsync());
        }

        public async Task ShowWarningDialogAsync(string title, string message = null, string detail = null)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                Icon = MessageDialog.WarningIcon,
                IconBrush = SolidColorBrush.Parse("#ffb900")
            }, OK);
            await dialog.ShowDialog(ContainerType, await GetActiveTopLevelAsync());
        }
        public async Task<bool?> ShowYesNoDialogAsync(string title, string message = null, string detail = null, bool cancelButon = false)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                Icon = MessageDialog.QuestionIcon,
                IconBrush = SolidColorBrush.Parse("#ffb900")
            }, cancelButon ? YesNoCancel : YesNo);
            return await dialog.ShowDialog<bool?>(ContainerType, await GetActiveTopLevelAsync());
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
            InputDialog dialog = new InputDialog(new InputDialogViewModel()
            {
                Title = title,
                Message = message,
                text = defaultText,
                Watermark = watermark,
                MultiLines = true,
                MaxLines = maxLines,
                MinHeight = minLines * 24,
                Validations = { validation, InputDialog.NotNullValidation }
            });
            return await dialog.ShowDialog<string>(ContainerType, await GetActiveTopLevelAsync());
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
            InputDialog dialog = new InputDialog(new InputDialogViewModel()
            {
                Title = title,
                Message = message,
                Watermark = watermark,
                PasswordChar = '*',
                Validations = { validation, InputDialog.NotNullValidation }
            });
            return await dialog.ShowDialog<string>(ContainerType, await GetActiveTopLevelAsync());
        }

        public async Task<string> ShowInputTextDialogAsync(
                                                                                                 string title,
                                                                 string message,
                                                                 string defaultText = null,
                                                                 string watermark = null,
                                                                 Action<string> validation = null)
        {
            InputDialog dialog = new InputDialog(new InputDialogViewModel()
            {
                Title = title,
                Message = message,
                text = defaultText,
                Watermark = watermark,
                Validations = { validation, InputDialog.NotNullValidation }
            });
            return await dialog.ShowDialog<string>(ContainerType, await GetActiveTopLevelAsync());
        }
        private async Task<T?> ShowInputNumberDialogAsync<T>(
                                                                  string title,
                                                                  string message,
                                                                  bool hasDefaultValue,
                                                                  T defaultValue,
                                                                  string watermark = null) where T : struct, INumber<T>
        {
            InputDialog dialog = new InputDialog(new InputDialogViewModel()
            {
                Title = title,
                Message = message,
                Watermark = watermark,
                text = hasDefaultValue ? defaultValue.ToString() : null,
                Validations = { InputDialog.NotNullValidation, InputDialog.GetNumberValidation<T>() }
            });
            var result = await dialog.ShowDialog<string>(ContainerType, await GetActiveTopLevelAsync());

            return result == null ? null : T.Parse(result, CultureInfo.InvariantCulture);

        }
        #endregion

        #region 选择
        public async Task<bool> ShowCheckItemDialog(string title, IList<CheckDialogItem> items, string message = null, int minCheckCount = 0, int maxCheckCount = int.MaxValue)
        {
            CheckBoxDialog dialog = new CheckBoxDialog(new CheckBoxDialogViewModel()
            {
                Title = title,
                Items = items,
                Message = message,
            }, minCheckCount, maxCheckCount);
            return await dialog.ShowDialog<bool>(ContainerType, await GetActiveTopLevelAsync());
        }

        public async Task<int?> ShowSelectItemDialog(string title, IList<SelectDialogItem> items, string message = null, object buttonContent = null, Action buttonCommand = null)
        {
            SelectItemDialog dialog = new SelectItemDialog(new SelectItemDialogViewModel()
            {
                Title = title,
                Items = items,
                Message = message,
            }, buttonContent, buttonCommand);
            return await dialog.ShowDialog<int?>(ContainerType, await GetActiveTopLevelAsync());
        }

        #endregion

        public async Task ShowCustomDialogAsync(DialogHost dialog)
        {
            await dialog.ShowDialog(ContainerType, await GetActiveTopLevelAsync());
        }

        public async Task<T> ShowCustomDialogAsync<T>(DialogHost dialog)
        {
            return await dialog.ShowDialog<T>(ContainerType, await GetActiveTopLevelAsync());
        }
    }
}