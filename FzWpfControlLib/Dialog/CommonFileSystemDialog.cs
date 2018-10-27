using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Control.Dialog
{
   public static class CommonFileSystemDialog
    {
        public static CommonOpenFileDialog OpenDialog => new CommonOpenFileDialog();
        public static CommonSaveFileDialog SaveDialog => new CommonSaveFileDialog();

        public static string GetSaveFile(IList<(string display,string extension)> filters=null,bool allExtensions=false,bool ensureExtension=false,string defaultFileName="")
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
            if(allExtensions)
            {
                dialog.Filters.Add(new CommonFileDialogFilter("所有文件", "*"));
            }
           
            if(dialog.ShowDialog()==CommonFileDialogResult.Ok)
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
    }
}
