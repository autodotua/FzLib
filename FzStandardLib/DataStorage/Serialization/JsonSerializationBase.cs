using Newtonsoft.Json;
using System;
using System.IO;
using static FzLib.DataStorage.Converter;

namespace FzLib.DataStorage.Serialization
{
    [Obsolete]
    public abstract class JsonSerializationBase
    {
        protected JsonSerializationBase()
        {
            Settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        }

        public static T Create<T>(string path = null, JsonSerializerSettings settings = null) where T : JsonSerializationBase, new()
        {
            path = GetPath(path);
            T instance = new T
            {
                Path = path,
            };
            instance.Loaded = true;
            instance.Settings = settings;
            return instance;
        }

        public static T TryOpenOrCreate<T>(string path = null, JsonSerializerSettings settings = null) where T : JsonSerializationBase, new()
        {
            path = GetPath(path);
            try
            {
                return OpenOrCreate<T>(path, settings);
            }
            catch
            {
                return Create<T>(path);
            }
        }

        public static T OpenOrCreate<T>(string path = null, JsonSerializerSettings settings = null) where T : JsonSerializationBase, new()
        {
            path = GetPath(path);
            T instance;
            if (File.Exists(path))
            {
                instance = GetObjectFromJson<T>(File.ReadAllText(path), settings);
            }
            else
            {
                instance = new T();
            }
            instance.Path = path;
            instance.Settings = settings;
            return instance;
        }

        public static T Open<T>(string path = null, JsonSerializerSettings settings = null) where T : JsonSerializationBase, new()
        {
            path = GetPath(path);
            T instance = GetObjectFromJson<T>(File.ReadAllText(path), settings);
            instance.Path = path;
            instance.Loaded = true;
            instance.Settings = settings;
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

        private static string GetPath(string path)
        {
            if (path == null)
            {
                return System.IO.Path.Combine(Program.App.ProgramDirectoryPath, "config.json");
            }
            return path;
        }

        protected bool Loaded { get; private set; } = false;

        public string Path { get; protected set; }

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
            if (!new FileInfo(path).Directory.Exists)
            {
                new FileInfo(path).Directory.Create();
            }
            File.WriteAllText(path, GetJson(this, Settings));
        }
    }
}