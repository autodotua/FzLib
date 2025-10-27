using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace FzLib.Avalonia.Services;

public interface IStorageProviderServicePickerBuilder
{
    /// <summary>
    /// 添加“全部文件”过滤器
    /// </summary>
    IStorageProviderServicePickerBuilder AddAllFilesFilter(string name = "全部文件");

    /// <summary>
    /// 添加文件类型过滤器（按名称和扩展名）
    /// </summary>
    IStorageProviderServicePickerBuilder AddFilter(string name, params string[] extensions);
    
    /// <summary>
    /// 手动添加完整 Filter（支持 Apple UTI、MIME 类型等）
    /// </summary>
    IStorageProviderServicePickerBuilder AddFilter(
        string name,
        IReadOnlyList<string> patterns,
        IReadOnlyList<string> appleUniformTypeIdentifiers = null,
        IReadOnlyList<string> mimeTypes = null);

    /// <summary>
    /// 是否允许多选
    /// </summary>
    IStorageProviderServicePickerBuilder AllowMultiple(bool allow = true);

    /// <summary>
    /// 控制覆盖提示（保存时）
    /// </summary>
    IStorageProviderServicePickerBuilder ShowOverwritePrompt(bool show = true);

    /// <summary>
    /// 设置建议文件名
    /// </summary>
    IStorageProviderServicePickerBuilder SuggestedFileName(string name);

    /// <summary>
    /// 设置起始位置目录
    /// </summary>
    IStorageProviderServicePickerBuilder SuggestedStartLocation(IStorageFolder folder);

    /// <summary>
    /// 设置标题
    /// </summary>
    IStorageProviderServicePickerBuilder Title(string title);
    
    /// <summary>
    /// 打开文件选择器并获取第一个选择的文件路径
    /// </summary>
    Task<string> OpenFilePickerAndGetFirstAsync();

    /// <summary>
    /// 打开文件选择器并获取所有选择的文件
    /// </summary>
    Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync();

    /// <summary>
    /// 打开文件夹选择器并获取第一个选择的文件夹路径
    /// </summary>
    Task<string> OpenFolderPickerAndGetFirstAsync();

    /// <summary>
    /// 打开文件夹选择器并获取所有选择的文件夹
    /// </summary>
    Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync();

    /// <summary>
    /// 打开保存文件选择器并获取保存路径
    /// </summary>
    Task<string> SaveFilePickerAndGetPathAsync();

    /// <summary>
    /// 打开保存文件选择器并获取保存的文件
    /// </summary>
    Task<IStorageFile> SaveFilePickerAsync();
}
