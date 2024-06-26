using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;

namespace FzLib.Avalonia.Messages
{
    public class GetStorageProviderMessage
    {
        public IStorageProvider StorageProvider { get; set; }
    }
}
