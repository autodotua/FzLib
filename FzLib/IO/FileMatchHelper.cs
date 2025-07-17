using System.Collections.Immutable;
using FzLib.IO;

namespace FzLib.IO;

public class FileMatchHelper
{
    private static readonly IReadOnlySet<string> EmptyStringSet = ImmutableHashSet<string>.Empty;
    private static readonly EqualityComparer<FileInfo> FileInfoComparer
        = EqualityComparer<FileInfo>.Create((f1, f2) => f1.FullName == f2.FullName
            , f => f.FullName.GetHashCode());

    private Dictionary<string, List<string>> hash2File = new();
    private Dictionary<long, List<string>> length2File = new();
    private Dictionary<string, List<string>> name2File = new();
    private Dictionary<DateTime, List<string>> time2File = new();
    public FileMatchHelper(bool byName, bool byLength, bool byTime, int timeToleranceSecond = 0)
    {
        if ((byName ? 1 : 0) + (byLength ? 1 : 0) + (byTime ? 1 : 0) < 2)
        {
            throw new ArgumentException("至少选择两个匹配方式");
        }

        if (timeToleranceSecond is < 0 or > 60)
        {
            throw new ArgumentException("时间范围应当在0-60秒之间", nameof(timeToleranceSecond));
        }

        ByName = byName;
        ByLength = byLength;
        ByTime = byTime;
        TimeToleranceSecond = timeToleranceSecond;
    }

    public bool ByLength { get; }
    public bool ByName { get; }
    public bool ByTime { get; }
    public int TimeToleranceSecond { get; }
    public void AddReferenceDir(string dir, CancellationToken cancellationToken = default)
    {
        foreach (var file in new DirectoryInfo(dir).EnumerateFiles(cancellationToken: cancellationToken))
        {
            AddFeatureIf(ByName, name2File, file.Name, file.FullName);
            AddFeatureIf(ByLength, length2File, file.Length, file.FullName);
            AddFeatureIf(ByTime, time2File, TruncateDateTime(file.LastWriteTime), file.FullName);
        }
    }

    public void Clear()
    {
        name2File.Clear();
        length2File.Clear();
        time2File.Clear();
        hash2File.Clear();
    }
    public IReadOnlySet<string> GetMatchedFiles(FileInfo file)
    {
        HashSet<string> result = null;
        if (ByName)
        {
            if (name2File.TryGetValue(file.Name, out var list))
            {
                result = new HashSet<string>(list);
            }
            else
            {
                return EmptyStringSet;
            }
        }

        if (ByLength)
        {
            if (length2File.TryGetValue(file.Length, out var list))
            {
                if (result == null)
                {
                    result = new HashSet<string>(list);
                }
                else
                {
                    result.IntersectWith(list);
                }

                if (result.Count == 0)
                {
                    return EmptyStringSet;
                }
            }
            else
            {
                return EmptyStringSet;
            }
        }

        if (ByTime)
        {
            var fileTime = TruncateDateTime(file.LastWriteTime);
            var endTime = fileTime.AddSeconds(TimeToleranceSecond);
            HashSet<string> possibleTimeFiles = new HashSet<string>();
            for (DateTime time = fileTime.AddSeconds(-TimeToleranceSecond); time <= endTime; time = time.AddSeconds(1))
            {
                if (time2File.TryGetValue(time, out var list))
                {
                    foreach (var matchedFile in list)
                    {
                        possibleTimeFiles.Add(matchedFile);
                    }
                }
            }

            if (possibleTimeFiles.Count == 0)
            {
                return EmptyStringSet;
            }

            if (result == null)
            {
                result = new HashSet<string>(possibleTimeFiles);
            }
            else
            {
                result.IntersectWith(possibleTimeFiles);
            }

            if (result.Count == 0)
            {
                return EmptyStringSet;
            }
        }

        return result ?? EmptyStringSet;
    }

    private void AddFeature<TKey, TItem>(Dictionary<TKey, List<TItem>> dic, TKey key, TItem item)
    {
        if (dic.ContainsKey(key))
        {
            dic[key].Add(item);
        }
        else
        {
            dic.Add(key, [item]);
        }
    }

    private void AddFeatureIf<TKey, TItem>(bool condition, Dictionary<TKey, List<TItem>> dic, TKey key, TItem item)
    {
        if (condition)
        {
            AddFeature(dic, key, item);
        }
    }

    private DateTime TruncateDateTime(DateTime dateTime, long resolution = TimeSpan.TicksPerSecond)
    {
        return new DateTime(dateTime.Ticks - dateTime.Ticks % resolution, dateTime.Kind);
    }
}