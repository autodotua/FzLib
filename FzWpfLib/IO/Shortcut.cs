using IWshRuntimeLibrary;
using System;
using System.Diagnostics;
using System.IO;

namespace FzLib.IO
{
    public static class Shortcut
    {
        public static void CreateShortcut(ShortcutInfo info)
        {
            WshShell shell = new WshShell();
            IWshShortcut sc = (IWshShortcut)shell.CreateShortcut(info.ShortcutFilePath);
            sc.TargetPath = info.TargetPath;
            if (!string.IsNullOrWhiteSpace(info.Arguments))
            {
                sc.Arguments = info.Arguments;
            }
            if (!string.IsNullOrWhiteSpace(info.WorkingDirectory))
            {
                sc.WorkingDirectory = Environment.CurrentDirectory;
            }
            if(!string.IsNullOrWhiteSpace(info.IconLocation))
            {
                sc.IconLocation = info.IconLocation;
            }
            if(!string.IsNullOrWhiteSpace(info.Description))
            {
                sc.Description = info.Description;
            }
            sc.WindowStyle = info.WindowStyle;
            sc.Save();
        }

        public static ShortcutStatus GetShortcutStatus(string shortcutFilePath,string targetPath)
        {
            if(!System.IO.File.Exists(shortcutFilePath))
            {
                return ShortcutStatus.NotExist;
            }

            WshShell shell = new WshShell();
            IWshShortcut sc = (IWshShortcut)shell.CreateShortcut(shortcutFilePath);
            if(sc.TargetPath==targetPath)
            {
                return ShortcutStatus.Exist;
            }
            return ShortcutStatus.NotMatch;
        }

        public enum ShortcutStatus
        {
            NotExist,
            Exist,
            NotMatch,
        }

        public class ShortcutInfo
        {
            public string ShortcutFilePath { get; set; }
            public string TargetPath { get; set; }
            public string Arguments { get; set; }
            public string WorkingDirectory { get; set; }
            public string IconLocation { get; set; }
            public string Description { get; set; }
            public int WindowStyle { get; set; } = 1;

            public void SetWorkingDirectoryToProgramDirectory()
            {
                WorkingDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            }

            public void SetTargetPathToProgramFile()
            {
                TargetPath = Process.GetCurrentProcess().MainModule.FileName;
            }

        }
    }
}
