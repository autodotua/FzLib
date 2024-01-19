using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace FzLib.DataStorage.Serialization
{
    public interface IJsonSerializable
    {
    }

    public static class JsonSerializationExtension
    {
        public static bool TryLoadFromJsonFile<T>(this T obj, string path, JsonSerializerSettings settings = null) where T : IJsonSerializable
        {
            if (File.Exists(path))
            {
                obj.LoadFromJsonFile(path, settings);
                return true;
            }
            return false;
        }

        public static void LoadFromJsonFile<T>(this T obj, string path, JsonSerializerSettings settings = null) where T : IJsonSerializable
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("找不到" + path);
            }
            string json = File.ReadAllText(path);
            T template = JsonConvert.DeserializeObject<T>(json, settings);
            new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, T>();
            }).CreateMapper().Map(template, obj);
        }

        public static void Save(this IJsonSerializable obj, string path, JsonSerializerSettings settings = null)
        {
            string json = JsonConvert.SerializeObject(obj, settings);
            File.WriteAllText(path, json);
        }

        public static JsonSerializerSettings SetIndented(this JsonSerializerSettings settings)
        {
            settings.Formatting = Formatting.Indented;
            return settings;
        }

        public static JsonSerializerSettings IgnoreDefaultValue(this JsonSerializerSettings settings)
        {
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            return settings;
        }

        public static JsonSerializerSettings IgnoreNullValue(this JsonSerializerSettings settings)
        {
            settings.NullValueHandling = NullValueHandling.Ignore;
            return settings;
        }

        public static JsonSerializerSettings SetDateTimeFormat(this JsonSerializerSettings settings, string format)
        {
            settings.DateFormatString = format;
            return settings;
        }

        public static JsonSerializerSettings UseStringEnumValue(this JsonSerializerSettings settings)
        {
            settings.Converters.Add(new StringEnumConverter());
            return settings;
        }

        public static JsonSerializerSettings IgnoreReferenceLoop(this JsonSerializerSettings settings)
        {
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return settings;
        }
    }
}