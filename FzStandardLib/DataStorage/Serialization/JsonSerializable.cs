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
        string Path { get; }
        JsonSerializerSettings Settings { get; set; }
    }

    internal class ShouldSerializeContractResolver : DefaultContractResolver
    {
        public static ShouldSerializeContractResolver Instance { get; } = new ShouldSerializeContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (member.Name == nameof(IJsonSerializable.Path) || member.Name == nameof(IJsonSerializable.Settings))
            {
                property.Ignored = true;
            }
            return property;
        }
    }

    public static class JsonSerializationExtension
    {
        public static bool TryLoadFromJsonFile<T>(this T obj) where T : IJsonSerializable
        {
            return TryLoadFromJsonFile(obj, obj.Path);
        }

        public static bool TryLoadFromJsonFile<T>(this T obj, string path) where T : IJsonSerializable
        {
            if (File.Exists(path))
            {
                obj.LoadFromJsonFile(path);
                return true;
            }
            return false;
        }

        public static void LoadFromJsonFile<T>(this T obj) where T : IJsonSerializable
        {
            LoadFromJsonFile(obj, obj.Path);
        }

        public static void LoadFromJsonFile<T>(this T obj, string path) where T : IJsonSerializable
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("找不到" + path);
            }
            string json = File.ReadAllText(path);
            T template = JsonConvert.DeserializeObject<T>(json, obj.Settings);
            new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, T>();
            }).CreateMapper().Map(template, obj);
        }

        public static bool DelectConfigFile<T>(this T obj) where T : IJsonSerializable
        {
            if (File.Exists(obj.Path))
            {
                File.Delete(obj.Path);
                return true;
            }
            return false;
        }

        public static void Save(this IJsonSerializable obj)
        {
            obj.Save(obj.Path);
        }

        public static void Save(this IJsonSerializable obj, string path)
        {
            var settings = obj.Settings ?? new JsonSerializerSettings();
            settings.ContractResolver = ShouldSerializeContractResolver.Instance;
            string json = JsonConvert.SerializeObject(obj, settings);
            File.WriteAllText(path, json);
        }

        private static IJsonSerializable EnsureSettings(this IJsonSerializable obj)
        {
            if (obj.Settings == null)
            {
                obj.Settings = new JsonSerializerSettings();
            }
            return obj;
        }

        public static IJsonSerializable SetIndented(this IJsonSerializable obj)
        {
            obj.EnsureSettings().Settings.Formatting = Formatting.Indented;
            return obj;
        }

        public static IJsonSerializable IgnoreDefaultValue(this IJsonSerializable obj)
        {
            obj.EnsureSettings().Settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            return obj;
        }

        public static IJsonSerializable IgnoreNullValue(this IJsonSerializable obj)
        {
            obj.EnsureSettings().Settings.NullValueHandling = NullValueHandling.Ignore;
            return obj;
        }

        public static IJsonSerializable SetDateTimeFormat(this IJsonSerializable obj, string format)
        {
            obj.EnsureSettings().Settings.DateFormatString = format;
            return obj;
        }

        public static IJsonSerializable UseStringEnumValue(this IJsonSerializable obj)
        {
            obj.EnsureSettings().Settings.Converters.Add(new StringEnumConverter());
            return obj;
        }

        public static IJsonSerializable IgnoreReferenceLoop(this IJsonSerializable obj)
        {
            obj.EnsureSettings().Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return obj;
        }
    }
}