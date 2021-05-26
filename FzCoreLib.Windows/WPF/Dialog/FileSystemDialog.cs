using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static FzLib.Basic.Loop;
using System.Reflection;
using System.IO;
using static FzLib.WPF.Common;

namespace FzLib.WPF.Dialog
{
    public static class FileSystemDialog
    {
        public static WindowOwner DefaultOwner { get; set; } = new WindowOwner();

        public static string GetSaveFile(
           FileFilterCollection filters, bool ensureExtension = true,
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
                if (string.IsNullOrEmpty(System.IO.Path.GetExtension(dialog.FileName)))
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
            FileFilterCollection filters = null,
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
            FileFilterCollection filters = null, string defaultFileName = "",
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

        private static CommonOpenFileDialog GetOpenDialog(string defaultFileName, FileFilterCollection filters)
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

    public class FileFilterCollection
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

        public FileFilterCollection Add(CommonFileDialogFilter filter, EventHandler<StorageOperationEventArgs> e = null)
        {
            CheckUnionIndex();
            filters.Add(filter);
            events.Add(e);
            return this;
        }

        public FileFilterCollection Add(string display, string extensions, EventHandler<StorageOperationEventArgs> e = null)
        {
            filters.Add(new CommonFileDialogFilter(display, extensions));
            events.Add(e);
            return this;
        }

        public FileFilterCollection AddAllFiles(EventHandler<StorageOperationEventArgs> e = null)
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
        public FileFilterCollection AddUnion()
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

    //public class FileOkEventArgs
    //{
    //    public FileOkEventArgs(string fileName, IEnumerable<string> fileNames)
    //    {
    //        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
    //        FileNames =( fileNames ?? throw new ArgumentNullException(nameof(fileNames))).ToArray();
    //    }
    //    public FileOkEventArgs(string fileName)
    //    {
    //        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
    //        FileNames = new string[] { fileName };
    //    }

    //    public string FileName { get;internal set; }
    //    public string[] FileNames { get;internal set; }
    //}
}