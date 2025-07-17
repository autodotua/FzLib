using System;
using System.Collections.Generic;
using System.Linq;

namespace FzLib.Collections
{
    /// <summary>
    /// 提供 <see cref="ICollection{T}"/> 的扩展方法
    /// </summary>
    public static class CollectionExtension
    {

        /// <summary>
        /// 检查集合是否为 null 或空集合
        /// </summary>
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }


        /// <summary>
        /// 移除所有符合条件的元素
        /// </summary>
        public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            if (collection.IsEmpty()) return;
            var itemsToRemove = collection.Where(predicate).ToList();
            foreach (var item in itemsToRemove)
            {
                collection.Remove(item);
            }
        }
    }
}