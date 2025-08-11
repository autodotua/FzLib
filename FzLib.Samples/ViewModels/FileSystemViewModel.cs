using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FzLib.Avalonia.Dialogs.Pickers;
using FzLib.Avalonia.Services;
using FzLib.Cryptography;
using FzLib.IO;
using FzLib.Numeric;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Samples.ViewModels;

public partial class FileSystemViewModel(IStorageProviderService storage) : ObservableObject
{
    [ObservableProperty]
    private string dir1;

    [ObservableProperty]
    private string dir2;

    [ObservableProperty]
    private string file1;

    [ObservableProperty]
    private string file2;

    [ObservableProperty]
    private double fileCopyProgress;

    [ObservableProperty]
    private string message;

    [ObservableProperty]
    private string pickerFiles;

    public IStorageProviderService Storage { get; } = storage;
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
    private async Task CopyFile1ToFile2Async()
    {
        if (string.IsNullOrWhiteSpace(File1) || string.IsNullOrWhiteSpace(File2))
        {
            Message = "请先选择文件";
            return;
        }

        var hash = await FileCopyHelper.CopyFileAsync(File1, File2, progress: new Progress<FileProcessProgress>(p =>
             {
                 FileCopyProgress = p.ProcessedBytes / (double)p.TotalBytes;
                 Message =
                     $"正在复制文件：{Path.GetFileName(p.SourceFilePath)} 到 {Path.GetFileName(p.DestinationFilePath)}（{NumberConverter.ByteToFitString(p.ProcessedBytes)} / {NumberConverter.ByteToFitString(p.TotalBytes)})";
             }), hashAlgorithmType: HashAlgorithmType.SHA1);
        Message = $"文件复制完成：{File1} 到 {File2}，SHA1={BitConverter.ToString(hash)}";
    }

    [RelayCommand]
    private async Task DecryptFile2ToFile1Async()
    {
        if (string.IsNullOrWhiteSpace(File1) || string.IsNullOrWhiteSpace(File2))
        {
            Message = "请先选择文件";
            return;
        }

        var hash = await GetAes().DecryptFileAsync(File2, File1, progress: new Progress<FileProcessProgress>(p =>
             {
                 FileCopyProgress = p.ProcessedBytes / (double)p.TotalBytes;
                 Message =
                     $"正在解密文件：{Path.GetFileName(p.SourceFilePath)} 到 {Path.GetFileName(p.DestinationFilePath)}（{NumberConverter.ByteToFitString(p.ProcessedBytes)} / {NumberConverter.ByteToFitString(p.TotalBytes)})";
             }), hashAlgorithmType: HashAlgorithmType.SHA1);
        Message = $"文件解密完成：{File2} 到 {File1}，SHA1={BitConverter.ToString(hash)}";
    }

    [RelayCommand]
    private async Task EncryptFile1ToFile2Async()
    {
        if (string.IsNullOrWhiteSpace(File1) || string.IsNullOrWhiteSpace(File2))
        {
            Message = "请先选择文件";
            return;
        }

        var hash = await GetAes().EncryptFileAsync(File1, File2, progress: new Progress<FileProcessProgress>(p =>
             {
                 FileCopyProgress = p.ProcessedBytes / (double)p.TotalBytes;
                 Message =
                     $"正在加密文件：{Path.GetFileName(p.SourceFilePath)} 到 {Path.GetFileName(p.DestinationFilePath)}（{NumberConverter.ByteToFitString(p.ProcessedBytes)} / {NumberConverter.ByteToFitString(p.TotalBytes)})";
             }), hashAlgorithmType: HashAlgorithmType.SHA1);
        Message = $"文件加密完成：{File1} 到 {File2}，SHA1={BitConverter.ToString(hash)}";
    }

    private Aes GetAes()
    {
        string password = "12345678";
        Aes aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.SetStringKey(Encoding.UTF8.GetBytes(nameof(FzLib)), password);
        return aes;
    }

    [RelayCommand]
    private async Task GetDecryptedFile2HashAsync()
    {
        if (string.IsNullOrWhiteSpace(File2))
        {
            Message = "请先选择文件";
            return;
        }
        var ok = false;
        var hash = await GetAes().GetDecryptedFileHashAsync(File2, progress: new Progress<FileProcessProgress>(p =>
             {
                 FileCopyProgress = p.ProcessedBytes / (double)p.TotalBytes;
                 if (!ok)
                     Message =
                         $"正在计算Hash：{Path.GetFileName(p.SourceFilePath)}（{NumberConverter.ByteToFitString(p.ProcessedBytes)} / {NumberConverter.ByteToFitString(p.TotalBytes)})";
             }), hashAlgorithmType: HashAlgorithmType.SHA1);
        ok = true;
        Message = $"文件Hash计算完成：{File1} 到 {File2}，SHA1={BitConverter.ToString(hash)}";
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
    private async Task OpenFolderAsync()
    {
        var folders = await Storage.CreatePickerBuilder()
            .AllowMultiple()
            .Title("选择文件夹")
            .OpenFolderPickerAsync();
        PickerFiles = string.Join(Environment.NewLine, folders.Select(f => f.TryGetLocalPath() ?? f.Name));
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
}