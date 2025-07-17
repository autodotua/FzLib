using System;
using System.Collections.Generic;
using System.Linq;

namespace FzLib.Collections
{

    /// <summary>
    /// 提供 <see cref="IList{T}"/> 的扩展方法
    /// </summary>
    public static class ListExtension
    {
        private static readonly Random rng = new Random();

        /// <summary>
        /// 将列表分块处理
        /// </summary>
        /// <param name="batchSize">每块的大小</param>
        public static IEnumerable<List<T>> Batch<T>(this IList<T> list, int batchSize)
        {
            for (int i = 0; i < list.Count; i += batchSize)
            {
                yield return list.Slice(i, Math.Min(batchSize, list.Count - i));
            }
        }

        /// <summary>
        /// 检查列表是否包含指定集合的所有元素
        /// </summary>
        public static bool ContainsAll<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list.IsEmpty() || items == null) return false;
            return items.All(list.Contains);
        }

        /// <summary>
        /// 检查列表是否包含指定集合的任意元素
        /// </summary>
        public static bool ContainsAny<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list.IsEmpty() || items == null) return false;
            return items.Any(list.Contains);
        }

        /// <summary>
        /// 检查列表是否以指定子列表结尾
        /// </summary>
        public static bool EndsWith<T>(this IList<T> source, IList<T> suffix)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (suffix == null) throw new ArgumentNullException(nameof(suffix));

            if (suffix.Count == 0) return true;
            if (source.Count < suffix.Count) return false;

            var comparer = EqualityComparer<T>.Default;
            int startIndex = source.Count - suffix.Count;

            for (int i = 0; i < suffix.Count; i++)
            {
                if (!comparer.Equals(source[startIndex + i], suffix[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 查找第一个符合条件的元素索引
        /// </summary>
        public static int FindIndex<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 查找最后一个符合条件的元素索引
        /// </summary>
        public static int FindLastIndex<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (predicate(list[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 获取列表最后一个元素的索引
        /// </summary>
        public static int LastIndex<T>(this IList<T> list) => list.Count - 1;

        /// <summary>
        /// 移除所有符合条件的元素
        /// </summary>
        public static void RemoveAll<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list.IsEmpty()) return;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (predicate(list[i]))
                    list.RemoveAt(i);
            }
        }

        /// <summary>
        /// 安全获取元素，避免索引越界
        /// </summary>
        /// <param name="defaultValue">当索引无效时返回的默认值</param>
        public static T SafeGet<T>(this IList<T> list, int index, T defaultValue = default)
        {
            if (list == null || index < 0 || index >= list.Count)
                return defaultValue;
            return list[index];
        }

        /// <summary>
        /// 随机打乱列表元素顺序
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            if (list.IsEmpty()) return;
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                list.Swap(i, j);
            }
        }

        /// <summary>
        /// 获取列表的子集
        /// </summary>
        /// <param name="start">起始索引</param>
        /// <param name="count">元素数量</param>
        public static List<T> Slice<T>(this IList<T> list, int start, int count)
        {
            if (list.IsEmpty() || start < 0 || count <= 0 || start >= list.Count)
                return new List<T>();

            count = Math.Min(count, list.Count - start);
            return list.Skip(start).Take(count).ToList();
        }

        /// <summary>
        /// 交换列表中两个元素的位置
        /// </summary>
        public static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            if (list == null || index1 < 0 || index2 < 0 ||
                index1 >= list.Count || index2 >= list.Count)
                return;
            (list[index1], list[index2]) = (list[index2], list[index1]);
        }
    }
}