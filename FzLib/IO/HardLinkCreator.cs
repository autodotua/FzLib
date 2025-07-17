using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace FzLib.IO
{
    public static class HardLinkCreator
    {
        // Windows API
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CreateHardLink(
            string lpFileName,
            string lpExistingFileName,
            IntPtr lpSecurityAttributes);

        // Unix API (Linux/macOS)
        [DllImport("libc", SetLastError = true)]
        private static extern int link(string oldpath, string newpath);

        public static void CreateHardLink(string linkPath, string sourcePath)
        {
            ValidatePaths(linkPath, sourcePath);

            if (OperatingSystem.IsWindows())
            {
                CreateWindowsHardLink(linkPath, sourcePath);
            }
            else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
            {
                CreateUnixHardLink(linkPath, sourcePath);
            }
            else
            {
                throw new PlatformNotSupportedException("Unsupported operating system");
            }
        }

        private static void CreateWindowsHardLink(string linkPath, string sourcePath)
        {
            if (!CreateHardLink(linkPath, sourcePath, IntPtr.Zero))
            {
                throw new IOException(
                    $"Failed to create hard link (0x{Marshal.GetLastWin32Error():X8})",
                    new Win32Exception());
            }
        }

        private static void CreateUnixHardLink(string linkPath, string sourcePath)
        {
            if (link(sourcePath, linkPath) != 0)
            {
                int errno = Marshal.GetLastWin32Error();
                throw new IOException(
                    $"Failed to create hard link (errno={errno}: {GetUnixErrorDescription(errno)})",
                    errno);
            }
        }

        private static void ValidatePaths(string linkPath, string sourcePath)
        {
            if (!File.Exists(sourcePath))
            {
                throw new FileNotFoundException("源文件不存在", sourcePath);
            }

            if (File.Exists(linkPath))
            {
                throw new IOException($"文件{linkPath}已存在");
            }

            // 仅在Windows上检查分区
            if (OperatingSystem.IsWindows() &&
                Path.GetPathRoot(linkPath) != Path.GetPathRoot(sourcePath))
            {
                throw new IOException("硬链接必须在同一分区");
            }
        }

        private static string GetUnixErrorDescription(int errno)
        {
            return errno switch
            {
                1 => "Operation not permitted (EPERM)",
                2 => "No such file or directory (ENOENT)",
                13 => "Permission denied (EACCES)",
                18 => "Cross-device link (EXDEV)",
                22 => "Invalid argument (EINVAL)",
                _ => $"Unknown error ({errno})"
            };
        }
    }
}