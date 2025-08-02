using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using FzLib.Avalonia.Services;

namespace FzLib.Avalonia.Dialogs.Pickers;

public class FilePickerOptionsBuilder : IFilePickerOptionsBuilder, IStorageProviderServicePickerBuilder
{
    private readonly List<FilePickerFileType> filters = new();

    private bool allowMultiple = false;

    private bool showOverwritePrompt = true;

    private string suggestedFileName;

    private IStorageFolder suggestedStartLocation;

    private string title;
    internal FilePickerOptionsBuilder(IStorageProviderService service)
    {
        Service = service;
    }

    private FilePickerOptionsBuilder()
    {
    }

    internal IStorageProviderService Service { get; }

    public static IFilePickerOptionsBuilder Create()
    {
        return new FilePickerOptionsBuilder();
    }

    IFilePickerOptionsBuilder IFilePickerOptionsBuilder.AddAllFilesFilter(string name = "全部文件")
    {
        return AddAllFilesFilter(name);
    }

    IStorageProviderServicePickerBuilder IStorageProviderServicePickerBuilder.AddAllFilesFilter(string name = "全部文件")
    {
        return AddAllFilesFilter(name);
    }

    IFilePickerOptionsBuilder IFilePickerOptionsBuilder.AddFilter(string name, params string[] extensions)
    {
        return AddFilter(name, extensions);
    }

    IStorageProviderServicePickerBuilder IStorageProviderServicePickerBuilder.AddFilter(string name,
        params string[] extensions)
    {
        return AddFilter(name, extensions);
    }

    IFilePickerOptionsBuilder IFilePickerOptionsBuilder.AddFilter(string name, IReadOnlyList<string> patterns,
        IReadOnlyList<string> appleUniformTypeIdentifiers, IReadOnlyList<string> mimeTypes)
    {
        return AddFilter(name, patterns, appleUniformTypeIdentifiers, mimeTypes);
    }

    IStorageProviderServicePickerBuilder IStorageProviderServicePickerBuilder.AddFilter(string name,
        IReadOnlyList<string> patterns,
        IReadOnlyList<string> appleUniformTypeIdentifiers, IReadOnlyList<string> mimeTypes)
    {
        return AddFilter(name, patterns, appleUniformTypeIdentifiers, mimeTypes);
    }

    IFilePickerOptionsBuilder IFilePickerOptionsBuilder.AllowMultiple(bool allow = true)
    {
        return AllowMultiple(allow);
    }

    IStorageProviderServicePickerBuilder IStorageProviderServicePickerBuilder.AllowMultiple(bool allow = true)
    {
        return AllowMultiple(allow);
    }

    public FolderPickerOpenOptions BuildFolderOptions()
    {
        var options = new FolderPickerOpenOptions
        {
            Title = title,
            SuggestedStartLocation = suggestedStartLocation,
            AllowMultiple = true
        };

        return options;
    }

    public FilePickerOpenOptions BuildOpenOptions()
    {
        var options = new FilePickerOpenOptions
        {
            Title = title,
            AllowMultiple = allowMultiple,
            FileTypeFilter = filters,
            SuggestedFileName = suggestedFileName,
            SuggestedStartLocation = suggestedStartLocation
        };

        return options;
    }

    public FilePickerSaveOptions BuildSaveOptions()
    {
        var options = new FilePickerSaveOptions
        {
            Title = title,
            FileTypeChoices = filters,
            SuggestedFileName = suggestedFileName,
            ShowOverwritePrompt = showOverwritePrompt,
            SuggestedStartLocation = suggestedStartLocation
        };

        return options;
    }


    public Task<string> OpenFilePickerAndGetFirstAsync()
    {
        return Service.OpenFilePickerAndGetFirstAsync(BuildOpenOptions());
    }

    public Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync()
    {
        return Service.OpenFilePickerAsync(BuildOpenOptions());
    }

    public Task<string> OpenFolderPickerAndGetFirstAsync()
    {
        return Service.OpenFolderPickerAndGetFirstAsync(BuildFolderOptions());
    }

    public Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync()
    {
        return Service.OpenFolderPickerAsync(BuildFolderOptions());
    }

    public Task<string> SaveFilePickerAndGetPathAsync()
    {
        return Service.SaveFilePickerAndGetPathAsync(BuildSaveOptions());
    }

    public Task<IStorageFile> SaveFilePickerAsync()
    {
        return Service.SaveFilePickerAsync(BuildSaveOptions());
    }

    IFilePickerOptionsBuilder IFilePickerOptionsBuilder.ShowOverwritePrompt(bool show = true)
    {
        return ShowOverwritePrompt(show);
    }

    IStorageProviderServicePickerBuilder IStorageProviderServicePickerBuilder.ShowOverwritePrompt(bool show = true)
    {
        return ShowOverwritePrompt(show);
    }

    IFilePickerOptionsBuilder IFilePickerOptionsBuilder.SuggestedFileName(string name)
    {
        return SuggestedFileName(name);
    }

    IStorageProviderServicePickerBuilder IStorageProviderServicePickerBuilder.SuggestedFileName(string name)
    {
        return SuggestedFileName(name);
    }

    IFilePickerOptionsBuilder IFilePickerOptionsBuilder.SuggestedStartLocation(IStorageFolder folder)
    {
        return SuggestedStartLocation(folder);
    }

    IStorageProviderServicePickerBuilder IStorageProviderServicePickerBuilder.SuggestedStartLocation(
        IStorageFolder folder)
    {
        return SuggestedStartLocation(folder);
    }

    IFilePickerOptionsBuilder IFilePickerOptionsBuilder.Title(string title)
    {
        return Title(title);
    }
    IStorageProviderServicePickerBuilder IStorageProviderServicePickerBuilder.Title(string title)
    {
        return Title(title);
    }

    private FilePickerOptionsBuilder AddAllFilesFilter(string name = "全部文件")
    {
        return AddFilter(name, FilePickerFileTypes.All.Patterns, FilePickerFileTypes.All.AppleUniformTypeIdentifiers,
            FilePickerFileTypes.All.MimeTypes);
    }

    private FilePickerOptionsBuilder AddFilter(string name, params string[] extensions)
    {
        List<string> patterns = extensions
            .Where(ext => !string.IsNullOrWhiteSpace(ext))
            .Select(ext => $"*.{ext.TrimStart('.')}")
            .ToList();
        return AddFilter(name, patterns, null, null);
    }
    
    private FilePickerOptionsBuilder AddFilter(string name, IReadOnlyList<string> patterns,
        IReadOnlyList<string> appleUniformTypeIdentifiers, IReadOnlyList<string> mimeTypes)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new System.ArgumentException("Filter名称不能为null或空字符串。", nameof(name));
        }

        if (patterns == null || patterns.Count == 0)
        {
            throw new System.ArgumentException("Filter模式不能为null或空字符串。", nameof(name));
        }

        foreach (var pattern in patterns)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new System.ArgumentException("Filter模式不能包含空字符串。", nameof(name));
            }

            if (pattern.Length < 2 || !pattern.StartsWith('*') || pattern[1] != '.')
            {
                throw new System.ArgumentException("Filter模式必须以'*.'开头。", nameof(name));
            }
        }

        FilePickerFileType filter = new FilePickerFileType(name)
        {
            Patterns = patterns,
            AppleUniformTypeIdentifiers = appleUniformTypeIdentifiers,
            MimeTypes = mimeTypes
        };
        filters.Add(filter);
        return this;
    }
    
    private FilePickerOptionsBuilder AllowMultiple(bool allow = true)
    {
        allowMultiple = allow;
        return this;
    }
    
    private FilePickerOptionsBuilder ShowOverwritePrompt(bool show = true)
    {
        showOverwritePrompt = show;
        return this;
    }

    private FilePickerOptionsBuilder SuggestedFileName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new System.ArgumentException("建议的文件名不能为null或空字符串。", nameof(name));
        }

        suggestedFileName = name;
        return this;
    }
    
    private FilePickerOptionsBuilder SuggestedStartLocation(IStorageFolder folder)
    {
        suggestedStartLocation = folder ?? throw new System.ArgumentNullException(nameof(folder), "建议的起始位置不能为null。");
        return this;
    }
    
    private FilePickerOptionsBuilder Title(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new System.ArgumentException("标题不能为null或空字符串。", nameof(title));
        }

        this.title = title;
        return this;
    }    
    
}