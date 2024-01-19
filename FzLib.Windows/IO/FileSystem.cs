using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static FzLib.IO.FileSystemTree;
using SearchOption = System.IO.SearchOption;

using vbFile = Microsoft.VisualBasic.FileIO;

namespace FzLib.IO
{
    public static class WindowsFileSystem
    {
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct WIN32_FIND_DATA
        {
            public uint dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll")]
        private static extern bool FindClose(IntPtr hFindFile);

        public static bool IsDirectoryEmpty(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(path);
            }

            if (Directory.Exists(path))
            {
                if (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    path += "*";
                }
                else
                {
                    path += Path.DirectorySeparatorChar + "*";
                }
                var findHandle = FindFirstFile(path, out WIN32_FIND_DATA findData);

                if (findHandle != INVALID_HANDLE_VALUE)
                {
                    try
                    {
                        bool empty = true;
                        do
                        {
                            if (findData.cFileName != "." && findData.cFileName != "..")
                            {
                                empty = false;
                            }
                        } while (empty && FindNextFile(findHandle, out findData));

                        return empty;
                    }
                    finally
                    {
                        FindClose(findHandle);
                    }
                }

                throw new Exception("读取目录中的第一个文件失败", Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()));
            }
            throw new DirectoryNotFoundException();
        }

        public static bool IsEmpty(this DirectoryInfo directory)
        {
            return IsDirectoryEmpty(directory.FullName);
        }

        [DllImport("Kernel32.dll")]
        private static extern uint GetShortPathName(string lpszLongPath, [Out] StringBuilder lpszShortPath, uint cchBuffer);

        // Return short path format of a file name
        public static string GetShortPathName(string longName)
        {
            StringBuilder s = new StringBuilder(1024);
            GetShortPathName(longName, s, (uint)s.Capacity);
            return s.ToString();
        }

        public static void DeleteFileOrFolder(string path, bool ui = false, bool toRecycleBin = true)
        {
            if (global::System.IO.File.Exists(path))
            {
                vbFile.FileSystem.DeleteFile(path,
                    ui ? UIOption.AllDialogs : UIOption.OnlyErrorDialogs,
                    toRecycleBin ? RecycleOption.SendToRecycleBin : RecycleOption.DeletePermanently);
            }
            else if (Directory.Exists(path))
            {
                vbFile.FileSystem.DeleteDirectory(path,
                     ui ? UIOption.AllDialogs : UIOption.OnlyErrorDialogs,
                     toRecycleBin ? RecycleOption.SendToRecycleBin : RecycleOption.DeletePermanently);
            }
        }

        public static void Delete(this FileInfo file, bool ui, bool toRecycleBin)
        {
            vbFile.FileSystem.DeleteFile(file.FullName,
                ui ? UIOption.AllDialogs : UIOption.OnlyErrorDialogs,
                toRecycleBin ? RecycleOption.SendToRecycleBin : RecycleOption.DeletePermanently);
        }

        public static void Delete(this DirectoryInfo file, bool ui, bool toRecycleBin)
        {
            vbFile.FileSystem.DeleteDirectory(file.FullName,
                ui ? UIOption.AllDialogs : UIOption.OnlyErrorDialogs,
                toRecycleBin ? RecycleOption.SendToRecycleBin : RecycleOption.DeletePermanently);
        }
    }
}