using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Services
{
    public interface IStorageProviderService
    {
        Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options);
        Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(FolderPickerOpenOptions options);
        Task<IStorageFile> SaveFilePickerAsync(FilePickerSaveOptions options);
        Task<IStorageFolder> TryGetWellKnownFolderAsync(WellKnownFolder wellKnownFolder);
        Task<string> OpenFilePickerAndGetFirstAsync(FilePickerOpenOptions options);
        Task<string> OpenFolderPickerAndGetFirstAsync(FolderPickerOpenOptions options);
        Task<string> SaveFilePickerAndGetPathAsync(FilePickerSaveOptions options);
        bool CanPickFolder { get; }
        bool CanOpen { get; }
        bool CanSave { get; }
    }
}