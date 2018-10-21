using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;
using WinDialog = Microsoft.WindowsAPICodePack.Dialogs.TaskDialog;
namespace FzLib.Wpf.Program.Notify
{
    public class TaskDialog : IDisposable
    {
        WinDialog Dialog = new WinDialog();
        Window window;
        private static Dictionary<Window, IntPtr> windowHandles = new Dictionary<Window, IntPtr>();
        private TaskDialog(Window window)
        {
            this.window = window;
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
            }
        }



        public TaskDialogResult Show()
        {
            return Dialog.Show();
        }

        public static void Show(Window window, string text, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None)
        {
            using (TaskDialog task = new TaskDialog(window))
            {
                task.Dialog.Caption = Information.ProgramName;
                task.Dialog.Text = text;
                task.Dialog.Icon = icon;

                task.Show();
            }
        }
        public static void Show(Window window, string text, string instructionText, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None)
        {
            using (TaskDialog task = new TaskDialog(window))
            {
                task.Dialog.Text = text;
                task.Dialog.InstructionText = instructionText;
                task.Dialog.Icon = icon;

                task.Show();
            }

        }
        public static bool? ShowWithCheckBox(Window window, string text, string instructionText, string checkBoxText, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool? isChecked = null)
        {
            bool? result;
            using (TaskDialog task = new TaskDialog(window))
            {
                task.Dialog.Text = text;
                task.Dialog.InstructionText = instructionText;
                task.Dialog.FooterCheckBoxChecked = isChecked;
                task.Dialog.FooterCheckBoxText = checkBoxText;
                task.Dialog.Icon = icon;

                task.Show();
                result = task.Dialog.FooterCheckBoxChecked;
            }
            return result;
        }
        public static void ShowWithDetail(Window window, string text, string instructionText, string detail, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, bool expandFooter = false, string expandedLabel = "查看详情")
        {
            using (TaskDialog task = new TaskDialog(window))
            {
                task.Dialog.Text = text;
                task.Dialog.InstructionText = instructionText;
                task.Dialog.DetailsExpandedLabel = expandedLabel;
                task.Dialog.DetailsExpandedText = detail;
                task.Dialog.Icon = icon;
                if (expandFooter)
                {
                    task.Dialog.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter;
                }
                task.Show();
            }
        }
        public static bool? ShowWithYesNoButtons(Window window, string text, string instructionText, string detail = null, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, string yesButtonText = "是", string noButtonText = "否", string expandedLabel = "查看详情")
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
                task.Show();

            }
            return result;
        }

        public static void ShowWithButtons(Window window, string text, string instructionText, IEnumerable<(string text, Action click)> buttons, string detail = null, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, string expandedLabel = "查看详情")
        {
            using (TaskDialog task = new TaskDialog(window))
            {
                task.Dialog.Text = text;
                task.Dialog.InstructionText = instructionText;
                task.Dialog.DetailsExpandedLabel = expandedLabel;
                task.Dialog.DetailsExpandedText = detail;
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

        public static void ShowWithCommandLinks(Window window, string text, string instructionText, IEnumerable<(string text, string instruction, Action click)> buttons, string detail = null, TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, string expandedLabel = "查看详情")
        {
            using (TaskDialog task = new TaskDialog(window))
            {
                task.Dialog.Text = text;
                task.Dialog.InstructionText = instructionText;
                task.Dialog.DetailsExpandedLabel = expandedLabel;
                task.Dialog.DetailsExpandedText = detail;
                foreach ((string buttonText, string instruction, Action click) in buttons)
                {
                    TaskDialogCommandLink button = new TaskDialogCommandLink(buttonText, buttonText, instruction);
                    button.Click += (p1, p2) =>
                    {
                        click();
                        task.Dialog.Close();
                    };
                    task.Dialog.Controls.Add(button);
                }
                task.Show();
            }
        }



        public void Dispose()
        {
            Dialog.Dispose();
        }

    }
}
