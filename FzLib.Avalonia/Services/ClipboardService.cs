using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Input.Platform;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Services
{
    public class ClipboardService : IClipboardService
    {
        public IClipboard Clipboard => ServiceExtension.GetMainTopLevel()?.Clipboard ??
            throw new InvalidOperationException("找不到剪贴板");

        public async Task ClearAsync() => await Clipboard?.ClearAsync();

        public async Task<T> GetDataAsync<T>(DataFormat<T> dataFormat) where T : class => await (await Clipboard?.TryGetDataAsync()).TryGetValueAsync(dataFormat);

        public async Task<IReadOnlyList<DataFormat>> GetFormatsAsync() => await Clipboard?.GetDataFormatsAsync();

        public async Task<string> GetTextAsync() => await Clipboard?.TryGetTextAsync();

        public async Task SetDataObjectAsync<T>(DataFormat<T> format, T value) where T : class
        {
            var item = new DataTransferItem();
            item.Set(format, value);
            var dt = new DataTransfer();
            dt.Add(item);

            await Clipboard.SetDataAsync(dt);
        }

        public async Task SetDataObjectAsync(IAsyncDataTransfer data) => await Clipboard.SetDataAsync(data);

        public Task SetTextAsync(string text) => Clipboard?.SetTextAsync(text) ?? Task.CompletedTask;
    }
}
