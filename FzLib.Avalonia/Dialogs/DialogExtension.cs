using Avalonia;
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
    public static class DialogExtension
    {
        public static DialogContainerType ContainerType { get; set; } = DialogContainerType.PopupPreferred;
        #region 信息
        public static Task ShowOkDialogAsync(this Visual visual, string title, string message = null, string detail = null)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                //Icon = MessageDialog.InfoIcon,
                //IconBrush = grid.Foreground
            }, OK);
            return dialog.ShowDialog(ContainerType, visual);
        }

        public static Task ShowWarningDialogAsync(this Visual visual, string title, string message = null, string detail = null)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                Icon = MessageDialog.WarningIcon,
                IconBrush = SolidColorBrush.Parse("#ffb900")
            }, OK);
            return dialog.ShowDialog(ContainerType, visual);
        }


        public static async Task<bool> ShowErrorDialogAsync(this Visual visual, string title, string message = null, string detail = null, bool retryButton = false)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                Icon = MessageDialog.ErrorIcon,
                IconBrush = Brushes.Red
            }, retryButton ? RetryCancel : OK);
            return await dialog.ShowDialog<bool?>(ContainerType, visual) == true;
        }

        public static async Task<bool> ShowErrorDialogAsync(this Visual visual, string title, Exception ex, bool retryButton = false)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = ex.Message,
                Detail = ex.ToString(),
                Icon = MessageDialog.ErrorIcon,
                IconBrush = Brushes.Red
            }, retryButton ? RetryCancel : OK);
            return await dialog.ShowDialog<bool?>(ContainerType, visual) == true;
        }

        public static async Task<bool?> ShowYesNoDialogAsync(this Visual visual, string title, string message = null, string detail = null, bool cancelButon = false)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                Icon = MessageDialog.QuestionIcon,
                IconBrush = SolidColorBrush.Parse("#ffb900")
            }, cancelButon ? YesNoCancel : YesNo);
            return await dialog.ShowDialog<bool?>(ContainerType, visual);
        }
        #endregion

        #region 输入
        public static async Task<string> ShowInputTextDialogAsync(this Visual visual,
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
            return await dialog.ShowDialog<string>(ContainerType, visual);
        }
        public static async Task<string> ShowInputMultiLinesTextDialogAsync(this Visual visual,
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
            return await dialog.ShowDialog<string>(ContainerType, visual);
        }

        public static async Task<string> ShowInputPasswordDialogAsync(this Visual visual,
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
            return await dialog.ShowDialog<string>(ContainerType, visual);
        }

        public static Task<T?> ShowInputNumberDialogAsync<T>(this Visual visual,
                                                                  string title,
                                                                  string message,
                                                                  string watermark = null) where T : struct, INumber<T>
        {
            return ShowInputNumberDialogAsync<T>(visual, title, message, false, default, watermark);
        }

        public static Task<T?> ShowInputNumberDialogAsync<T>(this Visual visual,
                                                                  string title,
                                                                  string message,
                                                                  T defaultValue,
                                                                  string watermark = null) where T : struct, INumber<T>
        {
            return ShowInputNumberDialogAsync<T>(visual, title, message, true, defaultValue, watermark);
        }

        private static async Task<T?> ShowInputNumberDialogAsync<T>(this Visual visual,
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
            var result = await dialog.ShowDialog<string>(ContainerType, visual);

            return result == null ? null : T.Parse(result, CultureInfo.InvariantCulture);

        }
        #endregion

        #region 选择
        public static async Task<int?> ShowSelectItemDialog(this Visual visual, string title, IList<SelectDialogItem> items, string message = null, object buttonContent = null, Action buttonCommand = null)
        {
            SelectItemDialog dialog = new SelectItemDialog(new SelectItemDialogViewModel()
            {
                Title = title,
                Items = items,
                Message = message,
            }, buttonContent, buttonCommand);
            return await dialog.ShowDialog<int?>(ContainerType, visual);
        }

        public static async Task<bool> ShowCheckItemDialog(this Visual visual, string title, IList<CheckDialogItem> items, string message = null, int minCheckCount = 0, int maxCheckCount = int.MaxValue)
        {
            CheckBoxDialog dialog = new CheckBoxDialog(new CheckBoxDialogViewModel()
            {
                Title = title,
                Items = items,
                Message = message,
            }, minCheckCount, maxCheckCount);
            return await dialog.ShowDialog<bool>(ContainerType, visual);
        }
        #endregion
    }
}