using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace FzLib.Control
{
    public static class Tools
    {


        /// <summary>
        /// 把对象序列化为字节数组
        /// </summary>
        public static byte[] SerializeObject<T>(T obj) 
        {
            object[] attributes = obj.GetType().GetCustomAttributes(typeof(SerializableAttribute),true);
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
        public static T DeserializeObject<T>(byte[] bytes) 
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
