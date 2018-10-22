using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

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
            PreviewDragOver += DragEnterEventHandler;
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

        private void DragEnterEventHandler(object sender, DragEventArgs e)
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
        public bool AcceptFiles { get; set; } = true;
        public bool AcceptFolders { get; set; } = true;
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
                    if (AcceptFiles)
                    {
                        if (File.Exists(file))
                        {
                            availableFiles.Add(file);
                        }
                    }
                    if (AcceptFolders)
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
}
