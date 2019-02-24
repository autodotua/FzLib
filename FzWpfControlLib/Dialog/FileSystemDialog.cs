using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;

namespace FzLib.Control.Dialog
{
    public static class FileSystemDialog
    {
        public static CommonOpenFileDialog OpenDialog => new CommonOpenFileDialog();
        public static CommonSaveFileDialog SaveDialog => new CommonSaveFileDialog();

        public static string GetSaveFile(IList<(string display, string extension)> filters = null, bool allExtensions = false, bool ensureExtension = false, string defaultFileName = "")
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

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
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

        public static string[] GetOpenFiles(IList<(string display, string extension)> filters = null, bool allExtensions = false, bool ensureExtension = false, string defaultFileName = "")
        {
            CommonOpenFileDialog dialog = GetDialog(filters, defaultFileName);
            dialog.Multiselect = true;
            if (allExtensions)
            {
                dialog.Filters.Add(new CommonFileDialogFilter("所有文件", "*"));
            }
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
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

        public static string GetOpenFile(IList<(string display, string extension)> filters = null, bool allExtensions = false, bool ensureExtension = false, string defaultFileName = "")
        {
            CommonOpenFileDialog dialog = GetDialog(filters, defaultFileName);
            if (allExtensions)
            {
                dialog.Filters.Add(new CommonFileDialogFilter("所有文件", "*"));
            }
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return null;
            }
            string fileName = dialog.FileName;
            return TryAttachExtension(filters, ensureExtension, dialog, fileName);
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


        public static string GetFolder()
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }
            return null;
        }

    }
}
