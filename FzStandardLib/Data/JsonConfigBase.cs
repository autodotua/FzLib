using System.Collections.Generic;
using System.IO;

namespace FzLib.Data
{
    public abstract class JsonConfigBase
    {

        public static T Creat<T>(string path = "config.json") where T : JsonConfigBase, new()
        {
            T instance = new T
            {
                Path = path,
            };
            return instance;
        }
        public static T OpenOrCreat<T>(string path = "config.json") where T : JsonConfigBase, new()
        {
            T instance;
            if (File.Exists(path))
            {
                instance = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            else
            {
                instance = new T();
            }
            instance.Path = path;
            return instance;
        }
        public static T Open<T>(string path = "config.json") where T : JsonConfigBase, new()
        {
            T instance = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            instance.Path = path;
            return instance;
        }


        protected JsonConfigBase() { }



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


        public void Save(bool format = false)
        {
            Save(Path,format);
        }

        public void Save(string path, bool format = false)
        {
            if (path == null)
            {
                path = Path;
            }

            if (format)
            {
                File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented));
            }
            else
            {
                File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(this));
            }
        }

    }

}


