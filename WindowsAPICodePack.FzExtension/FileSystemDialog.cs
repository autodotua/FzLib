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

    [Obsolete]
    public static class FileSystemDialog
    {
        public static WindowOwner DefaultOwner { get; set; } = new WindowOwner();

        public static string GetSaveFile(
           FileFilterCollectionLegacy filters, bool ensureExtension = true,
            string defaultFileName = "",
            Action<CommonSaveFileDialog> otherSetting = null,
            Window owner = null)
        {
            var dialog = new CommonSaveFileDialog
            {
                AlwaysAppendDefaultExtension = true,
                DefaultFileName = defaultFileName
            };
            filters.Filters.ForEach(p => dialog.Filters.Add(p));
            otherSetting?.Invoke(dialog);
            //dialog.AlwaysAppendDefaultExtension = ensureExtension;

            if (dialog.ShowDialog(owner ?? DefaultOwner.Owner) != CommonFileDialogResult.Ok)
            {
                return null;
            }
            if (ensureExtension && filters.Filters.Any())
            {
                var filter = filters.Filters.ToArray()[dialog.SelectedFileTypeIndex - 1];
                if (string.IsNullOrEmpty(Path.GetExtension(dialog.FileName)))
                {
                    string newName = dialog.FileName + "." + filter.Extensions.First();
                    filters.Raise(dialog, newName);
                    return newName;
                }
            }
            filters.Raise(dialog);
            return dialog.FileName;
        }

        public static string[] GetOpenFiles(
            FileFilterCollectionLegacy filters = null,
            string defaultFileName = "",
            Action<CommonOpenFileDialog> otherSetting = null,
            Window owner = null)
        {
            var dialog = GetOpenDialog(defaultFileName, filters);
            dialog.Multiselect = true;
            otherSetting?.Invoke(dialog);
            if (dialog.ShowDialog(owner ?? DefaultOwner.Owner) != CommonFileDialogResult.Ok)
            {
                return null;
            }
            filters.Raise(dialog);
            return dialog.FileNames.ToArray();
        }

        public static string GetOpenFile(
            FileFilterCollectionLegacy filters = null, string defaultFileName = "",
             Action<CommonOpenFileDialog> otherSetting = null,
             Window owner = null)
        {
            var dialog = GetOpenDialog(defaultFileName, filters);
            otherSetting?.Invoke(dialog);
            if (dialog.ShowDialog(owner ?? DefaultOwner.Owner) != CommonFileDialogResult.Ok)
            {
                return null;
            }
            filters.Raise(dialog);
            return dialog.FileName;
        }

        private static CommonOpenFileDialog GetOpenDialog(string defaultFileName, FileFilterCollectionLegacy filters)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                DefaultFileName = defaultFileName,
            };
            filters.Filters.ForEach(p => dialog.Filters.Add(p));
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

        public static string GetFolder(Action<CommonOpenFileDialog> otherSetting = null, Window owner = null)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
            };
            otherSetting?.Invoke(dialog);
            if (dialog.ShowDialog(owner ?? DefaultOwner.Owner) == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }
            return null;
        }
    }

    [Obsolete]
    public class FileFilterCollectionLegacy
    {
        public static string AllFilesDisplay { get; set; } = "所有文件";
        public static string UnionExtensionsDisplay { get; set; } = "支持的格式";
        private List<CommonFileDialogFilter> filters = new List<CommonFileDialogFilter>();
        private List<EventHandler<StorageOperationEventArgs>> events = new List<EventHandler<StorageOperationEventArgs>>();
        public IReadOnlyCollection<CommonFileDialogFilter> Filters => filters.AsReadOnly();
        private int unionIndex = -1;

        internal void Raise(CommonFileDialog dialog, string newName = null)
        {
            if (dialog.SelectedFileTypeIndex >= 0)
            {
                if (newName == null)
                {
                    newName = dialog.FileName;
                }
                EventHandler<StorageOperationEventArgs> @event = null;
                //如果Type是结合的，那么要获取后缀名，然后进行查询，到底是哪一个
                if (dialog.SelectedFileTypeIndex - 1 == unionIndex)
                {
                    string ext = Path.GetExtension(newName);
                    for (int i = 0; i < filters.Count; i++)
                    {
                        if (filters[i].Extensions.Contains(ext))
                        {
                            @event = events[i];
                        }
                    }
                }
                else
                {
                    @event = events[dialog.SelectedFileTypeIndex - 1];
                }
                if (dialog is CommonOpenFileDialog oDialog)
                {
                    @event?.Invoke(this, new StorageOperationEventArgs(oDialog.FileNames.ToArray()));
                }
                else
                {
                    @event?.Invoke(this, new StorageOperationEventArgs(newName));
                }
            }
        }

        public FileFilterCollectionLegacy Add(CommonFileDialogFilter filter, EventHandler<StorageOperationEventArgs> e = null)
        {
            CheckUnionIndex();
            filters.Add(filter);
            events.Add(e);
            return this;
        }

        public FileFilterCollectionLegacy Add(string display, string extensions, EventHandler<StorageOperationEventArgs> e = null)
        {
            filters.Add(new CommonFileDialogFilter(display, extensions));
            events.Add(e);
            return this;
        }

        public FileFilterCollectionLegacy AddAllFiles(EventHandler<StorageOperationEventArgs> e = null)
        {
            CheckUnionIndex();
            filters.Add(new CommonFileDialogFilter(AllFilesDisplay, "*"));
            events.Add(e);
            return this;
        }

        /// <summary>
        /// 添加所有支持的格式
        /// </summary>
        /// <returns></returns>
        public FileFilterCollectionLegacy AddUnion()
        {
            if (unionIndex >= 0)
            {
                return this;
            }
            unionIndex = filters.Count;
            List<string> allExtensions = new List<string>();
            //将所有已经添加的格式进行结合
            filters.ForEach(p => allExtensions.AddRange(p.Extensions));
            filters.Insert(0, new CommonFileDialogFilter(UnionExtensionsDisplay, string.Join(",", allExtensions)));
            return this;
        }

        private void CheckUnionIndex()
        {
            if (unionIndex >= 0)
            {
                throw new Exception("Call AddUnion Method Last");
            }
        }
    }

    [Obsolete]
    public class StorageOperationEventArgs : EventArgs
    {
        public StorageOperationEventArgs()
        {
        }

        public StorageOperationEventArgs(string name)
        {
            Names = new[] { name };
        }

        public StorageOperationEventArgs(string[] names)
        {
            Names = names;
        }

        public string Name
        {
            get
            {
                if (Names.Length == 1)
                {
                    return Names[0];
                }
                throw new Exception("Number of files is not ONE");
            }
        }

        public string[] Names { get; private set; }
        //public CommonFileDialogResult Result { get; private set; }
    }
}