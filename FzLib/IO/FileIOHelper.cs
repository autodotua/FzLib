using System.Buffers;
using System.Threading.Channels;

namespace FzLib.IO;

internal static class FileIOHelper
{
    internal static int GetOptimalBufferSize(long fileLength)
    {
        return fileLength switch
        {
            < 1 * 1024 * 1024 => 16 * 1024, // 小文件（<1MB）：16KB
            < 32 * 1024 * 1024 => 1 * 1024 * 1024, // 中等文件（1MB~32MB）：1MB
            _ => 4 * 1024 * 1024 // 大文件（>32MB）：4MB
        };
    }

    internal static async Task ReadDataAsync(
        FileStream sourceStream,
        ChannelWriter<(byte[] buffer, int bytesRead)> writer,
        int bufferSize,
        CancellationToken ct)
    {
        byte[] readBuffer = ArrayPool<byte>.Shared.Rent(bufferSize);
        try
        {
            while (true)
            {
                int bytesRead = await sourceStream.ReadAsync(readBuffer.AsMemory(0, bufferSize), ct);
                if (bytesRead <= 0) break;

                var bufferToSend = readBuffer;
                readBuffer = ArrayPool<byte>.Shared.Rent(bufferSize); // 提前租用下一个
                await writer.WriteAsync((bufferToSend, bytesRead), ct);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(readBuffer); // 确保最后一个缓冲区被返还
            writer.Complete();
        }
    }
}