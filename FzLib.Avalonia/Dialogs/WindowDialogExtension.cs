using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using static FzLib.Avalonia.Dialogs.MessageDialog.MessageDialogButtonDefinition;

namespace FzLib.Avalonia.Dialogs
{
    public static class WindowDialogExtension
    {
        #region 信息
        public static Task ShowOkDialogAsync(this Window window, string title, string message = null, string detail = null)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                //Icon = MessageDialog.InfoIcon,
                //IconBrush = window.Foreground
            }, OK);
            return dialog.ShowWindowDialog(window);
        }

        public static Task ShowWarningDialogAsync(this Window window, string title, string message = null, string detail = null)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                Icon = MessageDialog.WarningIcon,
                IconBrush = SolidColorBrush.Parse("#ffb900")
            }, OK);
            return dialog.ShowWindowDialog(window);
        }


        public static async Task<bool> ShowErrorDialogAsync(this Window window, string title, string message = null, string detail = null, bool retryButton = false)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                Icon = MessageDialog.ErrorIcon,
                IconBrush = Brushes.Red
            }, retryButton ? RetryCancel : OK);
            return await dialog.ShowWindowDialog<bool?>(window) == true;
        }

        public static async Task<bool> ShowErrorDialogAsync(this Window window, string title, Exception ex, bool retryButton = false)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = ex.Message,
                Detail = ex.ToString(),
                Icon = MessageDialog.ErrorIcon,
                IconBrush = Brushes.Red
            }, retryButton ? RetryCancel : OK);
            return await dialog.ShowWindowDialog<bool?>(window) == true;
        }

        public static async Task<bool?> ShowYesNoDialogAsync(this Window window, string title, string message = null, string detail = null, bool cancelButon = false)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                Icon = MessageDialog.QuestionIcon,
                IconBrush = SolidColorBrush.Parse("#ffb900")
            }, cancelButon ? YesNoCancel : YesNo);
            return await dialog.ShowWindowDialog<bool?>(window);
        }
        #endregion

        #region 输入
        public static async Task<string> ShowInputTextDialogAsync(this Window window,
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
            return await dialog.ShowWindowDialog<string>(window);
        }
        public static async Task<string> ShowInputMultiLinesTextDialogAsync(this Window window,
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
            return await dialog.ShowWindowDialog<string>(window);
        }

        public static async Task<string> ShowInputPasswordDialogAsync(this Window window,
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
            return await dialog.ShowWindowDialog<string>(window);
        }

        public static Task<T?> ShowInputNumberDialogAsync<T>(this Window window,
                                                                  string title,
                                                                  string message,
                                                                  string watermark = null) where T : struct, INumber<T>
        {
            return ShowInputNumberDialogAsync<T>(window, title, message, false, default, watermark);
        }

        public static Task<T?> ShowInputNumberDialogAsync<T>(this Window window,
                                                                  string title,
                                                                  string message,
                                                                  T defaultValue,
                                                                  string watermark = null) where T : struct, INumber<T>
        {
            return ShowInputNumberDialogAsync<T>(window, title, message, true, defaultValue, watermark);
        }

        private static async Task<T?> ShowInputNumberDialogAsync<T>(this Window window,
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
            var result = await dialog.ShowWindowDialog<string>(window);

            return result == null ? null : T.Parse(result, CultureInfo.InvariantCulture);

        }
        #endregion

        #region 选择
        public static async Task<int?> ShowSelectItemDialog(this Window window, string title, IList<SelectDialogItem> items, string message = null, object buttonContent = null, Action buttonCommand = null)
        {
            SelectItemDialog dialog = new SelectItemDialog(new SelectItemDialogViewModel()
            {
                Title = title,
                Items = items,
                Message = message,
            }, buttonContent, buttonCommand);
            return await dialog.ShowWindowDialog<int?>(window);
        }

        public static async Task<bool> ShowCheckItemDialog(this Window window, string title, IList<CheckDialogItem> items, string message = null, int minCheckCount = 0, int maxCheckCount = int.MaxValue)
        {
            CheckBoxDialog dialog = new CheckBoxDialog(new CheckBoxDialogViewModel()
            {
                Title = title,
                Items = items,
                Message = message,
            }, minCheckCount, maxCheckCount);
            return await dialog.ShowWindowDialog<bool>(window);
        }
        #endregion
    }
}