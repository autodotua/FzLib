using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace FzLib.Wpf.Control.FileSystem
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
            if(!allowDragDrop)
            {
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files =GetAvailableFiles( e.Data.GetData(DataFormats.FileDrop) as string[]);
                
                if (!Multiple && files.Length > 1 || files.Length==0)
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
        private bool allowFile = true;
        private bool allowFolder = true;
        private bool check = true;
        private bool multiple = true;
        private string filter = ".*";
        private bool allowDragDrop = true;

        public bool AllowFile { get => allowFile; set => allowFile = value; }
        public bool AllowFolder { get => allowFolder; set => allowFolder = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool Check { get => check; set => check = value; }
        public bool Multiple { get => multiple; set => multiple = value; }
        public string Filter { get => filter; set => filter = value; }
        public bool AllowDragDrop { get => allowDragDrop; set => allowDragDrop = value; }

        public delegate void FileDroppedHandler(object sender,Common.StorageOperationEventArgs e);
        public event FileDroppedHandler FileDropped;

        private void TextBox_Drop(object sender, DragEventArgs e)
        {
            if(!allowDragDrop)
            {
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files =GetAvailableFiles( e.Data.GetData(DataFormats.FileDrop) as string[]);
                if (!Multiple && files.Length > 1 || files.Length==0)
                {
                    return;
                }
                if (files.Length == 1)
                {
                    Text = files[0];
                    return;
                }

                StringBuilder str = new StringBuilder();
                List<string> result = new List<string>();
                foreach (var i in files)
                {
                    if (check)
                    {
                        if (File.Exists(i))
                        {
                            if (AllowFile)
                            {
                                str.Append(i + "|");
                                result.Add(i);
                            }
                        }
                        else if (Directory.Exists(i))
                        {
                            if (AllowFolder)
                            {
                                str.Append(i + "|");
                                result.Add(i);
                            }
                        }
                    }
                    else
                    {
                        str.Append(i + "|");
                        result.Add(i);
                    }
                }
                Text = str.ToString();
                FileDropped?.Invoke(this,new Common.StorageOperationEventArgs(result.ToArray(),Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.None));
            }
        }

        public  string[] GetAvailableFiles(string[] files)
        {
            Regex r = new Regex(filter, RegexOptions.Compiled);

            List<string> availableFiles = new List<string>();
            foreach (var file in files)
            {
               if(r.IsMatch(filter))
                {
                    if(File.Exists(file))
                    {
                        availableFiles.Add(file);
                    }
                }
            }

            return availableFiles.ToArray() ;
        }
        
    }
}
