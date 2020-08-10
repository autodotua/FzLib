using System.IO;
using sys = System;

namespace FzLib.IO
{
    public class FileSystemWatcher : sys.IO.FileSystemWatcher
    {
        public FileSystemWatcher(string path, bool includeSubdirectories) : base(path)
        {
            IncludeSubdirectories = includeSubdirectories;

        }

        public FileSystemWatcher() : base()
        {
        }

        public FileSystemWatcher(string path) : base(path)
        {
        }

        public FileSystemWatcher(string path, string filter) : base(path, filter)
        {
        }

        public void RegistAllEvent()
        {
            EnableRaisingEvents = true;
            Created += FileChanged;
            Renamed += FileChanged;
            Deleted += FileChanged;
            Changed += FileChanged;
        }

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            EveryChanged?.Invoke(sender, e);
        }

        public event FileSystemEventHandler EveryChanged;

        protected override void Dispose(bool disposing)
        {
            Created -= FileChanged;
            Renamed -= FileChanged;
            Deleted -= FileChanged;
            Changed -= FileChanged;

            base.Dispose(disposing);
        }
    }

}
