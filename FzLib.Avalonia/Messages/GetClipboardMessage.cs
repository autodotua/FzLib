using Avalonia.Input.Platform;

namespace FzLib.Avalonia.Messages
{
    public class GetClipboardMessage()
    {
        public IClipboard Clipboard { get; set; }
    }
}
