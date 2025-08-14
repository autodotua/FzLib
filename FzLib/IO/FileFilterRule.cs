using FzLib.Programming;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FzLib.IO;

public class FileFilterRule : NotifyPropertyChangedBase, IFileFilterRule
{
    private static readonly string DefaultExcludeFiles = $"Thumbs.db{Environment.NewLine}Thumb.db{Environment.NewLine}desktop.ini";
    private static readonly string DefaultExcludeFilesR = @"^(Thumbs?\.db)|(desktop.ini)$";
    private static readonly string DefaultExcludeFolders = "$*";
    private static readonly string DefaultExcludeFoldersR = @"\$.*";

    private bool isEnabled = true;

    private string excludeFiles = DefaultExcludeFiles;
    private string excludeFolders = DefaultExcludeFolders;
    private string excludePaths = "";
    private string includeFiles = "*";
    private string includeFolders = "*";
    private string includePaths = "*";
    private bool useRegex;

    public bool IsEnabled
    {
        get => isEnabled;
        set => SetField(ref isEnabled, value);
    }

    public string ExcludeFiles
    {
        get => excludeFiles;
        set => SetField(ref excludeFiles, value);
    }

    public string ExcludeFolders
    {
        get => excludeFolders;
        set => SetField(ref excludeFolders, value);
    }

    public string ExcludePaths
    {
        get => excludePaths;
        set => SetField(ref excludePaths, value);
    }

    public string IncludeFiles
    {
        get => includeFiles;
        set => SetField(ref includeFiles, value);
    }

    public string IncludeFolders
    {
        get => includeFolders;
        set => SetField(ref includeFolders, value);
    }

    public string IncludePaths
    {
        get => includePaths;
        set => SetField(ref includePaths, value);
    }

    public bool UseRegex
    {
        get => useRegex;
        set
        {
            if (SetField(ref useRegex, value))
            {
                OnUseRegexChanged(value);
            }
        }
    }

    private void OnUseRegexChanged(bool value)
    {
        if (value)
        {
            IncludeFiles = IncludeFiles == "*" ? ".*" : IncludeFiles;
            IncludeFolders = IncludeFolders == "*" ? ".*" : IncludeFolders;
            IncludePaths = IncludePaths == "*" ? ".*" : IncludePaths;
            ExcludeFiles = ExcludeFiles == DefaultExcludeFiles ? DefaultExcludeFilesR : ExcludeFiles;
            ExcludeFolders = ExcludeFolders == DefaultExcludeFolders ? DefaultExcludeFoldersR : ExcludeFolders;

            IncludeFiles = IncludeFiles.Replace(Environment.NewLine, "");
            IncludeFolders = IncludeFolders.Replace(Environment.NewLine, "");
            IncludePaths = IncludePaths.Replace(Environment.NewLine, "");
            ExcludeFiles = ExcludeFiles.Replace(Environment.NewLine, "");
            ExcludeFolders = ExcludeFolders.Replace(Environment.NewLine, "");
            ExcludePaths = ExcludePaths.Replace(Environment.NewLine, "");
        }
        else
        {
            IncludeFiles = IncludeFiles == ".*" ? "*" : IncludeFiles;
            IncludeFolders = IncludeFolders == ".*" ? "*" : IncludeFolders;
            IncludePaths = IncludePaths == ".*" ? "*" : IncludePaths;
            ExcludeFiles = ExcludeFiles == DefaultExcludeFilesR ? DefaultExcludeFiles : ExcludeFiles;
            ExcludeFolders = ExcludeFolders == DefaultExcludeFoldersR ? DefaultExcludeFolders : ExcludeFolders;
        }
    }
}
