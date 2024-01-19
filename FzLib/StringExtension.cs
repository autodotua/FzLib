using System;
using System.IO;
using System.Text;

namespace FzLib
{
    public static class StringExtension
    {
        /// <summary>
        /// 移除字符串头部指定字符串
        /// </summary>
        /// <param name="str">输入的字符串</param>
        /// <param name="neededToRemove">需要移除的内容</param>
        /// <param name="repeat">是否重复操作指导头部不再有指定字符串</param>
        /// <returns></returns>
        public static string RemoveStart(this string str, string neededToRemove, bool repeat = false)
        {
            if (str == null)
            {
                return null;
            }
            if (neededToRemove.Length > str.Length)
            {
                return str;
            }
            string result = str.Substring(neededToRemove.Length);
            if (neededToRemove + result != str)
            {
                return str;
            }
            if (repeat)
            {
                string repeatedResult;
                while ((repeatedResult = result.RemoveStart(neededToRemove)) != result)
                {
                    result = repeatedResult;
                }
            }
            return result;
        }

        /// <summary>
        /// 移除字符串尾部指定字符串
        /// </summary>
        /// <param name="str">输入的字符串</param>
        /// <param name="neededToRemove">需要移除的内容</param>
        /// <param name="repeat">是否重复操作指导尾部不再有指定字符串</param>
        /// <returns></returns>
        public static string RemoveEnd(this string str, string neededToRemove, bool repeat = false)
        {
            if (str == null)
            {
                return null;
            }
            if (neededToRemove.Length > str.Length)
            {
                return str;
            }
            string result = str.Remove(str.Length - neededToRemove.Length);

            if (result + neededToRemove != str)
            {
                return str;
            }
            if (repeat)
            {
                string repeatedResult;
                while ((repeatedResult = result.RemoveEnd(neededToRemove)) != result)
                {
                    result = repeatedResult;
                }
            }
            return result;
        }

        /// <summary>
        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件的编码类型</returns>
        public static Encoding GetEncoding(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }
            Encoding e;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                e = GetEncoding(fs);
            }
            return e;
        }

        /// <summary>
        /// 通过给定的字节数组，判断文件编码
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }
            Encoding e;
            using (Stream stream = new MemoryStream(bytes))
            {
                e = GetEncoding(stream);
            }
            return e;
        }

        /// <summary>
        /// 通过给定的流，判断文件的编码类型
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns>文件的编码类型</returns>
        public static Encoding GetEncoding(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException();
            }
            if (!stream.CanRead)
            {
                throw new Exception("该流不可读");
            }
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
            Encoding reVal = Encoding.Default;

            BinaryReader r = new BinaryReader(stream, Encoding.Default);
            int.TryParse(stream.Length.ToString(), out int i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;
        }

        /// <summary>
        /// 判断是否是不带 BOM 的 UTF8 格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1; //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }

        public static byte[] ToUTF8Bytes(this string str, bool bom = false)
        {
            return new UTF8Encoding(bom).GetBytes(str);
        }

        public static string ToUTF8String(this byte[] bytes, bool bom = false)
        {
            return new UTF8Encoding(bom).GetString(bytes);
        }

        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static byte[] ToBase64Bytes(this string str)
        {
            return Convert.FromBase64String(str);
        }
    }
}