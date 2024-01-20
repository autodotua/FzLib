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
using System.Threading.Tasks;
using System.Windows;
using static FzLib.Avalonia.Dialogs.MessageDialog.MessageDialogButtonDefinition;

namespace FzLib.Avalonia.Dialogs
{
    public static class CommonDialogExtension
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
            }, MessageDialog.MessageDialogButtonDefinition.OK);
            return dialog.ShowDialog(window);
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
            }, MessageDialog.MessageDialogButtonDefinition.OK);
            return dialog.ShowDialog(window);
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
            var button = await dialog.ShowDialog<CommonDialogButtonType>(window);
            return button == CommonDialogButtonType.Primary;
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
            var button = await dialog.ShowDialog<CommonDialogButtonType>(window);
            return button == CommonDialogButtonType.Primary;
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
            var button = await dialog.ShowDialog<CommonDialogButtonType>(window);
            return button switch
            {
                CommonDialogButtonType.Primary => true,
                CommonDialogButtonType.Secondary => false,
                CommonDialogButtonType.Close => null,
                _ => throw new InvalidEnumArgumentException()
            };
        }
        #endregion

        #region 输入
        public static async Task<string> ShowInputTextDialogAsync(this Window window,
                                                                  string title,
                                                                  string message,
                                                                  string defaultText = null,
                                                                  string watermark = null)
        {
            InputDialog dialog = new InputDialog(new InputDialogViewModel()
            {
                Title = title,
                Message = message,
                Text = defaultText,
                Watermark = watermark,
            });
            await dialog.ShowDialog<CommonDialogButtonType>(window);
            return "";
        }
        public static async Task<string> ShowInputMultiLinesTextDialogAsync(this Window window,
                                                                  string title,
                                                                  string message,
                                                                  int minLines=3,
                                                                  int maxLines=10,
                                                                  string defaultText = null,
                                                                  string watermark = null)
        {
            InputDialog dialog = new InputDialog(new InputDialogViewModel()
            {
                Title = title,
                Message = message,
                Text = defaultText,
                Watermark = watermark,
                MultiLines=true,
                MaxLines=maxLines,
                MinHeight=minLines*26,
            });
            await dialog.ShowDialog<CommonDialogButtonType>(window);
            return "";
        }

        public static async Task<string> ShowInputPasswordDialogAsync(this Window window,
                                                                  string title,
                                                                  string message,
                                                                  string watermark = null)
        {
            InputDialog dialog = new InputDialog(new InputDialogViewModel()
            {
                Title = title,
                Message = message,
                Watermark = watermark,
                PasswordChar = '*',
            });
            await dialog.ShowDialog<CommonDialogButtonType>(window);
            return "";
        }
        #endregion
    }
}