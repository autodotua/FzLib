using Avalonia.Input;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Services
{
    public interface IClipboardService
    {
        Task ClearAsync();
        Task<object> GetDataAsync(string format);
        Task<string[]> GetFormatsAsync();
        Task<string> GetTextAsync();
        Task SetDataObjectAsync(IDataObject data);
        Task SetTextAsync(string text);
    }
}