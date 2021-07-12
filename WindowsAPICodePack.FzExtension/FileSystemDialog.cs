using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static FzLib.LoopExtension;
using System.Reflection;
using System.IO;
using FzLib.WPF;
using FzLib.WPF.Dialog;
using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.FzExtension
{
    public class FileFilterCollection : List<CommonFileDialogFilter>
    {
        public string AllFilesDisplay { get; set; } = "所有文件";
        public string UnionExtensionsDisplay { get; set; } = "支持的格式";

        public FileFilterCollection()
        {
        }

        public FileFilterCollection(IEnumerable<CommonFileDialogFilter> filters)
        {
            foreach (var filter in filters)
            {
                base.Add(filter);
            }
        }

        public new FileFilterCollection Add(CommonFileDialogFilter filter)
        {
            base.Add(filter);
            return this;
        }

        public FileFilterCollection Add(string display, params string[] extensions)
        {
            base.Add(new CommonFileDialogFilter(display, string.Join(',', extensions)));
            return this;
        }

        public FileFilterCollection AddAll()
        {
            hasAll = true;
            return this;
        }

        internal bool hasUnion = false;
        internal bool hasAll = false;

        /// <summary>
        /// 添加所有支持的格式
        /// </summary>
        /// <returns></returns>
        public FileFilterCollection AddUnion()
        {
            hasUnion = true;
            return this;
        }
    }

    public static class FileSystemDialogExtension
    {
        public static T CreateDialog<T>(this FileFilterCollection filters) where T : CommonFileDialog, new()
        {
            T t = new T();
            return t.SetFilters(filters);
        }

        public static T SetFilters<T>(this T dialog, FileFilterCollection filters) where T : CommonFileDialog
        {
            dialog.Filters.Clear();
            if (filters.hasUnion && dialog is CommonOpenFileDialog)
            {
                dialog.Filters.Add(new CommonFileDialogFilter(
                    filters.UnionExtensionsDisplay, string.Join(',', filters.Select(p => string.Join(',', p.Extensions)))));
            }
            foreach (var filter in filters)
            {
                dialog.Filters.Add(filter);
            }
            if (filters.hasAll)
            {
                dialog.Filters.Add(new CommonFileDialogFilter(filters.AllFilesDisplay, "*"));
            }
            return dialog;
        }

        public static string GetFilePath(this CommonFileDialog dialog)
        {
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return null;
            }
            string path = dialog.FileName;
            if (dialog is CommonSaveFileDialog s && path == s.ReadInputFilePath() + ".*")
            {
                return s.ReadInputFilePath();
            }
            return path;
        }

        public static string GetInputFilePath(this CommonSaveFileDialog dialog)
        {
            return dialog.GetResult()?.ReadInputFilePath();
        }

        public static IEnumerable<string> GetFilePaths(this CommonOpenFileDialog dialog)
        {
            dialog.Multiselect = true;
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return null;
            }
            return dialog.FileNames;
        }

        public static string GetFolderPath(this CommonOpenFileDialog dialog)
        {
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return null;
            }
            return dialog.FileName;
        }

        public static CommonOpenFileDialog GetResult(this CommonOpenFileDialog dialog, bool multiselect = false)
        {
            dialog.Multiselect = multiselect;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dialog;
            }
            return null;
        }

        public static string ReadInputFilePath(this CommonSaveFileDialog dialog)
        {
            if (dialog == null)
            {
                throw new ArgumentNullException(nameof(dialog));
            }
            if (dialog.FileAsShellObject is ShellFile s)
            {
                return s.Path;
            }
            return null;
        }

        public static CommonSaveFileDialog GetResult(this CommonSaveFileDialog dialog)
        {
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dialog;
            }
            return null;
        }

        public static T Apply<T>(this T dialog, Action<T> action) where T : CommonFileDialog
        {
            action(dialog);
            return dialog;
        }

        public static T SetDefault<T>(this T dialog, string defaultName = null, string defaultDir = null, string defaultExt = null) where T : CommonFileDialog
        {
            dialog.DefaultFileName = defaultName;
            dialog.InitialDirectory = defaultDir;
            dialog.DefaultExtension = defaultExt;
            return dialog;
        }
    }
}