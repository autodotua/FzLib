using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FzLib.Avalonia.Dialogs;
using FzLib.Text;

namespace FzLib.Samples.ViewModels;

public partial class JsonViewModel : ObservableObject
{
    private readonly IDialogService dialogService;
    [ObservableProperty]
    private string message;

    JsonTest test = new JsonTest();
    public JsonViewModel(IDialogService dialogService)
    {
        this.dialogService = dialogService;
        if (RuntimeFeature.IsDynamicCodeSupported)
        {
            Message = "当前运行在非AOT环境，请编译为AOT后进行测试";
        }
        else
        {
            Message = "当前可能运行在AOT环境";
        }
    }

    [RelayCommand]
    private async Task TestAsync()
    {
        JsonTest obj = new JsonTest();
        string json = JsonTestFactory.Instance.ToJson(obj);
        Message = json;
        JsonTest newObj = JsonTestFactory.Instance.FromJson<JsonTest>(json);
        if (newObj.Equals(obj))
        {
            await dialogService.ShowOkDialogAsync("反序列化", "序列化及反序列化成功，新对象与原对象相同");
        }
        else
        {
            await dialogService.ShowErrorDialogAsync("反序列化失败", "序列化及反序列化失败");
        }
    }

    [RelayCommand]
    private async Task TestFileAsync()
    {
        JsonTest obj = new JsonTest();
        await JsonTestFactory.Instance.SaveJsonFileAsync(obj);
        Message = await File.ReadAllTextAsync(JsonTestFactory.Instance.FileName);
        JsonTest newObj = await JsonTestFactory.Instance.LoadJsonFileAsync<JsonTest>();
        if (newObj.Equals(obj))
        {
            await dialogService.ShowOkDialogAsync("反序列化", "序列化到文件及从文件反序列化成功，新对象与原对象相同");
        }
        else
        {
            await dialogService.ShowErrorDialogAsync("反序列化失败", "序列化到文件及从文件反序列化");
        }
    }

    class JsonTest : IEquatable<JsonTest>
    {
        public bool BoolValue { get; set; } = true;
        public AnotherClass CustomClass { get; set; } = new();
        public DateTime DateTimeValue { get; set; } = DateTime.Now;
        public Dictionary<int, string> DictValue { get; set; } =
            new() { { 1, "1" }, { 2, "2" }, { 3, "3" }, { 4, "4" }, { 5, "5" } };

        public double DoubleValue { get; set; } = 1234567890.1234567890;
        public Guid GuidValue { get; set; } = Guid.NewGuid();
        public int IntValue { get; set; } = 1234567890;
        public List<int> ListValue { get; set; } = new() { 1, 2, 3, 4, 5 };
        public object ObjIntValue { get; set; } = 1234567890;
        public object ObjStringValue { get; set; } = "Hello World Again";
        public string StringValue { get; set; } = "Hello World";
        public TimeSpan TimeSpanValue { get; set; } = TimeSpan.FromSeconds(1234567890);
        public Uri UriValue { get; set; } = new Uri("https://www.baidu.com");
        public bool Equals(JsonTest other)
        {
            if (other is null) return false;

            return StringValue == other.StringValue
                   && IntValue == other.IntValue
                   && DoubleValue.Equals(other.DoubleValue)
                   && BoolValue == other.BoolValue
                   && DateTimeValue == other.DateTimeValue
                   && TimeSpanValue == other.TimeSpanValue
                   && GuidValue == other.GuidValue
                   && Equals(UriValue, other.UriValue)
                   && ListValue.SequenceEqual(other.ListValue)
                   && DictValue.Count == other.DictValue.Count
                   && !DictValue.Except(other.DictValue).Any()
                   && Equals(ObjIntValue, other.ObjIntValue)
                   && Equals(ObjStringValue, other.ObjStringValue)
                   && Equals(CustomClass, other.CustomClass);
        }

        public override bool Equals(object obj) => Equals(obj as JsonTest);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(StringValue);
            hash.Add(IntValue);
            hash.Add(DoubleValue);
            hash.Add(BoolValue);
            hash.Add(DateTimeValue);
            hash.Add(TimeSpanValue);
            hash.Add(GuidValue);
            hash.Add(UriValue);
            if (ListValue != null)
            {
                foreach (var i in ListValue)
                    hash.Add(i);
            }

            if (DictValue != null)
            {
                foreach (var kv in DictValue.OrderBy(kv => kv.Key))
                {
                    hash.Add(kv.Key);
                    hash.Add(kv.Value);
                }
            }

            hash.Add(ObjIntValue);
            hash.Add(ObjStringValue);
            hash.Add(CustomClass);
            return hash.ToHashCode();
        }

        public class AnotherClass : IEquatable<AnotherClass>
        {
            public int IntValue { get; set; } = 1234567890;
            public string StringValue { get; set; } = "Hello World";
            public bool Equals(AnotherClass other)
            {
                if (other is null) return false;
                return StringValue == other.StringValue && IntValue == other.IntValue;
            }

            public override bool Equals(object obj) => Equals(obj as AnotherClass);

            public override int GetHashCode() => HashCode.Combine(StringValue, IntValue);
        }
    }

    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(JsonTest))]
    partial class JsonTestContext : JsonSerializerContext
    {
        static JsonTestContext()
        {
            Instance = new JsonTestContext(JsonSerializableExtensions.GetJsonSerializerOptions(converters:
                [new JsonTestFactory.ObjectJsonConverter()]));
        }

        public static JsonTestContext Instance { get; }
    }

    class JsonTestFactory : IJsonFileSerializableFactory
    {
        public static JsonTestFactory Instance { get; } = new JsonTestFactory();
        public JsonSerializerContext Context => JsonTestContext.Instance;

        public string FileName => "test_json.json";

        public class ObjectJsonConverter : JsonConverter<object>
        {
            public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.Number:
                        if (reader.TryGetInt32(out int i)) return i;
                        if (reader.TryGetInt64(out long l)) return l;
                        return reader.GetDouble();
                    case JsonTokenType.String:
                        return reader.GetString();
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                        return reader.GetBoolean();
                    case JsonTokenType.StartObject:
                        using (var doc = JsonDocument.ParseValue(ref reader))
                            return doc.RootElement.Clone(); // 可以再解析为具体类型
                    case JsonTokenType.StartArray:
                        using (var doc = JsonDocument.ParseValue(ref reader))
                            return doc.RootElement.Clone();
                    default:
                        return null;
                }
            }

            public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
            {
                switch (value)
                {
                    case int i: writer.WriteNumberValue(i); break;
                    case long l: writer.WriteNumberValue(l); break;
                    case double d: writer.WriteNumberValue(d); break;
                    case string s: writer.WriteStringValue(s); break;
                    case bool b: writer.WriteBooleanValue(b); break;
                    default:
                        JsonSerializer.Serialize(writer, value, value.GetType(), options);
                        break;
                }
            }
        }
    }
}