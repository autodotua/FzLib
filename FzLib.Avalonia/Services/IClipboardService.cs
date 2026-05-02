using Avalonia.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Services
{
    public interface IClipboardService
    {
        Task ClearAsync();
        Task<T> GetDataAsync<T>(DataFormat<T> dataFormat) where T : class;
        Task<IReadOnlyList<DataFormat>> GetFormatsAsync();
        Task<string> GetTextAsync();
        Task SetDataObjectAsync(IAsyncDataTransfer data);
        Task SetDataObjectAsync<T>(DataFormat<T> format, T value) where T : class;
        Task SetTextAsync(string text);
    }
}