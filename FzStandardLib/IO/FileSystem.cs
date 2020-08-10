using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static FzLib.IO.FileSystemTree;
using SearchOption = System.IO.SearchOption;

namespace FzLib.IO
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

        public static IReadOnlyList<string> EnumerateAccessibleFiles(string path, string fileFilter = "*", string directoryFilter = "*", bool directoryFirst = false)
        {
            return EnumerateAccessibleFiles(path, fileFilter, directoryFilter, directoryFirst, out _, out _);
        }
        public static IReadOnlyList<string> EnumerateAccessibleFiles(string path, out IReadOnlyList<string> failedFiles, out IReadOnlyList<string> failedDirectories)
        {
            return EnumerateAccessibleFiles(path, "*", "*", false, out failedFiles, out failedDirectories);
        }

        public static IReadOnlyList<string>EnumerateAccessibleFiles(string path, string fileFilter, string directoryFilter, bool directoryFirst, out IReadOnlyList<string> failedFiles, out IReadOnlyList<string> failedDirectories)
        {
            List<string> files = new List<string>();
            var failedFilesList = new List<string>();
            var failedDirectoriesList = new List<string>();
            enumerateFolders(path);

            failedFiles = failedFilesList.AsReadOnly();
            failedDirectories = failedDirectoriesList.AsReadOnly();

            return files.AsReadOnly();

            void enumerateFiles(string directory)
            {
                try
                {
                    files.AddRange(Directory.EnumerateFiles(directory, fileFilter));
                }
                catch
                {
                    failedFilesList.Add(directory);
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
                    failedDirectoriesList.Add(directory);
                }
                if (directoryFirst)
                {
                    enumerateFiles(directory);
                }
            }
        }

        public static IReadOnlyList<FileSystemInfo> EnumerateAccessibleFileSystemInfos(string path, string fileFilter = "*", string directoryFilter = "*", bool directoryFirst = false)
        {
            return EnumerateAccessibleFileSystemInfos(path, fileFilter, directoryFilter, directoryFirst, out _, out _);
        }
        public static IReadOnlyList<FileSystemInfo> EnumerateAccessibleFileSystemInfos(string path, out IReadOnlyList<FileSystemInfo> failedFileSystemInfos, out IReadOnlyList<FileSystemInfo> failedDirectories)
        {
            return EnumerateAccessibleFileSystemInfos(path, "*", "*", false, out failedFileSystemInfos, out failedDirectories);
        }

        public static IReadOnlyList<FileSystemInfo> EnumerateAccessibleFileSystemInfos(string path, string fileFilter, string directoryFilter, bool directoryFirst, out IReadOnlyList<FileSystemInfo> failedFileSystemInfos, out IReadOnlyList<FileSystemInfo> failedDirectories)
        {
            List<FileSystemInfo> FileSystemInfos = new List<FileSystemInfo>();
            var failedFileSystemInfosList = new List<FileSystemInfo>();
            var failedDirectoriesList = new List<FileSystemInfo>();
            DirectoryInfo root = new DirectoryInfo(path);
            enumerateFolders(root);

            failedFileSystemInfos = failedFileSystemInfosList.AsReadOnly();
            failedDirectories = failedDirectoriesList.AsReadOnly();

            return FileSystemInfos.AsReadOnly();

            void enumerateFileSystemInfos(DirectoryInfo di)
            {
                try
                {
                    FileSystemInfos.AddRange(di.EnumerateFiles(fileFilter));
                }
                catch
                {
                    failedFileSystemInfosList.Add(di);
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
                    failedDirectoriesList.Add(di);
                }
                if (directoryFirst)
                {
                    enumerateFileSystemInfos(di);
                }
            }
        }


        public static long GetDirectoryLength(string path)
        {
            if (!Directory.Exists(path))
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

        public static FileComparisonResult CompareFiles(string leftPath, string rightPath)
        {
            FileSystemTree left = GetFileSystemTree(leftPath);
            FileSystemTree right = GetFileSystemTree(rightPath);
            return FileSystemTree.CompareFiles(left, right);
        }
        public static void Copy(this DirectoryInfo directory, string destinationPath)
        {
            CopyDirectory(directory.FullName, destinationPath);
        }
        public async static Task CopyAsync(this DirectoryInfo directory, string destinationPath)
        {
            await CopyDirectoryAsync(directory.FullName, destinationPath);
        }
        public async static Task CopyDirectoryAsync(string sourcePath, string destinationPath)
        {
            await Task.Run(() => CopyDirectory(sourcePath, destinationPath));
        }

        public static void CopyDirectory(string sourcePath, string destinationPath)
        {
            DirectoryInfo info = new DirectoryInfo(sourcePath);
            Directory.CreateDirectory(destinationPath);
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                string destName = Path.Combine(destinationPath, fsi.Name);

                if (fsi is FileInfo)          //如果是文件，复制文件
                {
                    File.Copy(fsi.FullName, destName);
                }
                else                                    //如果是文件夹，新建文件夹，递归
                {
                    Directory.CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName);
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


        public static string GetNoDuplicateFile(string path)
        {
            if (!File.Exists(path))
            {
                return path;
            }
            int i = 2;
            string directoryName = Path.GetDirectoryName(path);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            string fileExtension = Path.GetExtension(path);
            while (true)
            {
                string newName = Path.Combine(directoryName, $"{fileNameWithoutExtension} ({i.ToString()}){fileExtension}");
                if (!File.Exists(newName))
                {
                    return newName;
                }
                i++;
            }
        }

    }

}


