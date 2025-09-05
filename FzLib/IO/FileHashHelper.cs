using System.Buffers;
using System.Security.Cryptography;
using System.Threading.Channels;

namespace FzLib.IO;

public static class FileHashHelper
{
    /// <summary>
    /// 高性能哈希计算（双缓冲流水线）
    /// </summary>
    public static async Task<byte[]> ComputeHashAsync(
        string filePath,
        HashAlgorithmType algorithmType = HashAlgorithmType.SHA1,
        int bufferSize = 0,
        IProgress<FileProcessProgress> progress = null,
        CancellationToken cancellationToken = default)
    {
        if (bufferSize <= 0)
        {
            var fileInfo = new FileInfo(filePath);
            bufferSize = FileIOHelper.GetOptimalBufferSize(fileInfo.Length);
        }

        using var hashAlgorithm = CreateHashAlgorithm(algorithmType);
        await using var stream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize,
            FileOptions.Asynchronous | FileOptions.SequentialScan);

        var bufferChannel = Channel.CreateBounded<(byte[] buffer, int bytesRead)>(2);

        var readTask = FileIOHelper.ReadDataAsync(stream, bufferChannel.Writer, bufferSize, cancellationToken);
        var computeTask = ComputeHashAsync(hashAlgorithm, bufferChannel.Reader, progress, filePath, stream.Length,
            cancellationToken);

        await Task.WhenAll(readTask, computeTask);
        return hashAlgorithm.Hash;
    }

    public static async Task<string> ComputeHashStringAsync(
            string filePath,
        HashAlgorithmType algorithmType = HashAlgorithmType.SHA1,
        int bufferSize = 0,
        IProgress<FileProcessProgress> progress = null,
        CancellationToken cancellationToken = default)
    {

        return Convert.ToHexString(await ComputeHashAsync(filePath, algorithmType, bufferSize, progress, cancellationToken));
    }
    public static bool IsValidHashString(string hash, HashAlgorithmType type)
    {
        if (string.IsNullOrEmpty(hash))
            return false;

        int expectedLength = type switch
        {
            HashAlgorithmType.MD5 => 32,
            HashAlgorithmType.SHA1 => 40,
            HashAlgorithmType.SHA256 => 64,
            HashAlgorithmType.SHA384 => 96,
            HashAlgorithmType.SHA512 => 128,
            _ => 0
        };

        if (hash.Length != expectedLength)
            return false;

        foreach (char c in hash)
        {
            bool isHexDigit = c is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F';
            if (!isHexDigit)
            {
                return false;
            }
        }

        return true;
    }

    internal static HashAlgorithm CreateHashAlgorithm(HashAlgorithmType algorithmType)
    {
        return algorithmType switch
        {
            HashAlgorithmType.MD5 => MD5.Create(),
            HashAlgorithmType.SHA1 => SHA1.Create(),
            HashAlgorithmType.SHA256 => SHA256.Create(),
            HashAlgorithmType.SHA384 => SHA384.Create(),
            HashAlgorithmType.SHA512 => SHA512.Create(),
            _ => SHA1.Create()
        };
    }

    private static async Task ComputeHashAsync(
        HashAlgorithm hashAlgorithm,
        ChannelReader<(byte[] buffer, int bytesRead)> reader,
        IProgress<FileProcessProgress> progress,
        string filePath,
        long totalBytes,
        CancellationToken ct = default)
    {
        long totalBytesProcessed = 0;

        await foreach (var (buffer, bytesRead) in reader.ReadAllAsync(ct))
        {
            hashAlgorithm.TransformBlock(buffer, 0, bytesRead, null, 0);
            totalBytesProcessed += bytesRead;

            // 报告进度
            progress?.Report(new FileProcessProgress
            {
                SourceFilePath = filePath,
                DestinationFilePath = null, // 哈希计算没有目标文件
                TotalBytes = totalBytes,
                ProcessedBytes = totalBytesProcessed
            });

            ArrayPool<byte>.Shared.Return(buffer);
        }

        hashAlgorithm.TransformFinalBlock([], 0, 0);
    }
}
