using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FzLib.IO
{
    public static class FileCopyHelper
    {
        /// <summary>
        /// 高性能文件复制（双缓冲流水线）
        /// </summary>
        public static async Task CopyFileAsync(
            string sourceFilePath,
            string destinationFilePath,
            int bufferSize = 0,
                        IProgress<FileProcessProgress> progress = null,
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

                // 启动并行任务
                var readTask =
                    FileIOHelper.ReadDataAsync(sourceStream, bufferChannel.Writer, bufferSize, cancellationToken);
                var writeTask = WriteDataAsync(destinationStream, bufferChannel.Reader, progress, sourceFilePath,
                    destinationFilePath, totalBytes, cancellationToken);

                await Task.WhenAll(readTask, writeTask);

                // 复制文件属性
                var sourceInfo = new FileInfo(sourceFilePath);
                File.SetLastWriteTimeUtc(destinationFilePath, sourceInfo.LastWriteTimeUtc);
                try
                {
                    File.SetCreationTimeUtc(destinationFilePath, sourceInfo.CreationTimeUtc);
                }
                catch
                {
                    // ignored
                }
            }
            catch (OperationCanceledException)
            {
                if (File.Exists(destinationFilePath))
                {
                    File.Delete(destinationFilePath);
                }
                throw;
            }
        }


        private static async Task WriteDataAsync(
            FileStream destinationStream,
            ChannelReader<(byte[] buffer, int bytesRead)> reader,
            IProgress<FileProcessProgress> progress, // 新增 progress 参数
            string sourceFilePath, // 新增 sourceFilePath
            string destinationFilePath, // 新增 destinationFilePath
            long totalBytes, // 新增 totalBytes
            CancellationToken ct)
        {
            long totalBytesWritten = 0;

            await foreach (var (buffer, bytesRead) in reader.ReadAllAsync(ct))
            {
                await destinationStream.WriteAsync(buffer.AsMemory(0, bytesRead), ct);
                totalBytesWritten += bytesRead;

                // 报告进度（基于已写入的字节数）
                progress?.Report(new FileProcessProgress
                {
                    SourceFilePath = sourceFilePath,
                    DestinationFilePath = destinationFilePath,
                    TotalBytes = totalBytes,
                    BytesCopied = totalBytesWritten
                });

                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }
}