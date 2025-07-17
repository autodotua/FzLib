using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FzLib.Collections
{
    public class AutoSortList<T> : IList<T>
    {
        private readonly Func<T, IComparable> compareFunc;
        private readonly List<T> list;
        public AutoSortList(Func<T, IComparable> compareFunc)
        {
            this.compareFunc = compareFunc ?? throw new ArgumentNullException(nameof(compareFunc));
            list = new List<T>();
        }

        public AutoSortList(Func<T, IComparable> compareFunc, IEnumerable<T> items)
        {
            this.compareFunc = compareFunc ?? throw new ArgumentNullException(nameof(compareFunc));
            list = new List<T>(items.OrderBy(p => this.compareFunc(p)));
        }

        public int Count => list.Count;
        public bool IsReadOnly => false;

        public T this[int index]
        {
            get => list[index];
            set => throw new NotSupportedException("Cannot set by index in a sorted list.");
        }

        public void Add(T item)
        {
            if (list.Count == 0)
            {
                list.Add(item);
                return;
            }

            int index = BinarySearch(item);
            if (index < 0)
            {
                index = ~index;
            }
            list.Insert(index, item);
        }

        public void Clear() => list.Clear();

        public bool Contains(T item) => list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(T item) => list.IndexOf(item);

        public void Insert(int index, T item)
        {
            throw new NotSupportedException("不允许通过索引插入到已排序列表中。请使用 Add(T item) 方法。");
        }

        public bool Remove(T item) => list.Remove(item);

        public void RemoveAt(int index) => list.RemoveAt(index);

        public override string ToString()
        {
            return string.Join(", ", list);
        }

        private int BinarySearch(T item)
        {
            int left = 0;
            int right = list.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                int cmp = compareFunc(item).CompareTo(compareFunc(list[mid]));

                if (cmp == 0)
                {
                    return mid;
                }
                else if (cmp < 0)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            return ~left;
        }
    }
}