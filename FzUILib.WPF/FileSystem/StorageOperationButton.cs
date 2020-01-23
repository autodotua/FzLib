using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static FzLib.UI.Common;

namespace FzLib.UI.FileSystem
{
    public class StorageOperationButton : FlatStyle.Button
    {

        public StorageOperationButton()
        {
            InitializeComponent();
            Click += ButtonClickEventHandler;
        }

        public IDictionary<string, string> Filters { get; set; }
        public bool ShowHiddenItems { get; set; } = false;
        public bool Multiselect { get; set; } = false;
        public string DefaultFileName { get; set; }
        public bool EnsurePathExists { get; set; } = true;
        public bool EnsureFileExists { get; set; } = true;
        public OperateTypes OperateType { get; set; } = OperateTypes.OpenFile;
        public string DefaultExtension { get; set; }
        public string DefaultDirectory { get; set; }
        public bool AllowNonFileSystemItems { get; set; } = false;
        public bool AddToMostRecentlyUsedList { get; set; } = true;

        private void ButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            PreviewClick?.Invoke(sender, e);
            if (OperateType == OperateTypes.OpenFile || OperateType == OperateTypes.OpenFolder)
            {

                CommonOpenFileDialog oepnDialog = new CommonOpenFileDialog()
                {
                    AddToMostRecentlyUsedList = AddToMostRecentlyUsedList,
                    AllowNonFileSystemItems = AllowNonFileSystemItems,
                    DefaultDirectory = DefaultDirectory,
                    DefaultExtension = DefaultExtension,
                    EnsureFileExists = EnsureFileExists,
                    DefaultFileName = DefaultFileName,
                    EnsurePathExists = EnsurePathExists,
                    IsFolderPicker = OperateType == OperateTypes.OpenFolder,
                    Multiselect = Multiselect,
                    ShowHiddenItems = ShowHiddenItems,

                };
                if (Filters != null)
                {
                    foreach (var i in Filters)
                    {
                        oepnDialog.Filters.Add(new CommonFileDialogFilter(i.Key, i.Value));
                    }
                }
                var result = oepnDialog.ShowDialog();
                if (result == CommonFileDialogResult.Ok)
                {
                    DialogComplete?.Invoke(this, new StorageOperationEventArgs(oepnDialog.FileNames.ToArray(), result));

                }
                else
                {
                    DialogFailed?.Invoke(this, new StorageOperationEventArgs(result));
                }
            }


            else if (OperateType == OperateTypes.SaveFile)
            {

                CommonSaveFileDialog saveDialog = new CommonSaveFileDialog()
                {
                    AddToMostRecentlyUsedList = AddToMostRecentlyUsedList,
                    DefaultDirectory = DefaultDirectory,
                    DefaultExtension = DefaultExtension,
                    EnsureFileExists = EnsureFileExists,
                    DefaultFileName = DefaultFileName,
                    EnsurePathExists = EnsurePathExists,
                    ShowHiddenItems = ShowHiddenItems,

                };
                if (Filters != null)
                {
                    foreach (var i in Filters)
                    {
                        saveDialog.Filters.Add(new CommonFileDialogFilter(i.Key, i.Value));
                    }
                }
                var result = saveDialog.ShowDialog();
                if (result == CommonFileDialogResult.Ok)
                {
                    DialogComplete?.Invoke(this, new StorageOperationEventArgs(saveDialog.FileName, result));
                }

                else
                {
                    DialogFailed?.Invoke(this, new StorageOperationEventArgs(result));

                }
            }
            else
            {
                throw new Exception("未指定操作类型");
            }

        }
        public delegate void PreviewClickHandler(object sender, RoutedEventArgs e);
        public event PreviewClickHandler PreviewClick;

        public delegate void DialogCompleteHandler(object sender, StorageOperationEventArgs e);
        public event DialogCompleteHandler DialogComplete;
        public delegate void DialogFailedHandler(object sender, StorageOperationEventArgs e);
        public event DialogFailedHandler DialogFailed;

        public enum OperateTypes
        {
            OpenFile,
            OpenFolder,
            SaveFile,
        }
    }

}
