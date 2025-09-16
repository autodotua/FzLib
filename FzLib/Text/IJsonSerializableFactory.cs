using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace FzLib.Text;

public interface IJsonFileSerializableFactory : IJsonSerializableFactory
{
    public string FileName { get; }
}

public interface IJsonSerializableFactory
{
    public JsonSerializerContext Context { get; }
}