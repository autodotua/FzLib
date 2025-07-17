namespace FzLib.IO
{
    public interface IFileFilterRule
    {
        string ExcludeFiles { get; set; }
        string ExcludeFolders { get; set; }
        string ExcludePaths { get; set; }
        string IncludeFiles { get; set; }
        string IncludeFolders { get; set; }
        string IncludePaths { get; set; }
        bool UseRegex { get; set; }
    }
}