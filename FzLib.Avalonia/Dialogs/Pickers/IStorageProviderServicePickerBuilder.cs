using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace FzLib.Avalonia.Dialogs.Pickers;

public interface IStorageProviderServicePickerBuilder
{
    // 添加“全部文件”过滤器
    IStorageProviderServicePickerBuilder AddAllFilesFilter(string name = "全部文件");

    // 添加文件类型过滤器（按名称和扩展名）
    IStorageProviderServicePickerBuilder AddFilter(string name, params string[] extensions);
    
    // 手动添加完整 Filter（支持 Apple UTI、MIME 类型等）
    IStorageProviderServicePickerBuilder AddFilter(
        string name,
        IReadOnlyList<string> patterns,
        IReadOnlyList<string>? appleUniformTypeIdentifiers = null,
        IReadOnlyList<string>? mimeTypes = null);

    // 是否允许多选
    IStorageProviderServicePickerBuilder AllowMultiple(bool allow = true);

    // 控制覆盖提示（保存时）
    IStorageProviderServicePickerBuilder ShowOverwritePrompt(bool show = true);

    // 设置建议文件名
    IStorageProviderServicePickerBuilder SuggestedFileName(string name);

    // 设置起始位置目录
    IStorageProviderServicePickerBuilder SuggestedStartLocation(IStorageFolder folder);
    // 设置标题
    IStorageProviderServicePickerBuilder Title(string title);
    
    
    Task<string> OpenFilePickerAndGetFirstAsync();

    Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync();

    Task<string> OpenFolderPickerAndGetFirstAsync();

    Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync();

    Task<string> SaveFilePickerAndGetPathAsync();

    Task<IStorageFile> SaveFilePickerAsync();

}