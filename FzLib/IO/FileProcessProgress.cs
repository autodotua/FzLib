namespace FzLib.IO
{
    public struct FileProcessProgress
    {
        public string SourceFilePath { get; set; }
        public string DestinationFilePath { get; set; }
        public long TotalBytes { get; set; }
        public long BytesCopied { get; set; }
        public double Percentage => TotalBytes > 0 ? (double)BytesCopied / TotalBytes * 100 : 0;
    }
}