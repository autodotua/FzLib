using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Input.Platform;
using System;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Services
{
    public class ClipboardService : IClipboardService
    {
        public IClipboard Clipboard => ServiceExtension.GetMainTopLevel()?.Clipboard ??
            throw new InvalidOperationException("找不到剪贴板");

        public Task ClearAsync() => Clipboard?.ClearAsync() ?? Task.CompletedTask;

        public Task<object> GetDataAsync(string format) => Clipboard?.GetDataAsync(format) ?? Task.FromResult<object>(null);

        public Task<string[]> GetFormatsAsync() => Clipboard?.GetFormatsAsync() ?? Task.FromResult(Array.Empty<string>());

        public Task<string> GetTextAsync() => Clipboard?.GetTextAsync() ?? Task.FromResult<string>(null);

        public Task SetDataObjectAsync(IDataObject data) => Clipboard?.SetDataObjectAsync(data) ?? Task.CompletedTask;

        public Task SetTextAsync(string text) => Clipboard?.SetTextAsync(text) ?? Task.CompletedTask;
    }
}
