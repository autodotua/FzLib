using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FzLib.IO;
using FzLib.Numeric;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using FzLib.Avalonia.Dialogs.Pickers;
using FzLib.Avalonia.Services;

namespace FzLib.Samples.ViewModels;

public partial class FileSystemViewModel(IStorageProviderService storage) : ObservableObject
{
    public IStorageProviderService Storage { get; } = storage;

    [ObservableProperty]
    private string message;

    [ObservableProperty]
    private string file1;

    [ObservableProperty]
    private string file2;

    [ObservableProperty]
    private string dir1;

    [ObservableProperty]
    private string dir2;

    [ObservableProperty]
    private double fileCopyProgress;

    [ObservableProperty]
    private string pickerFiles;

    [RelayCommand]
    private async Task CopyFile1ToFile2Async()
    {
        if (string.IsNullOrWhiteSpace(File1) || string.IsNullOrWhiteSpace(File2))
        {
            Message = "请先选择文件";
            return;
        }

        await FileCopyHelper.CopyFileAsync(File1, File2, progress: new Progress<FileProcessProgress>(p =>
        {
            FileCopyProgress = p.ProcessedBytes / (double)p.TotalBytes;
            Message =
                $"正在复制文件：{Path.GetFileName(p.SourceFilePath)} 到 {Path.GetFileName(p.DestinationFilePath)}（{NumberConverter.ByteToFitString(p.ProcessedBytes)} / {NumberConverter.ByteToFitString(p.TotalBytes)})";
        }));
        Message = $"文件复制完成：{File1} 到 {File2}";
    }

    [RelayCommand]
    private async Task CopyDir1ToDir2()
    {
        if (string.IsNullOrWhiteSpace(Dir1) || string.IsNullOrWhiteSpace(Dir2))
        {
            Message = "请先选择目录";
            return;
        }

        await FileCopyHelper.CopyDirectoryAsync(Dir1, Dir2, progress: new Progress<DirectoryProcessProgress>(p =>
        {
            FileCopyProgress = p.ProcessedBytes / (double)p.TotalBytes;
            Message =
                $"正在复制文件：{Path.GetFileName(p.SourceFilePath)} 到 {Path.GetFileName(p.DestinationFilePath)}（{NumberConverter.ByteToFitString(p.FileProcessedBytes)} / {NumberConverter.ByteToFitString(p.FileTotalBytes)})";
        }));
        Message = $"目录复制完成：{Dir1} 到 {Dir2}";
    }

    [RelayCommand]
    private async Task OpenFile1Async()
    {
        var files = await Storage.OpenFilePickerAsync(FilePickerOptionsBuilder.Create()
            .AddFilter("文本文件", "txt", "md")
            .AddAllFilesFilter()
            .AllowMultiple()
            .BuildOpenOptions());

        PickerFiles = string.Join(Environment.NewLine, files.Select(f => f.TryGetLocalPath() ?? f.Name));
    }

    [RelayCommand]
    private async Task OpenFile2Async()
    {
        PickerFiles = await Storage.CreatePickerBuilder()
            .AddFilter("图片文件", "jpg", "jpeg", "png", "gif")
            .AddAllFilesFilter()
            .OpenFilePickerAndGetFirstAsync();
    }

    [RelayCommand]
    private async Task SaveFileAsync()
    {
        PickerFiles = await Storage.CreatePickerBuilder()
            .Title("保存音频")
            .AddFilter("音频文件", "mp3", "wav", "flac")
            .SuggestedStartLocation(await Storage.TryGetWellKnownFolderAsync(WellKnownFolder.Music))
            .SuggestedFileName("新音频文件.mp3")
            .AddAllFilesFilter()
            .SaveFilePickerAndGetPathAsync();
    }

    [RelayCommand]
    private async Task OpenFolderAsync()
    {
        var folders = await Storage.CreatePickerBuilder()
            .AllowMultiple()
            .Title("选择文件夹")
            .OpenFolderPickerAsync();
        PickerFiles = string.Join(Environment.NewLine, folders.Select(f => f.TryGetLocalPath() ?? f.Name));
    }
}