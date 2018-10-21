using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SearchOption = System.IO.SearchOption;
using vbFile = Microsoft.VisualBasic.FileIO;

namespace FzLib.Wpf.IO
{
    public static class FileSystem
    {
        public static bool FileExistsCaseSensitive(string filename)
        {
            return new FileInfo(filename).ExistsCaseSensitive();
        }

        public static bool ExistsCaseSensitive(this FileInfo file)
        {
            string fileFullName = file.FullName;

            string directory = Path.GetDirectoryName(file.FullName);


            return directory != null && Directory.EnumerateFiles(directory).Any(p => p == fileFullName);
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

        public static string[] EnumerateAccessibleFiles(string path, bool directoryFirst = false)
        {
            return EnumerateAccessibleFiles(path, directoryFirst, out string[] no);
        }

        public static string[] EnumerateAccessibleFiles(string path, bool directoryFirst, out string[] failedPath)
        {
            List<string> files = new List<string>();
            var failed = new List<string>();
            enumerateFolders(path);

            failedPath = failed.ToArray();
            return files.ToArray();

            void enumerateFiles(string directory)
            {
                try
                {
                    files.AddRange(Directory.EnumerateFiles(directory));
                }
                catch
                {
                    failed.Add(directory);
                }
            }

            void enumerateFolders(string directory)
            {
                enumerateFiles(directory);
                try
                {
                    foreach (var sub in Directory.EnumerateDirectories(directory))
                    {
                        enumerateFolders(sub);
                    }
                }
                catch
                {
                    failed.Add(directory);
                }
            }
        }

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

                throw new Exception("读取目录中的第一个文件失败",Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()));
            }
            throw new DirectoryNotFoundException();
        }
        public static bool IsEmpty(this DirectoryInfo directory)
        {
            return IsDirectoryEmpty(directory.FullName);
        }

        public static long GetDirectoryLength(string path)
        {
            if(!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(path);
            }
            return Directory.GetFiles(path, "*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t).Length));
        }

        public static long GetLength(this DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException(directory.FullName);
            }
            return Directory.GetFiles(directory.FullName, "*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t).Length));

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
    }

}


