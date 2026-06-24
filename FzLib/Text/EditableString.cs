using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using FzLib.Programming;

namespace FzLib.Text;

public partial class EditableString : NotifyPropertyChangedBase
{
    private string stringValue;

    public string Value
    {
        get => stringValue;
        set => SetField(ref stringValue, value);
    }

    public EditableString(string value)
    {
        this.stringValue = value;
    }

    public EditableString()
    {
    }

    // 从 string 隐式转换为 EditableString
    public static implicit operator EditableString(string value)
    {
        return new EditableString(value);
    }

    // 从 EditableString 隐式转换为 string
    public static implicit operator string(EditableString editable)
    {
        return editable?.Value;
    }

    public override string ToString() => Value;

    public class JsonConverter : JsonConverter<EditableString>
    {
        public override EditableString Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string str = reader.GetString();
                return new EditableString(str);
            }

            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            throw new JsonException($"Unexpected token {reader.TokenType} when parsing EditableString.");
        }

        public override void Write(Utf8JsonWriter writer, EditableString value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.Value);
            }
        }
    }
}