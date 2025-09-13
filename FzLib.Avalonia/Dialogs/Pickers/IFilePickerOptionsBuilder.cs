using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace FzLib.Avalonia.Dialogs.Pickers;

public interface IFilePickerOptionsBuilder
{
    /// <summary>
    /// 添加“全部文件”过滤器
    /// </summary>
    IFilePickerOptionsBuilder AddAllFilesFilter(string name = "全部文件");

    /// <summary>
    /// 添加文件类型过滤器（按名称和扩展名）
    /// </summary>
    IFilePickerOptionsBuilder AddFilter(string name, params string[] extensions);

    /// <summary>
    /// 手动添加完整 Filter（支持 Apple UTI、MIME 类型等）
    /// </summary>
    IFilePickerOptionsBuilder AddFilter(
        string name,
        IReadOnlyList<string> patterns,
        IReadOnlyList<string> appleUniformTypeIdentifiers = null,
        IReadOnlyList<string> mimeTypes = null);

    /// <summary>
    /// 是否允许多选
    /// </summary>
    IFilePickerOptionsBuilder AllowMultiple(bool allow = true);

    /// <summary>
    /// 构建选择文件夹的选项
    /// </summary>
    FolderPickerOpenOptions BuildFolderOptions();

    /// <summary>
    /// 构建打开文件选项
    /// </summary>
    FilePickerOpenOptions BuildOpenOptions();

    /// <summary>
    /// 构建保存文件选项
    /// </summary>
    FilePickerSaveOptions BuildSaveOptions();

    /// <summary>
    /// 控制覆盖提示（保存时）
    /// </summary>
    IFilePickerOptionsBuilder ShowOverwritePrompt(bool show = true);

    /// <summary>
    /// 设置建议文件名
    /// </summary>
    IFilePickerOptionsBuilder SuggestedFileName(string name);

    /// <summary>
    /// 设置起始位置目录
    /// </summary>
    IFilePickerOptionsBuilder SuggestedStartLocation(IStorageFolder folder);
    
    /// <summary>
    /// 设置标题
    /// </summary>
    IFilePickerOptionsBuilder Title(string title);
}