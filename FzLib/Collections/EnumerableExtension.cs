using System;
using System.Collections.Generic;
using System.Linq;

namespace FzLib.Collections
{
    public static class EnumerableExtension
    {
        /// <summary>
        /// 将集合分块处理
        /// </summary>
        /// <param name="batchSize">每块的大小</param>
        public static IEnumerable<List<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            List<T> batch = new List<T>(batchSize);
            foreach (var item in source)
            {
                batch.Add(item);
                if (batch.Count == batchSize)
                {
                    yield return batch;
                    batch = new List<T>(batchSize);
                }
            }

            if (batch.Count > 0)
            {
                yield return batch;
            }
        }

        /// <summary>
        /// 将集合元素拼接为字符串
        /// </summary>
        /// <param name="separator">分隔符</param>
        /// <param name="formatter">元素格式化方法</param>
        public static string JoinToString<T>(this IEnumerable<T> source, string separator = ", ",
            Func<T, string> formatter = null)
        {
            if (source == null) return string.Empty;
            formatter = formatter ?? (x => x?.ToString() ?? "null");
            return string.Join(separator, source.Select(formatter));
        }

        public static async Task<T> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> source)
        {
            T result = default;
            await foreach (var item in source)
            {
                result = item;
                break;
            }

            return result;
        }

        public static async Task<T> FirstAsync<T>(this IAsyncEnumerable<T> source)
        {
            await foreach (var item in source)
            {
                return item;
            }

            throw new InvalidOperationException("集合为空");
        }

        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source)
        {
            List<T> list = new List<T>();
            await foreach (var item in source)
            {
                list.Add(item);
            }
            return list;
        }
    }
}