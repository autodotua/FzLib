using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace FzLib.UI.FileSystem
{
    public class FileDropTextBox : FlatStyle.TextBox
    {
        public FileDropTextBox()
        {
            AllowDrop = true;
            PreviewDragOver += TextBox_DragEnter;
            PreviewDrop += TextBox_Drop;
        }



        private void TextBox_DragEnter(object sender, DragEventArgs e)
        {
            if (!AllowDragDrop)
            {
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = GetAvailableFiles(e.Data.GetData(DataFormats.FileDrop) as string[]);

                if (!Multiple && files.Length > 1 || files.Length == 0)
                {
                    e.Effects = DragDropEffects.None;
                }
                else
                {
                    e.Effects = DragDropEffects.Copy;
                }
                e.Handled = true;
            }
        }

        public bool AllowFolder { get; set; } = true;
        /// <summary>
        /// 
        /// </summary>
        public bool Check { get; set; } = true;
        public bool Multiple { get; set; } = true;
        public string Filter { get; set; } = ".*";
        public bool AllowDragDrop { get; set; } = true;
        public bool AllowFile { get; set; } = true;
        public string Splitter { get; set; } = "|";

        public delegate void FileDroppedHandler(object sender, Common.StorageOperationEventArgs e);
        public event FileDroppedHandler FileDropped;

        private void TextBox_Drop(object sender, DragEventArgs e)
        {
            if (!AllowDragDrop)
            {
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = GetAvailableFiles(e.Data.GetData(DataFormats.FileDrop) as string[]);
                if (!Multiple && files.Length > 1 || files.Length == 0)
                {
                    return;
                }
                if (files.Length == 1)
                {
                    Text = files[0];
                    FileDropped?.Invoke(this, new Common.StorageOperationEventArgs(files[0]));

                    return;
                }

                List<string> result = new List<string>();
                foreach (var i in files)
                {
                    if (Check)
                    {
                        if (File.Exists(i))
                        {
                            if (AllowFile)
                            {
                                result.Add(i);
                            }
                        }
                        else if (Directory.Exists(i))
                        {
                            if (AllowFolder)
                            {
                                result.Add(i);
                            }
                        }
                    }
                    else
                    {
                        result.Add(i);
                    }
                }
                Text = string.Join(Splitter, result);
                FileDropped?.Invoke(this, new Common.StorageOperationEventArgs(result.ToArray()));
            }
        }

        public void SetFiles(params string[] files)
        {
            Text = string.Join(Splitter, files);
        }

        public string[] GetAvailableFiles(string[] files)
        {
            Regex r = new Regex(Filter, RegexOptions.Compiled);

            List<string> availableFiles = new List<string>();
            foreach (var file in files)
            {
                if (r.IsMatch(Filter))
                {
                    if (AllowFile)
                    {
                        if (File.Exists(file))
                        {
                            availableFiles.Add(file);
                        }
                    }
                    if(AllowFolder)
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
