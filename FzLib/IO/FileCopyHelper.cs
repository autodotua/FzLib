using System;
using System.Buffers;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FzLib.IO
{
    public static class FileCopyHelper
    {
        public static async Task CopyDirectoryAsync(
            string sourceDirPath,
            string destinationDirPath,
            int bufferSize = 0,
            EnumerationOptions enumerationOptions = null,
            IProgress<DirectoryProcessProgress> progress = null,
            CancellationToken cancellationToken = default)
        {
            enumerationOptions ??= new EnumerationOptions()
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = true
            };

            if (!Directory.Exists(sourceDirPath))
            {
                throw new DirectoryNotFoundException($"源目录不存在: {sourceDirPath}");
            }
            if (!Directory.Exists(destinationDirPath))
            {
                Directory.CreateDirectory(destinationDirPath);
            }
            var sourceDirInfo = new DirectoryInfo(sourceDirPath);
            List<FileInfo> files = new List<FileInfo>();

            long totalBytes = 0;
            long processedBytes = 0;
            await Task.Run(() =>
            {
                foreach (var file in sourceDirInfo.EnumerateFiles("*", enumerationOptions))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    files.Add(file);
                    totalBytes += file.Length;
                }
            }, cancellationToken);
            Progress<FileProcessProgress> fileProgress = null;
            if (progress != null)
            {
                fileProgress = new Progress<FileProcessProgress>(p =>
                {
                    progress.Report(new DirectoryProcessProgress
                    {
                        SourceDirPath = sourceDirPath,
                        DestinationDirPath = destinationDirPath,
                        SourceFilePath = p.SourceFilePath,
                        DestinationFilePath = p.DestinationFilePath,
                        TotalBytes = totalBytes,
                        ProcessedBytes = processedBytes + p.ProcessedBytes,
                        FileTotalBytes = p.TotalBytes,
                        FileProcessedBytes = p.ProcessedBytes
                    });
                });
            }
            foreach (FileInfo fileInfo in files)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var sourceFilePath = fileInfo.FullName;
                var relativePath = Path.GetRelativePath(sourceDirPath, sourceFilePath);
                var destinationFilePath = Path.Combine(destinationDirPath, relativePath);
                await CopyFileAsync(sourceFilePath, destinationFilePath, bufferSize, fileProgress, null, cancellationToken);
                processedBytes += fileInfo.Length;
            }
            if (progress != null)
            {
                progress.Report(new DirectoryProcessProgress
                {
                    SourceDirPath = sourceDirPath,
                    DestinationDirPath = destinationDirPath,
                    TotalBytes = totalBytes,
                    ProcessedBytes = processedBytes,
                });
            }
        }

        /// <summary>
        /// 高性能文件复制（双缓冲流水线）
        /// </summary>
        public static async Task<byte[]> CopyFileAsync(
            string sourceFilePath,
            string destinationFilePath,
            int bufferSize = 0,
            IProgress<FileProcessProgress> progress = null,
            HashAlgorithmType? hashAlgorithmType = null,
            CancellationToken cancellationToken = default)
        {
            if (!File.Exists(sourceFilePath))
            {
                throw new FileNotFoundException("源文件不存在", sourceFilePath);
            }

            // 确保目标目录存在
            string directory = Path.GetDirectoryName(destinationFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // 动态调整缓冲区
            if (bufferSize <= 0)
            {
                var fileInfo = new FileInfo(sourceFilePath);
                bufferSize = FileIOHelper.GetOptimalBufferSize(fileInfo.Length);
            }

            // 创建双缓冲通道（容量=2）
            var bufferChannel = Channel.CreateBounded<(byte[] buffer, int bytesRead)>(
                new BoundedChannelOptions(2)
                {
                    SingleWriter = true,
                    SingleReader = true,
                    FullMode = BoundedChannelFullMode.Wait
                });

            HashAlgorithm hasher = null;
            if (hashAlgorithmType.HasValue)
            {
                hasher = FileHashHelper.CreateHashAlgorithm(hashAlgorithmType.Value);
            }

            try
            {
                await using var sourceStream = new FileStream(
                    sourceFilePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    bufferSize,
                    FileOptions.Asynchronous | FileOptions.SequentialScan);

                await using var destinationStream = new FileStream(
                    destinationFilePath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize,
                    FileOptions.Asynchronous | FileOptions.WriteThrough);

                long totalBytes = sourceStream.Length;

                var readTask = FileIOHelper.ReadDataAsync(sourceStream, bufferChannel.Writer, bufferSize, cancellationToken);
                var writeTask = WriteDataAsync(destinationStream, bufferChannel.Reader, progress,
                    sourceFilePath, destinationFilePath, totalBytes, hasher, cancellationToken);

                await Task.WhenAll(readTask, writeTask);

                // 复制文件属性
                var sourceInfo = new FileInfo(sourceFilePath);
                File.SetLastWriteTimeUtc(destinationFilePath, sourceInfo.LastWriteTimeUtc);
                try
                {
                    File.SetCreationTimeUtc(destinationFilePath, sourceInfo.CreationTimeUtc);
                }
                catch { }

                return hasher?.Hash;
            }
            catch (OperationCanceledException)
            {
                if (File.Exists(destinationFilePath))
                {
                    File.Delete(destinationFilePath);
                }
                throw;
            }
            finally
            {
                hasher?.Dispose();
            }
        }

        private static async Task WriteDataAsync(
            FileStream destinationStream,
            ChannelReader<(byte[] buffer, int bytesRead)> reader,
            IProgress<FileProcessProgress> progress,
            string sourceFilePath,
            string destinationFilePath,
            long totalBytes,
            HashAlgorithm hasher = null,
            CancellationToken ct = default)
        {
            long totalBytesWritten = 0;

            await foreach (var (buffer, bytesRead) in reader.ReadAllAsync(ct))
            {
                await destinationStream.WriteAsync(buffer.AsMemory(0, bytesRead), ct);

                // 如果需要计算哈希，则在写入时顺便更新
                hasher?.TransformBlock(buffer, 0, bytesRead, null, 0);

                totalBytesWritten += bytesRead;
                progress?.Report(new FileProcessProgress
                {
                    SourceFilePath = sourceFilePath,
                    DestinationFilePath = destinationFilePath,
                    TotalBytes = totalBytes,
                    ProcessedBytes = totalBytesWritten
                });

                ArrayPool<byte>.Shared.Return(buffer);
            }

            hasher?.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        }
    }
}