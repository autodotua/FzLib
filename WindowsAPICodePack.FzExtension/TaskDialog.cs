using FzLib.Program;
using FzLib.WPF.Dialog;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;
using WinDialog = Microsoft.WindowsAPICodePack.Dialogs.TaskDialog;

namespace Microsoft.WindowsAPICodePack.FzExtension
{
    public class TaskDialog : IDisposable
    {
        private WinDialog Dialog = new WinDialog();
        public static WindowOwner DefaultOwner { get; set; } = new WindowOwner();

        private static Dictionary<Window, IntPtr> windowHandles = new Dictionary<Window, IntPtr>();

        private TaskDialog(Window window)
        {
            if (window != null)
            {
                IntPtr handle;
                if (!windowHandles.ContainsKey(window))
                {
                    handle = new WindowInteropHelper(window).Handle;
                    window.Closed += (p1, p2) => windowHandles.Remove(window);
                }
                else
                {
                    handle = windowHandles[window];
                }
                Dialog.OwnerWindowHandle = handle;
                Dialog.Opened += (p1, p2) => OpenedDialogCount++;
                Dialog.Closing += (p1, p2) => OpenedDialogCount--;
                Dialog.Cancelable = true;
            }
        }

        public TaskDialogResult Show()
        {
            return Dialog.Show();
        }

        public static int OpenedDialogCount { get; private set; }

        public static void Show(string text, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool cancelable = false)
        {
            Show(DefaultOwner.Owner, text, icon, cancelable);
        }

        public static void Show(Window window, string text, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool cancelable = false)
        {
            using (TaskDialog task = new TaskDialog(window))
            {
                task.Dialog.Caption = App.ProgramName;
                task.Dialog.Text = text;
                task.Dialog.Icon = icon;
                task.Dialog.Cancelable = cancelable;
                task.Show();
            }
        }

        public static void Show(string text, string instructionText, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool cancelable = false)
        {
            Show(DefaultOwner.Owner, text, instructionText, icon, cancelable);
        }

        public static void Show(Window window, string text, string instructionText, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool cancelable = false)
        {
            using (TaskDialog task = new TaskDialog(window))
            {
                task.Dialog.Text = text;
                task.Dialog.InstructionText = instructionText;
                task.Dialog.Icon = icon;
                task.Dialog.Cancelable = cancelable;

                task.Show();
            }
        }

        public static bool? ShowWithCheckBox(string text, string instructionText, string checkBoxText, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool? isChecked = null, bool cancelable = false)
        {
            return ShowWithCheckBox(DefaultOwner.Owner, text, instructionText, checkBoxText, icon, isChecked, cancelable);
        }

        public static bool? ShowWithCheckBox(Window window, string text, string instructionText, string checkBoxText, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool? isChecked = null, bool cancelable = false)
        {
            bool? result;
            using (TaskDialog task = new TaskDialog(window))
            {
                task.Dialog.Text = text;
                task.Dialog.InstructionText = instructionText;
                task.Dialog.FooterCheckBoxChecked = isChecked;
                task.Dialog.FooterCheckBoxText = checkBoxText;
                task.Dialog.Icon = icon;
                task.Dialog.Cancelable = cancelable;

                task.Show();
                result = task.Dialog.FooterCheckBoxChecked;
            }
            return result;
        }

        public static void ShowWithDetail(string text, string instructionText, string detail, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool expandFooter = false, bool cancelable = false, string expandedLabel = "查看详情")
        {
            ShowWithDetail(DefaultOwner.Owner, text, instructionText, detail, icon, expandFooter, cancelable, expandedLabel);
        }

        public static void ShowWithDetail(Window window, string text, string instructionText, string detail, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool expandFooter = false, bool cancelable = false, string expandedLabel = "查看详情")
        {
            using (TaskDialog task = new TaskDialog(window))
            {
                task.Dialog.Text = text;
                task.Dialog.InstructionText = instructionText;
                task.Dialog.DetailsExpandedLabel = expandedLabel;
                task.Dialog.DetailsExpandedText = detail;
                task.Dialog.Icon = icon;
                task.Dialog.Cancelable = cancelable;
                if (expandFooter)
                {
                    task.Dialog.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter;
                }
                task.Show();
            }
        }

        public static bool? ShowWithYesNoButtons(string text, string instructionText, string detail = null, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool cancelable = false, string yesButtonText = "是", string noButtonText = "否", string expandedLabel = "查看详情")
        {
            return ShowWithYesNoButtons(DefaultOwner.Owner, text, instructionText, detail, icon, cancelable, yesButtonText, noButtonText, expandedLabel);
        }

        public static bool? ShowWithYesNoButtons(Window window, string text, string instructionText, string detail = null, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool cancelable = false, string yesButtonText = "是", string noButtonText = "否", string expandedLabel = "查看详情")
        {
            bool? result = null;
            using (TaskDialog task = new TaskDialog(window))
            {
                task.Dialog.Text = text;
                task.Dialog.InstructionText = instructionText;
                task.Dialog.DetailsExpandedLabel = expandedLabel;
                task.Dialog.DetailsExpandedText = detail;
                TaskDialogButton yesButton = new TaskDialogButton("yes", yesButtonText);
                yesButton.Click += (p1, p2) =>
                {
                    result = true;
                    task.Dialog.Close();
                };
                TaskDialogButton noButton = new TaskDialogButton("no", noButtonText);
                noButton.Click += (p1, p2) =>
                {
                    result = false;
                    task.Dialog.Close();
                };
                task.Dialog.Controls.Add(yesButton);
                task.Dialog.Controls.Add(noButton);
                task.Dialog.Cancelable = cancelable;
                task.Show();
            }
            return result;
        }

        public static void ShowWithButtons(string text, string instructionText, IEnumerable<(string text, Action click)> buttons, string detail = null, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool cancelable = false, string expandedLabel = "查看详情")
        {
            ShowWithButtons(text, instructionText, buttons, detail, icon, cancelable, expandedLabel);
        }

        public static void ShowWithButtons(Window window, string text, string instructionText, IEnumerable<(string text, Action click)> buttons, string detail = null, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool cancelable = false, string expandedLabel = "查看详情")
        {
            using (TaskDialog task = new TaskDialog(window))
            {
                task.Dialog.Text = text;
                task.Dialog.InstructionText = instructionText;
                task.Dialog.DetailsExpandedLabel = expandedLabel;
                task.Dialog.DetailsExpandedText = detail;
                task.Dialog.Cancelable = cancelable;
                foreach ((string buttonText, Action action) in buttons)
                {
                    TaskDialogButton button = new TaskDialogButton(buttonText, buttonText);
                    button.Click += (p1, p2) =>
                    {
                        action();
                        task.Dialog.Close();
                    };
                    task.Dialog.Controls.Add(button);
                }
                task.Show();
            }
        }

        public static string ShowWithCommandLinks(string text, string instructionText, IEnumerable<(string text, string instruction, Action click)> buttons, string detail = null, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool cancelable = false, string expandedLabel = "查看详情")
        {
            return ShowWithCommandLinks(DefaultOwner.Owner, text, instructionText, buttons, detail, icon, cancelable, expandedLabel);
        }

        public static string ShowWithCommandLinks(Window window, string text, string instructionText, IEnumerable<(string text, string instruction, Action click)> buttons, string detail = null, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool cancelable = false, string expandedLabel = "查看详情")
        {
            string result = null;
            using (TaskDialog task = new TaskDialog(window))
            {
                task.Dialog.Text = text;
                task.Dialog.InstructionText = instructionText;
                task.Dialog.DetailsExpandedLabel = expandedLabel;
                task.Dialog.DetailsExpandedText = detail;
                task.Dialog.Cancelable = cancelable;
                foreach ((string buttonText, string instruction, Action click) in buttons)
                {
                    TaskDialogCommandLink button = new TaskDialogCommandLink(buttonText, buttonText, instruction);
                    button.Click += (p1, p2) =>
                    {
                        click?.Invoke();
                        task.Dialog.Close();
                        result = button.Text;
                    };
                    task.Dialog.Controls.Add(button);
                }
                task.Show();
            }
            return result;
        }

        public static void ShowException(Exception ex, string message = null, bool cancelable = false)
        {
            ShowException(DefaultOwner.Owner, ex, message, cancelable);
        }

        public static void ShowException(Window win, Exception ex, string message = null, bool cancelable = false)
        {
            ShowWithDetail(win, ex.Message, message ?? "程序发生异常", ex.ToString(), TaskDialogStandardIcon.Error, false, cancelable, "查看详细错误");
        }

        public static void ShowError(string title, string detail)
        {
            ShowError(DefaultOwner.Owner, title, detail);
        }

        public static void ShowError(Window win, string title, string detail)
        {
            ShowWithDetail(win, title, "错误", detail, TaskDialogStandardIcon.Error, false, true, "查看详细错误");
        }

        public static void ShowError(string title)
        {
            ShowError(DefaultOwner.Owner, title);
        }

        public static void ShowError(Window win, string title)
        {
            Show(win, title, "错误", TaskDialogStandardIcon.Error, true);
        }

        public void Dispose()
        {
            Dialog.Dispose();
        }
    }
}