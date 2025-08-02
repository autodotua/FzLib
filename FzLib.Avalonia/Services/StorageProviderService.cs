using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FzLib.Avalonia.Dialogs.Pickers;

namespace FzLib.Avalonia.Services
{
    public class StorageProviderService : IStorageProviderService
    {
        public bool CanOpen => StorageProvider?.CanOpen ?? false;

        public bool CanPickFolder => StorageProvider?.CanPickFolder ?? false;

        public bool CanSave => StorageProvider?.CanSave ?? false;

        protected virtual IStorageProvider StorageProvider => TopLevelExtension.GetMainTopLevel()?.StorageProvider ??
                                    throw new InvalidOperationException("找不到存储提供程序");

        public IStorageProviderServicePickerBuilder CreatePickerBuilder()
        {
            return new FilePickerOptionsBuilder(this);
        }

        public async Task<string> OpenFilePickerAndGetFirstAsync(FilePickerOpenOptions options)
        {
            var files = await OpenFilePickerAsync(options);
            return files.Count > 0 ? files[0].TryGetLocalPath() : null;
        }

        public async Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options)
        {
            return StorageProvider != null
                ? await StorageProvider.OpenFilePickerAsync(options)
                : new List<IStorageFile>();
        }
        public async Task<string> OpenFolderPickerAndGetFirstAsync(FolderPickerOpenOptions options)
        {
            var folders = await OpenFolderPickerAsync(options);
            return folders.Count > 0 ? folders[0].TryGetLocalPath() : null;
        }

        public async Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(FolderPickerOpenOptions options)
        {
            return StorageProvider != null
                ? await StorageProvider.OpenFolderPickerAsync(options)
                : new List<IStorageFolder>();
        }
        public async Task<string> SaveFilePickerAndGetPathAsync(FilePickerSaveOptions options)
        {
            var file = await SaveFilePickerAsync(options);
            return file?.TryGetLocalPath();
        }

        public async Task<IStorageFile> SaveFilePickerAsync(FilePickerSaveOptions options)
        {
            return StorageProvider != null
                ? await StorageProvider.SaveFilePickerAsync(options)
                : null;
        }
        public async Task<IStorageFolder> TryGetWellKnownFolderAsync(WellKnownFolder wellKnownFolder)
        {
            return StorageProvider != null
                ? await StorageProvider.TryGetWellKnownFolderAsync(wellKnownFolder)
                : null;
        }
    }
}
