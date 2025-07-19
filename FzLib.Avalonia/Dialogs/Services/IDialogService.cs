using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Dialogs
{
    public interface IDialogService
    {
        TopLevel DefaultOwner { get; set; }
        DialogContainerType ContainerType { get; set; }
        #region 信息对话框
        Task ShowOkDialogAsync(string title, string message = null, string detail = null);

        Task ShowWarningDialogAsync(string title, string message = null, string detail = null);

        Task<bool> ShowErrorDialogAsync(string title, string message = null, string detail = null, bool retryButton = false);

        Task<bool> ShowErrorDialogAsync(string title, Exception ex, bool retryButton = false);

        Task<bool?> ShowYesNoDialogAsync(string title, string message = null, string detail = null, bool cancelButon = false);
        #endregion

        #region 输入对话框
        Task<string> ShowInputTextDialogAsync(
            string title,
            string message,
            string defaultText = null,
            string watermark = null,
            Action<string> validation = null);

        Task<string> ShowInputMultiLinesTextDialogAsync(
            string title,
            string message,
            int minLines = 3,
            int maxLines = 10,
            string defaultText = null,
            string watermark = null,
            Action<string> validation = null);

        Task<string> ShowInputPasswordDialogAsync(
            string title,
            string message,
            string watermark = null,
            Action<string> validation = null);

        Task<T?> ShowInputNumberDialogAsync<T>(
            string title,
            string message,
            string watermark = null) where T : struct, INumber<T>;

        Task<T?> ShowInputNumberDialogAsync<T>(
            string title,
            string message,
            T defaultValue,
            string watermark = null) where T : struct, INumber<T>;
        #endregion

        #region 选择对话框
        Task<int?> ShowSelectItemDialog(
            string title,
            IList<SelectDialogItem> items,
            string message = null,
            object buttonContent = null,
            Action buttonCommand = null);

        Task<bool> ShowCheckItemDialog(
            string title,
            IList<CheckDialogItem> items,
            string message = null,
            int minCheckCount = 0,
            int maxCheckCount = int.MaxValue);
        #endregion

        #region 自定义对话框
        Task ShowCustomDialogAsync(DialogHost dialog);
        Task<T> ShowCustomDialogAsync<T>(DialogHost dialog);
        #endregion
    }
}