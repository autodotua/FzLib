using System;
using System.IO;
using System.Text;

namespace FzLib.Text
{
    public static class EncodingDetector
    {
        /// <summary>
        /// 通过字节数组检测文本编码
        /// </summary>
        public static Encoding DetectFromBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            using var stream = new MemoryStream(bytes);
            return DetectFromStream(stream);
        }

        /// <summary>
        /// 通过文件路径检测文本编码
        /// </summary>
        public static Encoding DetectFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return DetectFromStream(fs);
        }
        /// <summary>
        /// 通过流检测文本编码（核心实现）
        /// </summary>
        public static Encoding DetectFromStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanRead)
            {
                throw new ArgumentException("Stream is not readable", nameof(stream));
            }

            // 只读取前4字节足够判断大多数编码
            byte[] buffer = new byte[4];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            // 优先检查BOM标记
            if (bytesRead >= 2)
            {
                if (buffer[0] == 0xFF && buffer[1] == 0xFE) return Encoding.Unicode;
                if (buffer[0] == 0xFE && buffer[1] == 0xFF) return Encoding.BigEndianUnicode;
                if (bytesRead >= 3 && buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF) return Encoding.UTF8;
            }

            // 没有BOM时，使用更高效的UTF8检测
            if (IsLikelyUtf8(buffer))
            {
                return Encoding.UTF8;
            }

            // 回退到系统默认编码
            return Encoding.Default;
        }

        /// <summary>
        /// 快速判断可能是UTF8编码（无BOM）
        /// </summary>
        private static bool IsLikelyUtf8(byte[] buffer)
        {
            try
            {
                // 简单检查UTF8多字节序列模式
                for (int i = 0; i < buffer.Length;)
                {
                    byte b = buffer[i];
                    if (b <= 0x7F) { i++; continue; }

                    int bytesToFollow = 0;
                    if ((b & 0xE0) == 0xC0) bytesToFollow = 1;
                    else if ((b & 0xF0) == 0xE0) bytesToFollow = 2;
                    else if ((b & 0xF8) == 0xF0) bytesToFollow = 3;
                    else return false;

                    while (bytesToFollow-- > 0)
                    {
                        if (++i >= buffer.Length) return false;
                        if ((buffer[i] & 0xC0) != 0x80) return false;
                    }
                    i++;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}