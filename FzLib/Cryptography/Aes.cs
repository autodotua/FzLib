using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using FzLib.Basic;

namespace FzLib.Cryptography
{
    public class Aes : CryptographyBase
    {


        #region 字段
        public RijndaelManaged Manager { get; } = new RijndaelManaged();
        #endregion



        #region 可修改属性
        private string encryptedExtention = ".encrypted";
        public string EncryptedFileExtention
        {
            get => encryptedExtention;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = "";
                }
                encryptedExtention = value;
            }
        }
        public long VolumeSize { get; set; } = 0;
        public bool CoverExistFiles { get; set; } = false;
        public bool DeleteSourceFile { get; set; } = false;
        #endregion






        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str">要加密的 string 字符串</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Encrypt(string str)
        {
            byte[] stringArray = StringEncoding.GetBytes(str);

            byte[] result = Encrypt(stringArray);

            return Convert.ToBase64String(result);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="array">要加密的 byte[] 数组</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] array)
        {
            var encryptor = Manager.CreateEncryptor();
            return encryptor.TransformFinalBlock(array, 0, array.Length);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str">要解密的 string 字符串</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Decrypt(string str)
        {
            byte[] stringArray = Convert.FromBase64String(str);

            byte[] result = Decrypt(stringArray);

            return StringEncoding.GetString(result);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="array">要解密的 byte[] 数组</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] array)
        {
            var decryptor = Manager.CreateDecryptor();
            return decryptor.TransformFinalBlock(array, 0, array.Length);

        }
        public string SetStringKey(string key, char fill = (char)0)
        {
            if (key == null)
            {
                key = "";
            }
            int keyLength = Manager.BlockSize / 8;
            if (key.Length < keyLength)
            {
                key = key + new string(fill, keyLength - key.Length);
            }
            else if (key.Length > keyLength)
            {
                key = key.Substring(0, keyLength);
            }
            Manager.Key = StringEncoding.GetBytes(key);
            return key;
        }

        public string SetStringIV(string iv, char fill = (char)0)
        {
            if (iv == null)
            {
                iv = "";
            }
            int keyLength = Manager.BlockSize / 8;
            if (iv.Length < keyLength)
            {
                iv = iv + new string(fill, keyLength - iv.Length);
            }
            else if (iv.Length > keyLength)
            {
                iv = iv.Substring(0, keyLength);
            }
            Manager.IV = StringEncoding.GetBytes(iv);
            return iv;
        }

        private void CheckFileAndDirectoryExist(string path)
        {

            if (File.Exists(path))
            {
                if (CoverExistFiles)
                {
                    File.Delete(path);
                }
                else
                {
                    throw new Exception("文件"+path+"已存在");
                }
            }
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }

        public void EncryptStreamToStream(Stream streamInput, Stream streamOutput)
        {
            if (!streamInput.CanRead)
            {
                throw new Exception("输入流不可读");
            }
            if (!streamOutput.CanWrite)
            {
                throw new Exception("输出流不可写");
            }
            using (var encryptor = Manager.CreateEncryptor())
            {

                byte[] input = new byte[BufferLength];
                byte[] output = new byte[BufferLength];
                int size;
                long length = streamInput.Length;
                while ((size = streamInput.Read(input, 0, BufferLength)) != 0)
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

        public void DecryptStreamToStream(Stream streamInput, Stream streamOutput)
        {
            if (!streamInput.CanRead)
            {
                throw new Exception("输入流不可读");
            }
            if (!streamOutput.CanWrite)
            {
                throw new Exception("输出流不可写");
            }
            using (var encryptor = Manager.CreateDecryptor())
            {

                byte[] input = new byte[BufferLength];
                byte[] output = new byte[BufferLength];
                int size;
                int outputSize = 0;
                long length = streamInput.Length;
                while ((size = streamInput.Read(input, 0, BufferLength)) != 0)
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


        public void EncryptFile(string sourcePath, string targetPath, RefreshFileProgress refreshFileProgress = null)
        {
            if (EncryptedFileExtention == "" && VolumeSize > 0)
            {
                throw new Exception("加密文件扩展名为空和分卷加密不可同时存在");
            }

            string encryptedFileName = targetPath + encryptedExtention;
            CheckFileAndDirectoryExist(encryptedFileName);
            int fileCount = 0;

            try
            {

                using (FileStream streamSource = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
                {
                    FileStream streamTarget = new FileStream(encryptedFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    using (var encryptor = Manager.CreateEncryptor())
                    {
                        long currentSize = 0;

                        int size;
                        byte[] input = new byte[BufferLength];
                        byte[] output = new byte[BufferLength];
                        long fileLength = streamSource.Length;
                        while ((size = streamSource.Read(input, 0, BufferLength)) != 0)
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
                            CheckVolume(targetPath, ref encryptedFileName, ref fileCount, ref streamTarget, ref currentSize, output);

                            streamTarget.Write(output, 0, output.Length);
                            streamTarget.Flush();
                            refreshFileProgress?.Invoke(sourcePath, encryptedFileName, fileLength, currentSize); //更新进度

                        }
                        streamTarget.Close();
                        streamTarget.Dispose();
                    }

                }
                FileInfo encryptedFile = new FileInfo(targetPath + encryptedExtention)
                {
                    Attributes = File.GetAttributes(sourcePath)
                };
                if (DeleteSourceFile)
                {
                    File.Delete(sourcePath);
                }

            }
            catch (Exception ex)
            {
                HandleException(sourcePath, encryptedFileName, fileCount, ex);
            }
        }

        private void HandleException(string sourcePath, string encryptedFileName, int fileCount, Exception ex)
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
                    File.Delete(sourcePath + encryptedExtention + i);
                }
                catch
                {

                }
            }
            throw ex;
        }

        private void CheckVolume(string targetPath, ref string encryptedFileName, ref int fileCount, ref FileStream fsnew, ref long currentSize, byte[] result)
        {
            if (VolumeSize != 0 && currentSize > VolumeSize)
            {
                fsnew.Close();
                fsnew.Dispose();
                currentSize = result.Length;
                fileCount++;
                encryptedFileName = targetPath + encryptedExtention + fileCount;
                CheckFileAndDirectoryExist(encryptedFileName);
                fsnew = new FileStream(encryptedFileName, FileMode.OpenOrCreate, FileAccess.Write);

            }
        }

        public void DecryptFile(string sourcePath, string targetPath, RefreshFileProgress refreshFileProgress = null)
        {

            string target = targetPath.RemoveEnd(encryptedExtention);
            CheckFileAndDirectoryExist(target);
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
                    using (var decryptor = Manager.CreateDecryptor())
                    {
                        foreach (var encryptedFileName in encryptedFileNames)
                        {
                            FileStream streamSource = new FileStream(encryptedFileName, FileMode.Open, FileAccess.Read);

                            long currentSize = 0;

                            int size;
                            byte[] input = new byte[BufferLength];
                            byte[] output = new byte[BufferLength];
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


                FileInfo rawFile = new FileInfo(target)
                {
                    Attributes = File.GetAttributes(sourcePath)
                };
                if (DeleteSourceFile)
                {
                    foreach (var file in encryptedFileNames)
                    {
                        File.Delete(file);
                    }
                }

            }
            catch (Exception ex)
            {
                HandleException(target, ex);
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

        public override void Dispose()
        {
            Manager.Dispose();
        }

        /// <summary>
        /// 更新文件加密进度
        /// </summary>
        public delegate void RefreshFileProgress(string source, string target, long max, long value);

    }




}