using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using static FzLib.Data.Converter;

namespace FzLib.Data.Serialization
{
    public abstract class XmlSerializationBase
    {
        protected XmlSerializationBase() { }

        public static T Create<T>(string path = "config.xml") where T : XmlSerializationBase, new()
        {
            T instance = new T
            {
                Path = path,
            };
            return instance;
        }
        public static T TryOpenOrCreate<T>(string path = "config.xml") where T : XmlSerializationBase, new()
        {
            try
            {
                return OpenOrCreate<T>(path);
            }
            catch
            {
                return Create<T>(path);
            }
        }
        public static T OpenOrCreate<T>(string path = "config.xml") where T : XmlSerializationBase, new()
        {
            T instance;
            if (File.Exists(path))
            {
                instance = GetObjectFromXml<T>(File.ReadAllText(path));
            }
            else
            {
                instance = new T();
            }
            instance.Path = path;
            return instance;
        }
        public static T Open<T>(string path = "config.xml") where T : XmlSerializationBase, new()
        {
            T instance = GetObjectFromXml<T>(File.ReadAllText(path));
            instance.Path = path;
            return instance;
        }





        public static bool Reset(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }



        protected string Path { get; private set; } = "config.xml";


        protected JsonSerializerSettings Settings { get; set; } = new JsonSerializerSettings();
        public virtual void Save(bool format = false)
        {
            Save(Path,format);
        }

        public virtual void Save(string path, bool format = false)
        {
            if (path == null)
            {
                path = Path;
            }
            File.WriteAllText(path, GetXml(this, Settings));
        }

    }

}


