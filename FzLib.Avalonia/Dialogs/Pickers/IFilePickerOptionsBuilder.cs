using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace FzLib.Avalonia.Dialogs.Pickers;

public interface IFilePickerOptionsBuilder
{
    // 添加“全部文件”过滤器
    IFilePickerOptionsBuilder AddAllFilesFilter(string name = "全部文件");

    // 添加文件类型过滤器（按名称和扩展名）
    IFilePickerOptionsBuilder AddFilter(string name, params string[] extensions);
    // 手动添加完整 Filter（支持 Apple UTI、MIME 类型等）
    IFilePickerOptionsBuilder AddFilter(
        string name,
        IReadOnlyList<string> patterns,
        IReadOnlyList<string> appleUniformTypeIdentifiers = null,
        IReadOnlyList<string> mimeTypes = null);

    // 是否允许多选
    IFilePickerOptionsBuilder AllowMultiple(bool allow = true);

    // 构建选择文件夹的选项
    FolderPickerOpenOptions BuildFolderOptions();

    // 构建打开文件选项
    FilePickerOpenOptions BuildOpenOptions();

    // 构建保存文件选项
    FilePickerSaveOptions BuildSaveOptions();

    // 控制覆盖提示（保存时）
    IFilePickerOptionsBuilder ShowOverwritePrompt(bool show = true);

    // 设置建议文件名
    IFilePickerOptionsBuilder SuggestedFileName(string name);

    // 设置起始位置目录
    IFilePickerOptionsBuilder SuggestedStartLocation(IStorageFolder folder);
    // 设置标题
    IFilePickerOptionsBuilder Title(string title);
}