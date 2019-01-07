using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;

namespace FzLib.Control.Dialog
{
    public static class CommonFileSystemDialog
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

        public static string GetOpenFile(IList<(string display, string extension)> filters = null, bool allExtensions = false, bool ensureExtension = false, string defaultFileName = "")
        {
            var dialog = new CommonOpenFileDialog
            {
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
