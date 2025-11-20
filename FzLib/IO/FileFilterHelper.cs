using FzLib;
using System.Text.RegularExpressions;

namespace FzLib.IO;

public partial class FileFilterHelper
{
    private static readonly char[] PathSplitter = ['/', '\\'];

    private readonly string[] excludeFiles;

    private readonly string[] excludeFolders;

    private readonly string[] excludePaths;

    private readonly string[] includeFiles;

    private readonly string[] includeFolders;

    private readonly string[] includePaths;

    private readonly Regex rExcludeFiles;

    private readonly Regex rExcludeFolders;

    private readonly Regex rExcludePaths;

    private readonly Regex rIncludeFiles;

    private readonly Regex rIncludeFolders;

    private readonly Regex rIncludePaths;

    private readonly bool useRegex;

    public FileFilterHelper(IFileFilterRule filter)
    {
        useRegex = filter.UseRegex;
        if (filter.UseRegex)
        {
            RegexOptions regexOptions = OperatingSystem.IsWindows() ? RegexOptions.IgnoreCase : RegexOptions.None;
            rIncludeFiles = string.IsNullOrWhiteSpace(filter.IncludeFiles)
                ? AllowAllRegex()
                : new Regex(filter.IncludeFiles, regexOptions);
            rIncludeFolders = string.IsNullOrWhiteSpace(filter.IncludeFolders)
                ? AllowAllRegex()
                : new Regex(filter.IncludeFolders, regexOptions);
            rIncludePaths = string.IsNullOrWhiteSpace(filter.IncludePaths)
                ? AllowAllRegex()
                : new Regex(filter.IncludePaths, regexOptions);
            rExcludeFiles = string.IsNullOrWhiteSpace(filter.ExcludeFiles)
                ? null
                : new Regex(filter.ExcludeFiles, regexOptions);
            rExcludeFolders = string.IsNullOrWhiteSpace(filter.ExcludeFolders)
                ? null
                : new Regex(filter.ExcludeFolders, regexOptions);
            rExcludePaths = string.IsNullOrWhiteSpace(filter.ExcludePaths)
                ? null
                : new Regex(filter.ExcludePaths, regexOptions);
        }
        else
        {
            includeFiles = string.IsNullOrWhiteSpace(filter.IncludeFiles)
                ? ["*"]
                : filter.IncludeFiles.Split(Environment.NewLine);

            includeFolders = string.IsNullOrWhiteSpace(filter.IncludeFolders)
                ? ["*"]
                : filter.IncludeFolders.Split(Environment.NewLine);

            includePaths = string.IsNullOrWhiteSpace(filter.IncludePaths)
                ? ["*"]
                : filter.IncludePaths.Split(Environment.NewLine);

            excludeFiles = string.IsNullOrWhiteSpace(filter.ExcludeFiles)
                ? []
                : filter.ExcludeFiles.Split(Environment.NewLine);

            excludeFolders = string.IsNullOrWhiteSpace(filter.ExcludeFolders)
                ? []
                : filter.ExcludeFolders.Split(Environment.NewLine);

            excludePaths = string.IsNullOrWhiteSpace(filter.ExcludePaths)
                ? []
                : filter.ExcludePaths.Split(Environment.NewLine);
        }
    }

    public bool IsMatched(string path, bool isFile = true)
    {
        if (isFile)
        {
            string name = Path.GetFileName(path);
            if (!IsMatchedName(name))
            {
                return false;
            }
        }

        path = path.Replace('\\', '/');
        if (!IsMatchedPath(path))
        {
            return false;
        }

        string[] folders = isFile
            ? Path.GetDirectoryName(path)?.Split(PathSplitter, StringSplitOptions.RemoveEmptyEntries)
            : path.Split(PathSplitter, StringSplitOptions.RemoveEmptyEntries);
        return IsMatchedFolder(folders);
    }

    public bool IsMatched(FileSystemInfo file)
    {
        return IsMatched(file.FullName, !file.Attributes.HasFlag(FileAttributes.Directory));
    }

    [GeneratedRegex(".*")]
    private static partial Regex AllowAllRegex();

    /// <summary>
    /// 支持通配符（*表示0个或多个任意字符，?表示1个任意字符）的字符串匹配
    /// </summary>
    /// <param name="text">字符串</param>
    /// <param name="pattern">匹配模式</param>
    /// <returns></returns>
    public static bool IsMatchedByPattern(string text, string pattern)
    {
        // 判断是否在 Windows 系统上运行
        if (OperatingSystem.IsWindows())
        {
            text = text.ToLower(); // 转换文本为小写
            pattern = pattern.ToLower(); // 转换模式为小写
        }

        int textLen = text.Length; // 文本长度
        int patternLen = pattern.Length; // 模式长度

        bool[] dp = new bool[patternLen + 1]; // 创建一维 DP 数组
        dp[0] = true; // 空模式与空文本匹配

        // 初始化 DP 数组：处理模式以 '*' 开头的情况
        for (int j = 1; j <= patternLen; j++)
        {
            if (pattern[j - 1] == '*') // 如果模式字符是 '*'，则当前 dp[j] 可以继承 dp[j-1]
            {
                dp[j] = dp[j - 1];
            }
        }

        // 逐字符遍历文本和模式，更新 DP 数组
        for (int i = 1; i <= textLen; i++)
        {
            bool prev = dp[0]; // 存储 dp[i-1][j-1]（上一行的状态）
            dp[0] = false; // dp[i][0] 表示文本非空时无法与空模式匹配

            for (int j = 1; j <= patternLen; j++)
            {
                bool temp = dp[j]; // 暂存 dp[i-1][j]，以备下次迭代使用

                if (pattern[j - 1] == text[i - 1] || pattern[j - 1] == '?') // 模式字符匹配文本字符或模式为 '?'
                {
                    dp[j] = prev; // 更新 dp[i][j] = dp[i-1][j-1]
                }
                else if (pattern[j - 1] == '*') // 模式字符为 '*'，表示匹配任意数量字符
                {
                    dp[j] = dp[j] || dp[j - 1]; // dp[i][j] = dp[i-1][j] 或 dp[i][j-1]
                }
                else
                {
                    dp[j] = false; // 模式与文本字符不匹配
                }

                prev = temp; // 更新 prev 为 dp[i-1][j]，用于下一次迭代
            }
        }

        return dp[patternLen]; // 返回最终结果，表示文本是否与模式完全匹配
    }

    private bool IsMatchedFolder(params string[] folders)
    {
        if (useRegex)
        {
            return folders.All(folder => !IsMatchesRegex(rExcludeFolders, folder))
                   && folders.Any(folder => IsMatchesRegex(rIncludeFolders, folder));
        }

        //每个目录的一部分都与每个黑名单不匹配，且任意目录的一部分与任意白名单匹配
        return folders.All(folder => excludeFolders.All(p => !IsMatchedByPattern(folder, p)))
               && folders.Any(folder => includeFolders.Any(p => IsMatchedByPattern(folder, p)));
    }

    private bool IsMatchedName(string fileName)
    {
        ArgumentException.ThrowIfNullOrEmpty(fileName);

        if (useRegex)
        {
            return !IsMatchesRegex(rExcludeFiles, fileName)
                   && IsMatchesRegex(rIncludeFiles, fileName);
        }

        return excludeFiles.All(p => !IsMatchedByPattern(fileName, p))
               && includeFiles.Any(p => IsMatchedByPattern(fileName, p));
    }

    private bool IsMatchedPath(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);

        if (useRegex)
        {
            return !IsMatchesRegex(rExcludePaths, path)
                   && IsMatchesRegex(rIncludePaths, path);
        }

        return excludePaths.All(p => !IsMatchedByPattern(path, p))
               && includePaths.Any(p => IsMatchedByPattern(path, p));
    }

    private bool IsMatchesRegex(Regex regex, string input)
    {
        return regex != null && regex.IsMatch(input);
    }
}