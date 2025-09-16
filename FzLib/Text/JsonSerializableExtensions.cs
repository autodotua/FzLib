using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace FzLib.Text;

public static class JsonSerializableExtensions
{
    public static T FromJson<T>(this IJsonSerializableFactory factory, string json)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentException.ThrowIfNullOrWhiteSpace(json);
        return (T)JsonSerializer.Deserialize(json, typeof(T), factory.Context);
    }

    public static JsonSerializerOptions GetJsonSerializerOptions(
        bool writeIndented = true,
        bool propertyNameCaseInsensitive = true,
        JsonIgnoreCondition ignoreCondition = JsonIgnoreCondition.WhenWritingNull,
        JsonNumberHandling numberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        UnicodeRange unicodeRanges = null,
        IEnumerable<JsonConverter> converters = null)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(unicodeRanges ?? UnicodeRanges.All),
            DefaultIgnoreCondition = ignoreCondition,
            PropertyNameCaseInsensitive = propertyNameCaseInsensitive,
            WriteIndented = writeIndented,
            NumberHandling = numberHandling,
        };

        if (converters != null)
        {
            foreach (var converter in converters)
            {
                options.Converters.Add(converter);
            }
        }

        return options;
    }

    public static T LoadJsonFile<T>(this IJsonFileSerializableFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentException.ThrowIfNullOrWhiteSpace(factory.FileName);
        if (!File.Exists(factory.FileName))
        {
            throw new FileNotFoundException($"文件{factory.FileName}不存在");
        }
        string json = File.ReadAllText(factory.FileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(json);
        return (T)JsonSerializer.Deserialize(json, typeof(T), factory.Context);
    }

    public static T LoadJsonFile<T>(this IJsonFileSerializableFactory factory, Func<T> defaultValue)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentException.ThrowIfNullOrWhiteSpace(factory.FileName);
        if (!File.Exists(factory.FileName))
        {
            return defaultValue();
        }

        string json = File.ReadAllText(factory.FileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(json);
        return (T)JsonSerializer.Deserialize(json, typeof(T), factory.Context);
    }

    public static async Task<T> LoadJsonFileAsync<T>(this IJsonFileSerializableFactory factory,
            CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentException.ThrowIfNullOrWhiteSpace(factory.FileName);
        if (!File.Exists(factory.FileName))
        {
            throw new FileNotFoundException($"文件{factory.FileName}不存在");
        }
        string json = await File.ReadAllTextAsync(factory.FileName, ct);
        ArgumentException.ThrowIfNullOrWhiteSpace(json);
        return (T)JsonSerializer.Deserialize(json, typeof(T), factory.Context);
    }
    public static async Task<T> LoadJsonFileAsync<T>(this IJsonFileSerializableFactory factory, Func<T> defaultValue,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentException.ThrowIfNullOrWhiteSpace(factory.FileName);
        if (!File.Exists(factory.FileName))
        {
            return defaultValue();
        }

        string json = await File.ReadAllTextAsync(factory.FileName, ct);
        ArgumentException.ThrowIfNullOrWhiteSpace(json);
        return (T)JsonSerializer.Deserialize(json, typeof(T), factory.Context);
    }

    public static void SaveJsonFile<T>(this IJsonFileSerializableFactory factory, T obj)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentException.ThrowIfNullOrWhiteSpace(factory.FileName);
        ArgumentNullException.ThrowIfNull(obj);
        string json = JsonSerializer.Serialize(obj, typeof(T), factory.Context);
        File.WriteAllText(factory.FileName, json);
    }

    public static async Task SaveJsonFileAsync<T>(this IJsonFileSerializableFactory factory, T obj,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentException.ThrowIfNullOrWhiteSpace(factory.FileName);
        ArgumentNullException.ThrowIfNull(obj);
        string json = JsonSerializer.Serialize(obj, typeof(T), factory.Context);
        await File.WriteAllTextAsync(factory.FileName, json, ct);
    }

    public static string ToJson<T>(this IJsonSerializableFactory factory, T obj)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(obj);
        return JsonSerializer.Serialize(obj, typeof(T), factory.Context);
    }
}