using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FzLib.DataStorage.Serialization
{
    public class BinarySerialization
    { /// <summary>
      /// 把对象序列化为字节数组
      /// </summary>
        public static byte[] Serialize<T>(T obj)
        {
            object[] attributes = obj.GetType().GetCustomAttributes(typeof(SerializableAttribute), true);
            if (attributes == null || attributes.Length == 0)
            {
                throw new InvalidOperationException("无法序列化");
            }
            if (obj == null)
            {
                return null;
            }
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;
            byte[] bytes = new byte[ms.Length];
            ms.Read(bytes, 0, bytes.Length);
            ms.Close();
            return bytes;
        }

        /// <summary>
        /// 把字节数组反序列化成对象
        /// </summary>
        public static T Deserialize<T>(byte[] bytes)
        {
            T obj = default(T);
            if (bytes == null)
            {
                return obj;
            }
            MemoryStream ms = new MemoryStream(bytes)
            {
                Position = 0
            };
            BinaryFormatter formatter = new BinaryFormatter();
            obj = (T)formatter.Deserialize(ms);
            ms.Close();
            return obj;
        }
    }
}
