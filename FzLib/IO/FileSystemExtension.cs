using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SearchOption = System.IO.SearchOption;
using System.Threading;

namespace FzLib.IO
{
    public static class FileSystemExtension
    {
        public static IEnumerable<DirectoryInfo> EnumerateDirectories(
            this DirectoryInfo directory,
            FileFilterRule filter = null,
            bool skipRecycleBin = true,
            CancellationToken cancellationToken = default)
        {
            var options = directory.GetEnumerationOptions();
            return directory.EnumerateDirectories("*", options)
                .ApplyFilter(cancellationToken, filter, skipRecycleBin);
        }

        public static IEnumerable<FileInfo> EnumerateFiles(
            this DirectoryInfo directory,
            FileFilterRule filter = null,
            bool skipRecycleBin = true,
            CancellationToken cancellationToken = default)
        {
            var options = directory.GetEnumerationOptions();
            return directory.EnumerateFiles("*", options)
                .ApplyFilter(cancellationToken, filter, skipRecycleBin);
        }

        public static IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(
            this DirectoryInfo directory,
            FileFilterRule filter = null,
            bool skipRecycleBin = true,
            CancellationToken cancellationToken = default)
        {
            var options = directory.GetEnumerationOptions();
            return directory.EnumerateFileSystemInfos("*", options)
                .ApplyFilter(cancellationToken, filter, skipRecycleBin);
        }

        public static EnumerationOptions GetEnumerationOptions(this DirectoryInfo directory,
                                    bool includingSubDirs = true,
            MatchCasing matchCasing = MatchCasing.PlatformDefault)
        {
            return new EnumerationOptions()
            {
                IgnoreInaccessible = true,
                AttributesToSkip = 0,
                RecurseSubdirectories = includingSubDirs,
                MatchCasing = matchCasing
            };
        }

        public static long GetLength(this DirectoryInfo directory, CancellationToken cancellationToken = default)
        {
            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException(directory.FullName);
            }
            long length = 0;
            foreach (var file in directory.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                cancellationToken.ThrowIfCancellationRequested();
                length += file.Length;
            }
            return length;
        }

        public static Task<long> GetLengthAsync(this DirectoryInfo directory, CancellationToken cancellationToken)
        {
            return Task.Run(() => directory.GetLength(cancellationToken));
        }

        public static void OpenWithDefault(this FileSystemInfo file)
        {
            if (!file.Exists)
            {
                throw new FileNotFoundException("文件不存在");
            }
            Process.Start(new ProcessStartInfo(file.FullName) { UseShellExecute = true });
        }

        private static IEnumerable<T> ApplyFilter<T>(this IEnumerable<T> source,
                                    CancellationToken cancellationToken, FileFilterRule filter = null, bool skipRecycleBin = true)
        {
            var filterHelper = filter == null ? null : new FileFilterHelper(filter);
            foreach (var item in source)
            {
                cancellationToken.ThrowIfCancellationRequested();
                bool canYieldReturn = true;
                if (skipRecycleBin)
                {
                    string path = item switch
                    {
                        string str => str,
                        FileSystemInfo fi => fi.FullName,
                        _ => throw new NotSupportedException($"不支持ApplyFilter的类型：{typeof(T).Name}")
                    };

                    if (path != null && path.Contains("$RECYCLE.BIN", StringComparison.OrdinalIgnoreCase))
                    {
                        canYieldReturn = false;
                    }
                }

                if (filter != null && canYieldReturn)
                {
                    canYieldReturn = item switch
                    {
                        string str => filterHelper.IsMatched(str),
                        FileSystemInfo fi => filterHelper.IsMatched(fi),
                        _ => throw new NotSupportedException($"不支持ApplyFilter的类型：{typeof(T).Name}")
                    };
                }

                if (canYieldReturn)
                {
                    yield return item;
                }
            }
        }
    }
}