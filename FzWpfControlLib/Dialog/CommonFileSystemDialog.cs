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
    }
}
