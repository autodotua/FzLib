using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static FzLib.IO.FileSystemTree;
using SearchOption = System.IO.SearchOption;
using static FzLib.IO.FileSystem;

namespace FzLib.IO
{
    public static class FileSystemInfoExtension
    {
        public static bool ExistsCaseSensitive(this FileInfo file)
        {
            string fileFullName = file.FullName;

            string directory = Path.GetDirectoryName(file.FullName);

            return directory != null && Directory.EnumerateFiles(directory).Any(p => p == fileFullName);
        }

        public static Task<long> GetLengthAsync(this DirectoryInfo directory)
        {
            return Task.Run(() => directory.GetLength());
        }

        public static long GetLength(this DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException(directory.FullName);
            }
            return directory.GetFiles("*", SearchOption.AllDirectories).Sum(p => p.Length);
        }

        public static FileInfo ToFileInfo(this string str)
        {
            return new FileInfo(str);
        }

        public static DirectoryInfo ToDirectoryInfo(this string str)
        {
            return new DirectoryInfo(str);
        }
    }

    public static class FileSystem
    {
        public static bool FileExistsCaseSensitive(string filename)
        {
            return new FileInfo(filename).ExistsCaseSensitive();
        }

        public static IReadOnlyList<string> EnumerateAccessibleFiles(string path, out IReadOnlyList<string> failedDirectories)
        {
            return EnumerateAccessibleFiles(path, "*", "*", false, out failedDirectories);
        }

        public static IReadOnlyList<string> EnumerateAccessibleFiles(string path, string fileFilter = "*", string directoryFilter = "*", bool directoryFirst = false)
        {
            return EnumerateAccessibleFiles(path, fileFilter, directoryFilter, directoryFirst, out _);
        }

        public static IReadOnlyList<string> EnumerateAccessibleFiles(string path, string fileFilter, string directoryFilter, bool directoryFirst, out IReadOnlyList<string> failedDirectories)
        {
            _ = path ?? throw new ArgumentNullException(nameof(path));
            if (path.EndsWith(":"))
            {
                path = path + "\\";
            }
            List<string> files = new List<string>();
            var faileds = new HashSet<string>();
            enumerateFolders(path);

            failedDirectories = faileds.ToList().AsReadOnly();

            return files.AsReadOnly();

            void enumerateFiles(string directory)
            {
                try
                {
                    files.AddRange(Directory.EnumerateFiles(directory, fileFilter));
                }
                catch
                {
                    faileds.Add(directory);
                }
            }

            void enumerateFolders(string directory)
            {
                if (!directoryFirst)
                {
                    enumerateFiles(directory);
                }
                try
                {
                    foreach (var sub in Directory.EnumerateDirectories(directory, directoryFilter))
                    {
                        enumerateFolders(sub);
                    }
                }
                catch
                {
                    faileds.Add(directory);
                }
                if (directoryFirst)
                {
                    enumerateFiles(directory);
                }
            }
        }

        public static IReadOnlyList<string> EnumerateAccessibleDirectories(string path, string filter = "*")
        {
            return EnumerateAccessibleDirectories(path, filter, out _);
        }

        public static IReadOnlyList<string> EnumerateAccessibleDirectories(string path, string filter, out IReadOnlyList<string> failedDirectories)
        {
            if (path.EndsWith(":"))
            {
                path = path + "\\";
            }
            List<string> files = new List<string>();
            var failedDirectoriesList = new List<string>();
            enumerateFolders(path);

            failedDirectories = failedDirectoriesList.AsReadOnly();

            return files.AsReadOnly();

            void enumerateFolders(string directory)
            {
                try
                {
                    foreach (var sub in Directory.EnumerateDirectories(directory, filter))
                    {
                        files.Add(sub);
                    }
                    foreach (var sub in Directory.EnumerateDirectories(directory))
                    {
                        enumerateFolders(sub);
                    }
                }
                catch
                {
                    failedDirectoriesList.Add(directory);
                }
            }
        }

        public static IReadOnlyList<FileSystemInfo> EnumerateAccessibleFileSystemInfos(string path, string fileFilter = "*", string directoryFilter = "*", bool directoryFirst = false)
        {
            return EnumerateAccessibleFileSystemInfos(path, fileFilter, directoryFilter, directoryFirst, out _);
        }

        public static IReadOnlyList<FileSystemInfo> EnumerateAccessibleFileSystemInfos(string path, out IReadOnlyList<FileSystemInfo> failedDirectories)
        {
            return EnumerateAccessibleFileSystemInfos(path, "*", "*", false, out failedDirectories);
        }

        public static IReadOnlyList<FileSystemInfo> EnumerateAccessibleFileSystemInfos(string path, string fileFilter, string directoryFilter, bool directoryFirst, out IReadOnlyList<FileSystemInfo> failedDirectories)
        {
            List<FileSystemInfo> FileSystemInfos = new List<FileSystemInfo>();
            var faileds = new HashSet<FileSystemInfo>();
            DirectoryInfo root = new DirectoryInfo(path);
            enumerateFolders(root);

            failedDirectories = faileds.ToList().AsReadOnly();

            return FileSystemInfos.AsReadOnly();

            void enumerateFileSystemInfos(DirectoryInfo di)
            {
                try
                {
                    FileSystemInfos.AddRange(di.EnumerateFiles(fileFilter));
                }
                catch
                {
                    faileds.Add(di);
                }
            }

            void enumerateFolders(DirectoryInfo di)
            {
                if (!directoryFirst)
                {
                    enumerateFileSystemInfos(di);
                }
                if (root != di)
                {
                    FileSystemInfos.Add(di);
                }
                try
                {
                    foreach (var sub in di.EnumerateDirectories(directoryFilter))
                    {
                        enumerateFolders(sub);
                    }
                }
                catch
                {
                    faileds.Add(di);
                }
                if (directoryFirst)
                {
                    enumerateFileSystemInfos(di);
                }
            }
        }

        public static Task<long> GetDirectoryLengthAsync(string path)
        {
            return Task.Run(() => GetDirectoryLength(path));
        }

        public static long GetDirectoryLength(string path)
        {
            _ = path ?? throw new ArgumentNullException(nameof(path));
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(path);
            }
            return Directory.GetFiles(path, "*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t).Length));
        }

        public static FileComparisonResult CompareFiles(string leftPath, string rightPath)
        {
            FileSystemTree left = GetFileSystemTree(leftPath);
            FileSystemTree right = GetFileSystemTree(rightPath);
            return FileSystemTree.CompareFiles(left, right);
        }

        public static async Task<FileComparisonResult> CompareFilesAsync(string leftPath, string rightPath)
        {
            FileSystemTree left = await GetFileSystemTreeAsync(leftPath);
            FileSystemTree right = await GetFileSystemTreeAsync(rightPath);
            return await FileSystemTree.CompareFilesAsync(left, right);
        }

        public async static Task CopyDirectoryAsync(string sourcePath, string destinationPath, EventHandler<FileCopyEventArgs> e = null)
        {
            await Task.Run(() => CopyDirectory(sourcePath, destinationPath, e));
        }

        public static void CopyDirectory(string sourcePath, string destinationPath, EventHandler<FileCopyEventArgs> e = null)
        {
            _ = sourcePath ?? throw new ArgumentNullException(nameof(sourcePath));
            _ = destinationPath ?? throw new ArgumentNullException(nameof(destinationPath));
            DirectoryInfo info = new DirectoryInfo(sourcePath);
            Directory.CreateDirectory(destinationPath);
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                string destName = Path.Combine(destinationPath, fsi.Name);

                if (fsi is FileInfo)          //如果是文件，复制文件
                {
                    File.Copy(fsi.FullName, destName);
                    e?.Invoke(null, new FileCopyEventArgs(fsi.FullName, destName));
                }
                else                                    //如果是文件夹，新建文件夹，递归
                {
                    Directory.CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName, e);
                }
            }
        }

        ///// <summary>
        ///// 删除文件夹（及文件夹下所有子文件夹和文件）
        ///// </summary>
        ///// <param name="directoryPath"></param>
        //public static void DeleteDirectory(string directoryPath, bool forceDeleteReadOnly)
        //{
        //    if(Directory.Exists(directoryPath))
        //    {
        //        throw new DirectoryNotFoundException();
        //    }
        //    foreach (string d in Directory.GetFileSystemEntries(directoryPath))
        //    {
        //        if (File.Exists(d))
        //        {
        //            FileInfo file = new FileInfo(d);
        //            if (file.Attributes.HasFlag(FileAttributes.ReadOnly))
        //            {
        //                if (forceDeleteReadOnly)
        //                {
        //                    file.Attributes = FileAttributes.Normal;
        //                }
        //                else
        //                {
        //                    throw new IOException("文件" + file.FullName + "是只读的");
        //                }
        //            }
        //            File.Delete(d);     //删除文件
        //        }
        //        else
        //        {
        //            DeleteDirectory(d, forceDeleteReadOnly);    //删除文件夹
        //        }
        //    }
        //    Directory.Delete(directoryPath);    //删除空文件夹
        //}

        ///// <summary>
        ///// 删除文件夹（及文件夹下所有子文件夹和文件）
        ///// </summary>
        ///// <param name="directoryPath"></param>
        //public async static Task DeleteDirectoryAsync(string directoryPath, bool forceDeleteReadOnly)
        //{
        //    await Task.Run(() => DeleteDirectory(directoryPath, forceDeleteReadOnly));
        //}

        public static string GetNoDuplicateFile(string path, string suffixFormat = " ({i})")
        {
            if (!File.Exists(path))
            {
                return path;
            }
            if (!suffixFormat.Contains("{i}"))
            {
                throw new ArgumentException("后缀应包含“{i}”以表示索引");
            }
            int i = 2;
            string directoryName = Path.GetDirectoryName(path);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            string fileExtension = Path.GetExtension(path);
            while (true)
            {
                string newName = Path.Combine(directoryName, $"{fileNameWithoutExtension}{suffixFormat.Replace("{i}", i.ToString())}{fileExtension}");
                if (!File.Exists(newName))
                {
                    return newName;
                }
                i++;
            }
        }

        public static void OpenFileOrFolder(string path)
        {
            new Process()
            {
                StartInfo = new ProcessStartInfo(Path.GetFullPath(path))
                {
                    UseShellExecute = true
                }
            }.Start();
        }

        public static void OpenFileOrFolder(string useProgram, string path)
        {
            new Process()
            {
                StartInfo = new ProcessStartInfo(useProgram, Path.GetFullPath(path))
                {
                    UseShellExecute = true
                }
            }.Start();
        }
    }

    public class FileCopyEventArgs : EventArgs
    {
        public FileCopyEventArgs(string pathFrom, string pathTo)
        {
            PathFrom = pathFrom ?? throw new ArgumentNullException(nameof(pathFrom));
            PathTo = pathTo ?? throw new ArgumentNullException(nameof(pathTo));
        }

        public string PathFrom { get; set; }
        public string PathTo { get; set; }
    }
}