using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FzLib.Cryptography
{
    public static class AesExtension
    {
        public static RijndaelManaged CreateManager(CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
            => new RijndaelManaged()
            {
                Mode = mode,
                Padding = padding
            };

        public static RijndaelManaged SetStringKey(this RijndaelManaged manager, string key, char fill = (char)0, Encoding encoding = null)
        {
            if (key == null)
            {
                key = "";
            }
            int keyLength = manager.BlockSize / 8;
            if (key.Length < keyLength)
            {
                key = key + new string(fill, keyLength - key.Length);
            }
            else if (key.Length > keyLength)
            {
                key = key.Substring(0, keyLength);
            }
            manager.Key = (encoding ?? Encoding.UTF8).GetBytes(key);
            return manager;
        }

        public static RijndaelManaged SetStringIV(this RijndaelManaged manager, string iv, char fill = (char)0, Encoding encoding = null)
        {
            if (iv == null)
            {
                iv = "";
            }
            int keyLength = manager.BlockSize / 8;
            if (iv.Length < keyLength)
            {
                iv = iv + new string(fill, keyLength - iv.Length);
            }
            else if (iv.Length > keyLength)
            {
                iv = iv.Substring(0, keyLength);
            }
            manager.IV = (encoding ?? Encoding.UTF8).GetBytes(iv);
            return manager;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str">要加密的 string 字符串</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(this RijndaelManaged manager, string str, Encoding encoding = null)
        {
            byte[] stringArray = (encoding ?? Encoding.UTF8).GetBytes(str);

            byte[] result = manager.Encrypt(stringArray);

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="array">要加密的 byte[] 数组</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Encrypt(this RijndaelManaged manager, byte[] array)
        {
            var encryptor = manager.CreateEncryptor();
            return encryptor.TransformFinalBlock(array, 0, array.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str">要解密的 string 字符串</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(this RijndaelManaged manager, string str, Encoding encoding = null)
        {
            byte[] stringArray = Convert.FromBase64String(str);

            byte[] result = manager.Decrypt(stringArray);

            return (encoding ?? DefaultEncoding).GetString(result);
        }

        public static Encoding DefaultEncoding => Encoding.UTF8;

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="array">要解密的 byte[] 数组</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Decrypt(this RijndaelManaged manager, byte[] array)
        {
            var decryptor = manager.CreateDecryptor();
            return decryptor.TransformFinalBlock(array, 0, array.Length);
        }

        public static void EncryptStreamToStream(this RijndaelManaged manager, Stream streamInput, Stream streamOutput, int bufferLength = 1024 * 1024)
        {
            if (!streamInput.CanRead)
            {
                throw new Exception("输入流不可读");
            }
            if (!streamOutput.CanWrite)
            {
                throw new Exception("输出流不可写");
            }
            using (var encryptor = manager.CreateEncryptor())
            {
                byte[] input = new byte[bufferLength];
                byte[] output = new byte[bufferLength];
                int size;
                long length = streamInput.Length;
                while ((size = streamInput.Read(input, 0, bufferLength)) != 0)
                {
                    if (streamInput.Position == length)
                    {
                        output = encryptor.TransformFinalBlock(input, 0, size);
                    }
                    else
                    {
                        encryptor.TransformBlock(input, 0, size, output, 0);
                    }
                    streamOutput.Write(output, 0, output.Length);
                    streamOutput.Flush();
                }
            }
        }

        public static void DecryptStreamToStream(this RijndaelManaged manager, Stream streamInput, Stream streamOutput, int bufferLength = 1024 * 1024)
        {
            if (!streamInput.CanRead)
            {
                throw new Exception("输入流不可读");
            }
            if (!streamOutput.CanWrite)
            {
                throw new Exception("输出流不可写");
            }
            using (var encryptor = manager.CreateDecryptor())
            {
                byte[] input = new byte[bufferLength];
                byte[] output = new byte[bufferLength];
                int size;
                int outputSize = 0;
                long length = streamInput.Length;
                while ((size = streamInput.Read(input, 0, bufferLength)) != 0)
                {
                    if (streamInput.Position == length)
                    {
                        output = encryptor.TransformFinalBlock(input, 0, size);
                        outputSize = output.Length;
                    }
                    else
                    {
                        outputSize = encryptor.TransformBlock(input, 0, size, output, 0);
                    }
                    streamOutput.Write(output, 0, outputSize);
                    streamOutput.Flush();
                }
            }
        }

        private static void CheckFileAndDirectoryExist(string path, bool coverExistedFiles)
        {
            if (File.Exists(path))
            {
                if (coverExistedFiles)
                {
                    File.Delete(path);
                }
                else
                {
                    throw new IOException("文件" + path + "已存在");
                }
            }
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }

        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="sourcePath">源文件地址</param>
        /// <param name="targetPath">目标文件地址</param>
        /// <param name="bufferLength">缓冲区大小</param>
        /// <param name="suffix">加密后的文件后缀</param>
        /// <param name="volumeSize">分卷大小，0表示不分卷</param>
        /// <param name="coverExistedFile">是否覆盖已存在文件。若为False但存在文件，则会抛出异常</param>
        /// <param name="refreshFileProgress"></param>
        /// <returns></returns>
        public static FileInfo EncryptFile(this RijndaelManaged manager, string sourcePath, string targetPath,
            int bufferLength = 1024 * 1024,
            string suffix = null,
            int volumeSize = 0,
            bool coverExistedFile = false,
            RefreshFileProgress refreshFileProgress = null)
        {
            if (volumeSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(volumeSize), "分卷大小不可小于0");
            }
            if (string.IsNullOrEmpty(suffix) && volumeSize > 0)
            {
                throw new ArgumentException("加密文件扩展名为空和分卷加密不可同时存在");
            }
            if (suffix == null)
            {
                suffix = "";
            }

            string encryptedFileName = targetPath + suffix;
            CheckFileAndDirectoryExist(encryptedFileName, coverExistedFile);
            int fileCount = 0;

            try
            {
                using (FileStream streamSource = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
                {
                    FileStream streamTarget = new FileStream(encryptedFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    using (var encryptor = manager.CreateEncryptor())
                    {
                        long currentSize = 0;

                        int size;
                        byte[] input = new byte[bufferLength];
                        byte[] output = new byte[bufferLength];
                        long fileLength = streamSource.Length;
                        while ((size = streamSource.Read(input, 0, bufferLength)) != 0)
                        {
                            if (streamSource.Position == fileLength)
                            {
                                output = encryptor.TransformFinalBlock(input, 0, size);
                            }
                            else
                            {
                                encryptor.TransformBlock(input, 0, size, output, 0);
                            }

                            currentSize += size;

                            if (volumeSize != 0 && currentSize > volumeSize)
                            {
                                streamTarget.Close();
                                streamTarget.Dispose();
                                currentSize = output.Length;
                                fileCount++;
                                encryptedFileName = targetPath + suffix + fileCount;
                                CheckFileAndDirectoryExist(encryptedFileName, coverExistedFile);
                                streamTarget = new FileStream(encryptedFileName, FileMode.OpenOrCreate, FileAccess.Write);
                            }
                            streamTarget.Write(output, 0, output.Length);
                            streamTarget.Flush();
                            refreshFileProgress?.Invoke(sourcePath, encryptedFileName, fileLength, currentSize); //更新进度
                        }
                        streamTarget.Close();
                        streamTarget.Dispose();
                    }
                }
                FileInfo encryptedFile = new FileInfo(targetPath + suffix);

                encryptedFile.Attributes = File.GetAttributes(sourcePath);
                return encryptedFile;
            }
            catch (Exception ex)
            {
                HandleException(sourcePath, encryptedFileName, fileCount, ex, suffix);
                return null;
            }
        }

        private static void HandleException(string sourcePath, string encryptedFileName, int fileCount, Exception ex, string suffix)
        {
            try
            {
                File.Delete(encryptedFileName);
            }
            catch
            {
            }
            for (int i = 1; i <= fileCount; i++)
            {
                try
                {
                    File.Delete(sourcePath + suffix + i);
                }
                catch
                {
                }
            }
            throw ex;
        }

        //private static void CheckVolume(string targetPath, byte[] result, int volumnSize,string extension,bool coverExisted ref string encryptedFileName, ref int fileCount, ref FileStream fsnew, ref long currentSize)
        //{
        //}

        public static FileInfo[] DecryptFile(this RijndaelManaged manager, string sourcePath, string targetPath,
            int bufferLength = 1024 * 1024,
            string suffix = null,
            bool coverExistedFile = false,
            RefreshFileProgress refreshFileProgress = null)
        {
            string target = targetPath.RemoveEnd(suffix);
            CheckFileAndDirectoryExist(target, coverExistedFile);
            List<string> encryptedFileNames = new List<string>() { sourcePath };
            var lastencryptedFile = sourcePath;
            try
            {
                int fileCount = 0;
                while (File.Exists(sourcePath + (++fileCount)))
                {
                    encryptedFileNames.Add(sourcePath + fileCount);

                    lastencryptedFile = sourcePath + fileCount;
                }

                using (FileStream streamTarget = new FileStream(target, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var decryptor = manager.CreateDecryptor())
                    {
                        foreach (var encryptedFileName in encryptedFileNames)
                        {
                            FileStream streamSource = new FileStream(encryptedFileName, FileMode.Open, FileAccess.Read);

                            long currentSize = 0;

                            int size;
                            byte[] input = new byte[bufferLength];
                            byte[] output = new byte[bufferLength];
                            long fileLength = streamSource.Length;
                            while ((size = streamSource.Read(input, 0, input.Length)) != 0)
                            {
                                int outputSize = 0;
                                if (streamSource.Position == fileLength && encryptedFileName == lastencryptedFile)
                                {
                                    outputSize = (output = decryptor.TransformFinalBlock(input, 0, size)).Length;
                                }
                                else
                                {
                                    outputSize = decryptor.TransformBlock(input, 0, size, output, 0);
                                }

                                currentSize += output.Length;

                                streamTarget.Write(output, 0, outputSize);
                                streamTarget.Flush();
                                refreshFileProgress?.Invoke(sourcePath, encryptedFileName, fileLength, currentSize); //更新进度
                            }
                            streamSource.Close();
                            streamSource.Dispose();
                        }
                    }

                    streamTarget.Close();
                    streamTarget.Dispose();
                }

                new FileInfo(target).Attributes = File.GetAttributes(sourcePath);
                return encryptedFileNames.Select(p => new FileInfo(p)).ToArray();
            }
            catch (Exception ex)
            {
                HandleException(target, ex);
                return null;
            }
        }

        private static void HandleException(string target, Exception ex)
        {
            try
            {
                File.Delete(target);
            }
            catch
            {
            }
            throw ex;
        }

        /// <summary>
        /// 更新文件加密进度
        /// </summary>
        public delegate void RefreshFileProgress(string source, string target, long max, long value);
    }

    //public class Aes : CryptographyBase
    //{
    //    #region 字段

    //    public RijndaelManaged manager { get; } = new RijndaelManaged();

    //    #endregion 字段

    //    #region 可修改属性

    //    private string encryptedExtention = ".encrypted";

    //    public string EncryptedFileExtention
    //    {
    //        get => encryptedExtention;
    //        set
    //        {
    //            if (string.IsNullOrWhiteSpace(value))
    //            {
    //                value = "";
    //            }
    //            encryptedExtention = value;
    //        }
    //    }

    //    public long VolumeSize { get; set; } = 0;
    //    public bool CoverExistFiles { get; set; } = false;
    //    public bool DeleteSourceFile { get; set; } = false;

    //    #endregion 可修改属性

    //    public override void Dispose()
    //    {
    //        manager.Dispose();
    //    }
    //}
}