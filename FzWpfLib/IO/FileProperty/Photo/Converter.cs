using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace FzLib.IO.FileProperty.Photo
{
    internal static class Converter
    {
        public static Dictionary<int, ExifItem> Items = Init();

        public static Dictionary<int, ExifItem> Init()
        {
            //return TextHelper.Items.ToDictionary(x => x.Id);
            Dictionary<int, ExifItem> dic = new Dictionary<int, ExifItem>();
            foreach (var item in TextHelper.Items)
            {
                if (!dic.ContainsKey(item.Id))
                {
                    dic.Add(item.Id, item);
                }
            }
            return dic;
        }

        public static ExifItem Convert(this PropertyItem item)
        {
            if (!Items.TryGetValue(item.Id, out ExifItem result))
            {
                result = new ExifItem();
                result.Id = item.Id;
            }
            DataType type = (DataType)item.Type;
            result.DataType = type;
            object value = item.Value.GetSpecifiedFormatObject(type, item.Len);
            if (value is string str)
            {
                if (str.EndsWith("\0"))
                {
                    result.Value = str.Substring(0, str.Length - 1);
                }
                else
                {
                    result.Value = value;
                }
            }
            else
            {
                result.Value = value;
            }
            result.Length = item.Len;

            return result;

        }

        public static object GetSpecifiedFormatObject(this byte[] bytes, DataType type, int len)
        {
            switch (type)
            {
                case DataType.Byte:
                    return bytes;
                case DataType.String:
                    return Encoding.ASCII.GetString(bytes);
                case DataType.UInt16:
                    return BitConverter.ToUInt16(bytes.GetSafeBytes(2), 0);
                case DataType.UInt32:
                    return BitConverter.ToUInt32(bytes.GetSafeBytes(4), 0);
                case DataType.URational:
                    return new URational
                    {
                        Denominator = BitConverter.ToUInt32(bytes, 4),
                        Numerator = BitConverter.ToUInt32(bytes, 0)
                    };
                case DataType.Object:
                    return bytes;
                case DataType.Int32:
                    return BitConverter.ToInt32(bytes.GetSafeBytes(4), 0);
                case DataType.Long:
                    return BitConverter.ToInt64(bytes.GetSafeBytes(8), 0);
                case DataType.Rational:
                    return new Rational
                    {
                        Denominator = BitConverter.ToInt32(bytes, 0),
                        Numerator = BitConverter.ToInt32(bytes, 4)
                    };

                default:
                    return bytes;
            }
        }

        public static byte[] GetSafeBytes(this byte[] bytes, int minimun)
        {
            if (bytes.Length >= minimun)
            {
                return bytes;
            }
            var safe = new byte[minimun];
            bytes.CopyTo(safe, minimun - bytes.Length);
            return safe;
        }
    }
}
