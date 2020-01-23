using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;

namespace FzLib.UI.Dialog
{
    public static class FileSystemDialog
    {
        public static WindowOwner DefaultOwner { get; set; } = new WindowOwner();
        public static string GetSaveFile(IList<(string display, string extension)> filters = null,
            bool allExtensions = false, bool ensureExtension = false,
            string defaultFileName = "", Action<CommonSaveFileDialog> otherSetting = null)
        {
            return (GetSaveFile(DefaultOwner.Owner, filters,
             allExtensions, ensureExtension, defaultFileName, otherSetting));
        }
        public static string GetSaveFile(Window owner,
            IList<(string display, string extension)> filters = null,
            bool allExtensions = false, bool ensureExtension = false,
            string defaultFileName = "", Action<CommonSaveFileDialog> otherSetting = null)
        {
            var dialog = new CommonSaveFileDialog
            {
                AlwaysAppendDefaultExtension = true,
                DefaultFileName = defaultFileName
            };
            if (filters != null)
            {
                foreach ((string display, string extension) in filters)
                {
                    dialog.Filters.Add(new CommonFileDialogFilter(display, extension));
                }

            }
            if (allExtensions)
            {
                dialog.Filters.Add(new CommonFileDialogFilter("所有文件", "*"));
            }
            otherSetting?.Invoke(dialog);

            if (dialog.ShowDialog(owner) == CommonFileDialogResult.Ok)
            {
                string fileName = dialog.FileName;
                if (ensureExtension && filters != null && dialog.SelectedFileTypeIndex <= filters.Count)
                {
                    string extension = filters[dialog.SelectedFileTypeIndex - 1].extension;
                    if (!fileName.EndsWith("." + extension))
                    {
                        if (!fileName.EndsWith("."))
                        {
                            fileName += ".";
                        }
                        fileName += extension;
                    }
                }
                return fileName;
            }
            else
            {
                return null;
            }
        }

        public static string[] GetOpenFiles(
                    IList<(string display, string extension)> filters = null,
                    bool allExtensions = false, bool ensureExtension = false,
                    string defaultFileName = "", Action<CommonOpenFileDialog> otherSetting = null)
        {
            return GetOpenFiles(DefaultOwner.Owner, filters,
             allExtensions, ensureExtension,
             defaultFileName, otherSetting);
        }
        public static string[] GetOpenFiles(Window owner,
            IList<(string display, string extension)> filters = null,
            bool allExtensions = false, bool ensureExtension = false,
            string defaultFileName = "", Action<CommonOpenFileDialog> otherSetting = null)
        {
            CommonOpenFileDialog dialog = GetDialog(filters, defaultFileName);
            dialog.Multiselect = true;
            if (allExtensions)
            {
                dialog.Filters.Add(new CommonFileDialogFilter("所有文件", "*"));
            }
            otherSetting?.Invoke(dialog);
            if (dialog.ShowDialog(owner) != CommonFileDialogResult.Ok)
            {
                return null;
            }
            List<string> files = new List<string>();
            foreach (var fileName in dialog.FileNames)
            {
                files.Add(TryAttachExtension(filters, ensureExtension, dialog, fileName));
            }
            return files.ToArray();
        }

        public static string GetOpenFile(
            IList<(string display, string extension)> filters = null,
            bool allExtensions = false, string defaultFileName = "",
             Action<CommonOpenFileDialog> otherSetting = null)
        {
            return GetOpenFile(DefaultOwner.Owner, filters,
             allExtensions, defaultFileName, otherSetting);
        }

        public static string GetOpenFile(Window owner,
            IList<(string display, string extension)> filters = null,
            bool allExtensions = false, string defaultFileName = "",
             Action<CommonOpenFileDialog> otherSetting = null)
        {
            CommonOpenFileDialog dialog = GetDialog(filters, defaultFileName);
            if (allExtensions)
            {
                dialog.Filters.Add(new CommonFileDialogFilter("所有文件", "*"));
            }
            otherSetting?.Invoke(dialog);
            if (dialog.ShowDialog(owner) != CommonFileDialogResult.Ok)
            {
                return null;
            }
            string fileName = dialog.FileName;
            return fileName;
        }

        private static CommonOpenFileDialog GetDialog(IList<(string display, string extension)> filters, string defaultFileName)
        {
            var dialog = new CommonOpenFileDialog
            {
                DefaultFileName = defaultFileName,
            };
            if (filters != null)
            {
                foreach ((string display, string extension) in filters)
                {
                    dialog.Filters.Add(new CommonFileDialogFilter(display, extension));
                }

            }

            return dialog;
        }

        private static string TryAttachExtension(IList<(string display, string extension)> filters, bool ensureExtension, CommonOpenFileDialog dialog, string fileName)
        {
            if (ensureExtension && filters != null && dialog.SelectedFileTypeIndex <= filters.Count)
            {
                string extension = filters[dialog.SelectedFileTypeIndex - 1].extension;
                if (!fileName.EndsWith("." + extension))
                {
                    if (!fileName.EndsWith("."))
                    {
                        fileName += ".";
                    }
                    fileName += extension;
                }
            }

            return fileName;
        }

        public static string GetFolder(Action<CommonOpenFileDialog> otherSetting = null)
        {
            return GetFolder(DefaultOwner.Owner, otherSetting);
        }
        public static string GetFolder(Window owner,
            Action<CommonOpenFileDialog> otherSetting = null)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
            };
            otherSetting?.Invoke(dialog);
            if (dialog.ShowDialog(owner) == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }
            return null;
        }

    }
}
