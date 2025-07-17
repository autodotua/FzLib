using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FzLib.IO;

public static class FileDeleteHelper
{
    private static readonly IFileDeleter deleter;
    private static readonly Dictionary<string, bool> commandCache = new();
    private static bool? linuxAvailable;
    private static bool? macOSAvailable;

    static FileDeleteHelper()
    {
        if (OperatingSystem.IsWindows())
        {
            deleter = new WindowsFileDeleter();
        }
        else if (OperatingSystem.IsLinux())
        {
            deleter = new LinuxFileDeleter();
        }
        else if (OperatingSystem.IsMacOS())
        {
            deleter = new MacOSFileDeleter();
        }
        else
        {
            throw new PlatformNotSupportedException("Unsupported operating system");
        }
    }

    public static void ClearCache()
    {
        linuxAvailable = null;
        macOSAvailable = null;
        commandCache.Clear();
    }

    public static void DeleteToRecycleBin(string path)
    {
        deleter.DeleteToRecycleBin(path);
    }

    public static void DirectlyDelete(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        else
        {
            throw new FileNotFoundException("指定的文件或目录不存在", path);
        }
    }

    private static bool CommandExists(string command)
    {
        if (commandCache.TryGetValue(command, out bool exists))
        {
            return exists;
        }

        string[] paths = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator) ?? Array.Empty<string>();
        exists = paths.Any(p => File.Exists(Path.Combine(p, command)));
        commandCache[command] = exists;
        return exists;
    }


    internal interface IFileDeleter
    {
        void DeleteToRecycleBin(string path);
    }

    internal class WindowsFileDeleter : IFileDeleter
    {
        [Flags]
        public enum FileOperationFlags : ushort
        {
            FOF_ALLOWUNDO = 0x0040,
            FOF_NOCONFIRMATION = 0x0010,
            FOF_NOERRORUI = 0x0400,
            FOF_SILENT = 0x0004
        }

        public enum FileOperationType : uint
        {
            FO_DELETE = 0x0003
        }

        public void DeleteToRecycleBin(string path)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                throw new FileNotFoundException("指定的文件或目录不存在", path);
            }

            try
            {
                var fileOp = new SHFILEOPSTRUCT
                {
                    wFunc = FileOperationType.FO_DELETE,
                    pFrom = path + '\0' + '\0',
                    fFlags = FileOperationFlags.FOF_ALLOWUNDO |
                             FileOperationFlags.FOF_NOCONFIRMATION |
                             FileOperationFlags.FOF_NOERRORUI |
                             FileOperationFlags.FOF_SILENT,
                    hwnd = IntPtr.Zero
                };

                int result = SHFileOperation(ref fileOp);

                if (result != 0)
                {
                    string errorMsg = result switch
                    {
                        0x2 => "系统找不到指定的文件",
                        0x3 => "系统找不到指定的路径",
                        0x5 => "拒绝访问",
                        0x71 => "目标路径无效",
                        _ => $"未知错误 (0x{result:X})"
                    };
                    throw new IOException($"删除到回收站失败: {errorMsg}");
                }
            }
            catch (Exception ex)
            {
                throw new IOException("删除到回收站时发生错误", ex);
            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public FileOperationType wFunc;
            public string pFrom;
            public string pTo;
            public FileOperationFlags fFlags;

            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;

            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }
    }

    internal class LinuxFileDeleter : IFileDeleter
    {
        public void DeleteToRecycleBin(string path)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                throw new FileNotFoundException("指定的文件或目录不存在", path);
            }

            if (!IsRecycleBinAvailable())
            {
                throw new InvalidOperationException("回收站不可用");
            }

            string command = GetTrashCommand();
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = $"trash \"{path}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new IOException($"Linux删除到回收站失败，退出代码: {process.ExitCode}");
            }
        }

        private bool IsRecycleBinAvailable()
        {
            if (linuxAvailable.HasValue)
            {
                return linuxAvailable.Value;
            }

            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string trashPath = Path.Combine(home, ".local/share/Trash");
            if (Directory.Exists(Path.Combine(trashPath, "files")) &&
                Directory.Exists(Path.Combine(trashPath, "info")))
            {
                linuxAvailable = true;
                return true;
            }

            if (File.Exists("/usr/bin/trash-put") ||
                File.Exists("/usr/bin/gio"))
            {
                linuxAvailable = true;
                return true;
            }

            linuxAvailable = false;
            return false;
        }

        private string GetTrashCommand()
        {
            if (CommandExists("gio"))
            {
                return "gio";
            }
            else if (CommandExists("trash-put"))
            {
                return "trash-put";
            }

            throw new InvalidOperationException("请先安装gio或trash-cli工具: sudo apt install trash-cli");
        }
    }

    internal class MacOSFileDeleter : IFileDeleter
    {
        public void DeleteToRecycleBin(string path)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                throw new FileNotFoundException("指定的文件或目录不存在", path);
            }

            if (!IsRecycleBinAvailable())
            {
                throw new InvalidOperationException("回收站不可用");
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/usr/bin/osascript",
                    Arguments = $"-e 'tell application \"Finder\" to delete POSIX file \"{path}\"'",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new IOException($"macOS删除到回收站失败，退出代码: {process.ExitCode}");
            }
        }

        private bool IsRecycleBinAvailable()
        {
            if (macOSAvailable.HasValue)
            {
                return macOSAvailable.Value;
            }

            if (!File.Exists("/usr/bin/osascript"))
            {
                macOSAvailable = false;
                return false;
            }

            macOSAvailable = true;
            return true;
        }
    }
}