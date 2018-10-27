using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace FzLib.Control.FileSystem
{
    public class FileDropListBox : FlatStyle.ListBox
    {
        public FileDropListBox()
        {
            Name = "fdlb";
            AllowDrop = true;
            SelectionMode = System.Windows.Controls.SelectionMode.Extended;
            PreviewKeyDown += PreviewKeyDownEventHandler;
            PreviewDragOver += FileDragEnter;
            PreviewDrop += DropEventHandler;
            ItemsSource = FileList;
            //Binding binding = new Binding(nameof(FileList)) { ElementName = Name };
            //   SetBinding(ItemsSourceProperty, binding);
        }

        public ObservableCollection<string> FileList { get; } = new ObservableCollection<string>();

        private void PreviewKeyDownEventHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!AllowDragDrop)
            {
                return;
            }
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                foreach (var file in SelectedItems.Cast<string>().ToArray())
                {
                    FileList.Remove(file);
                }
            }
        }

        private void FileDragEnter(object sender, DragEventArgs e)
        {
            if (!AllowDragDrop)
            {
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = GetAvailableFiles(e.Data.GetData(DataFormats.FileDrop) as string[]);
                if (files.Length > 0)
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
                e.Handled = true;
            }
        }
        public FileAcceptMode AcceptMode { get; set; } = FileAcceptMode.FilesAndFilesInFolders;
        public string Filter { get; set; } = ".*";
        public bool PressDeleteKeyToDeleteItem { get; set; } = true;

        //public string[] Files => Items.Cast<string>().ToArray();

        public bool AllowDragDrop { get; set; } = true;

        private void DropEventHandler(object sender, DragEventArgs e)
        {
            if (!AllowDragDrop)
            {
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = GetAvailableFiles(e.Data.GetData(DataFormats.FileDrop) as string[]);
                if (files.Length == 0)
                {
                    return;
                }
                foreach (var file in files)
                {
                    if (!FileList.Contains(file))
                    {
                        FileList.Add(file);
                    }
                }

                FileDropped?.Invoke(this, new Common.StorageOperationEventArgs(files.ToArray(), Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.None));
            }
        }
        public delegate void FileDroppedHandler(object sender, Common.StorageOperationEventArgs e);
        public event FileDroppedHandler FileDropped;
        public string[] GetAvailableFiles(string[] files)
        {
            Regex r = new Regex(Filter, RegexOptions.Compiled);

            List<string> availableFiles = new List<string>();
            foreach (var file in files)
            {
                if (r.IsMatch(Filter))
                {
                    if (AcceptMode == FileAcceptMode.Files || AcceptMode == FileAcceptMode.FilesAndFilesInFolders)
                    {
                        if (File.Exists(file))
                        {
                            availableFiles.Add(file);
                        }
                    }
                    if (AcceptMode == FileAcceptMode.FilesAndFilesInFolders)
                    {
                        if (Directory.Exists(file))
                        {
                          availableFiles.AddRange(  IO.FileSystem.EnumerateAccessibleFiles(file));
                        }
                        }
                    else if (AcceptMode == FileAcceptMode.Folders || AcceptMode == FileAcceptMode.FilesAndFolders)
                    {
                        if (Directory.Exists(file))
                        {
                            availableFiles.Add(file);
                        }
                    }
                }
            }

            return availableFiles.ToArray();
        }

    }
    public enum FileAcceptMode
    {
        Files,
        Folders,
        FilesAndFilesInFolders,
        FilesAndFolders
    }
}
