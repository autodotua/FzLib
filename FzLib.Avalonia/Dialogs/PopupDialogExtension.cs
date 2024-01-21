using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Threading.Tasks;
using static FzLib.Avalonia.Dialogs.MessageDialog.MessageDialogButtonDefinition;

namespace FzLib.Avalonia.Dialogs
{
    public static class PopupDialogExtension
    {
        #region 信息
        public static Task ShowOkDialogAsync(this Grid grid, string title, string message = null, string detail = null)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                //Icon = MessageDialog.InfoIcon,
                //IconBrush = grid.Foreground
            }, OK);
            return dialog.ShowPopupDialog(grid);
        }

        public static Task ShowWarningDialogAsync(this Grid grid, string title, string message = null, string detail = null)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                Icon = MessageDialog.WarningIcon,
                IconBrush = SolidColorBrush.Parse("#ffb900")
            }, OK);
            return dialog.ShowPopupDialog(grid);
        }


        public static async Task<bool> ShowErrorDialogAsync(this Grid grid, string title, string message = null, string detail = null, bool retryButton = false)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                Icon = MessageDialog.ErrorIcon,
                IconBrush = Brushes.Red
            }, retryButton ? RetryCancel : OK);
            return await dialog.ShowPopupDialog<bool?>(grid) == true;
        }

        public static async Task<bool> ShowErrorDialogAsync(this Grid grid, string title, Exception ex, bool retryButton = false)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = ex.Message,
                Detail = ex.ToString(),
                Icon = MessageDialog.ErrorIcon,
                IconBrush = Brushes.Red
            }, retryButton ? RetryCancel : OK);
            return await dialog.ShowPopupDialog<bool?>(grid) == true;
        }

        public static async Task<bool?> ShowYesNoDialogAsync(this Grid grid, string title, string message = null, string detail = null, bool cancelButon = false)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                Icon = MessageDialog.QuestionIcon,
                IconBrush = SolidColorBrush.Parse("#ffb900")
            }, cancelButon ? YesNoCancel : YesNo);
            return await dialog.ShowPopupDialog<bool?>(grid);
        }
        #endregion

        #region 输入
        public static async Task<string> ShowInputTextDialogAsync(this Grid grid,
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
            return await dialog.ShowPopupDialog<string>(grid);
        }
        public static async Task<string> ShowInputMultiLinesTextDialogAsync(this Grid grid,
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
            return await dialog.ShowPopupDialog<string>(grid);
        }

        public static async Task<string> ShowInputPasswordDialogAsync(this Grid grid,
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
            return await dialog.ShowPopupDialog<string>(grid);
        }

        public static Task<T> ShowInputNumberDialogAsync<T>(this Grid grid,
                                                                  string title,
                                                                  string message,
                                                                  string watermark = null) where T : INumber<T>
        {
            return ShowInputNumberDialogAsync<T>(grid, title, message, false, default, watermark);
        }

        public static Task<T> ShowInputNumberDialogAsync<T>(this Grid grid,
                                                                  string title,
                                                                  string message,
                                                                  T defaultValue,
                                                                  string watermark = null) where T : INumber<T>
        {
            return ShowInputNumberDialogAsync<T>(grid, title, message, true, defaultValue, watermark);
        }

        private static async Task<T> ShowInputNumberDialogAsync<T>(this Grid grid,
                                                                  string title,
                                                                  string message,
                                                                  bool hasDefaultValue,
                                                                  T defaultValue,
                                                                  string watermark = null) where T : INumber<T>
        {
            InputDialog dialog = new InputDialog(new InputDialogViewModel()
            {
                Title = title,
                Message = message,
                Watermark = watermark,
                text = hasDefaultValue ? defaultValue.ToString() : null,
                Validations = { InputDialog.NotNullValidation, InputDialog.GetNumberValidation<T>() }
            });
            var result = await dialog.ShowPopupDialog<string>(grid);

            return T.Parse(result, CultureInfo.InvariantCulture);

        }
        #endregion

        #region 选择
        public static async Task<int?> ShowSelectItemDialog(this Grid grid, string title, IList<SelectDialogItem> items, string message = null, object buttonContent = null, Action buttonCommand = null)
        {
            SelectItemDialog dialog = new SelectItemDialog(new SelectItemDialogViewModel()
            {
                Title = title,
                Items = items,
                Message = message,
            }, buttonContent, buttonCommand);
            return await dialog.ShowPopupDialog<int?>(grid);
        }

        public static async Task<bool> ShowCheckItemDialog(this Grid grid, string title, IList<CheckDialogItem> items, string message = null, int minCheckCount = 0, int maxCheckCount = int.MaxValue)
        {
            CheckBoxDialog dialog = new CheckBoxDialog(new CheckBoxDialogViewModel()
            {
                Title = title,
                Items = items,
                Message = message,
            }, minCheckCount, maxCheckCount);
            return await dialog.ShowPopupDialog<bool>(grid);
        }
        #endregion
    }
}