namespace FzLib.IO
{
    public struct DirectoryProcessProgress
    {
        public string DestinationDirPath { get; set; }
        public string DestinationFilePath { get; set; }
        public long FileProcessedBytes { get; set; }
        public long FileTotalBytes { get; set; }
        public double Percentage => TotalBytes > 0 ? (double)ProcessedBytes / TotalBytes * 100 : 0;
        public long ProcessedBytes { get; set; }
        public string SourceDirPath { get; set; }
        public string SourceFilePath { get; set; }
        public long TotalBytes { get; set; }
    }
}