using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Services
{
    public interface IStorageProviderService
    {
        bool CanOpen { get; }

        bool CanPickFolder { get; }

        bool CanSave { get; }

        IStorageProviderServicePickerBuilder CreatePickerBuilder();

        Task<string> OpenFilePickerAndGetFirstAsync(FilePickerOpenOptions options);

        Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options);

        Task<string> OpenFolderPickerAndGetFirstAsync(FolderPickerOpenOptions options);

        Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(FolderPickerOpenOptions options);

        Task<string> SaveFilePickerAndGetPathAsync(FilePickerSaveOptions options);

        Task<IStorageFile> SaveFilePickerAsync(FilePickerSaveOptions options);

        Task<IStorageFolder> TryGetWellKnownFolderAsync(WellKnownFolder wellKnownFolder);
    }
}