using Newtonsoft.Json;
using System.IO;
using static FzLib.DataStorage.Converter;

namespace FzLib.DataStorage.Serialization
{
    public abstract class JsonSerializationBase
    {
        protected JsonSerializationBase()
        {
            Settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        }

        public static T Create<T>(string path = "config.json") where T : JsonSerializationBase, new()
        {
            T instance = new T
            {
                Path = path,
            };
            return instance;
        }
        public static T TryOpenOrCreate<T>(string path = "config.json") where T : JsonSerializationBase, new()
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
        public static T OpenOrCreate<T>(string path = "config.json") where T : JsonSerializationBase, new()
        {
            T instance;
            if (File.Exists(path))
            {
                instance = GetObjectFromJson<T>(File.ReadAllText(path));
            }
            else
            {
                instance = new T();
            }
            instance.Path = path;
            return instance;
        }
        public static T Open<T>(string path = "config.json") where T : JsonSerializationBase, new()
        {
            T instance = GetObjectFromJson<T>(File.ReadAllText(path));
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



        protected string Path { get; private set; } = "config.json";

        protected JsonSerializerSettings Settings { get; set; } = new JsonSerializerSettings();


        public virtual void Save()
        {
            Save(Path);
        }

        public virtual void Save(string path)
        {
            if (path == null)
            {
                path = Path;
            }
            File.WriteAllText(path, GetJson(this, Settings));
        }

    }

}


