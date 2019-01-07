using FzLib.Basic.Collection;
using FzLib.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FzLib.DataAnalysis
{
    public static class Filter
    {
        public static List<FilterResult<T>> MedianValueFilter<T>(IEnumerable<T> items, Func<T, IComparable> valueFunc, int sampleCount, int jump)
        {
            T[] itemArray = items.ToArray();
            if (itemArray.Length < sampleCount)
            {
                itemArray = itemArray.OrderBy(p => valueFunc(p)).ToArray();
                var item = GetMedianItem(itemArray);
                return new List<FilterResult<T>>() { new FilterResult<T>(item, itemArray, valueFunc(item)) };
            }
            List<FilterResult<T>> results = new List<FilterResult<T>>();


            for (int i = sampleCount - 1; i < itemArray.Length; i += jump)
            {
                AutoSortList<T> sortedList = new AutoSortList<T>(valueFunc);
                for (int j = i - sampleCount + 1; j <= i; j++)
                {
                    sortedList.Add(itemArray[j]);
                }
                T medianValue = GetMedianItem(sortedList);
                results.Add(new FilterResult<T>(medianValue, sortedList, valueFunc(medianValue)));
            }

            return results;
        }

        public static T GetMedianItem<T>(IList<T> items)
        {
            int count = items.Count;
            int index = count / 2;
            return items[index];
        }

        public class FilterResult<T>
        {
            public FilterResult(T selectedItem, IEnumerable<T> referenceItems, IComparable value)
            {
                SelectedItem = selectedItem;
                ReferenceItems = referenceItems;
                Value = value;
            }

            public T SelectedItem { get; private set; }
            public IEnumerable<T> ReferenceItems { get; private set; }
            public IComparable Value { get; private set; }
        }
    }
}
