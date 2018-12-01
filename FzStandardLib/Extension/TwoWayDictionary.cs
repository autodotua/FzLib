using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Extension
{
    public class TwoWayDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public TwoWayDictionary() : base()
        {

        }
        public TwoWayDictionary(IDictionary<TKey, TValue> dictionary)
        {
            foreach (var item in dictionary)
            {
                Add(item.Key,item.Value);
            }
        }


        public new void Add(TKey value1, TValue value2)
        {
            if (ReverseDictionary.ContainsKey(value2))
            {
                throw new ArgumentException("已经有相同的值了");
            }
            base.Add(value1, value2);
            ReverseDictionary.Add(value2, value1);

        }
        
        public new void Remove(TKey item)
        {
            TValue value = this[item];
            base.Remove(item);
            ReverseDictionary.Remove(value);
        }
        public TKey GetKey(TValue value)
        {
            return ReverseDictionary[value];
        }
        public new bool ContainsValue(TValue value)
        {
            return ReverseDictionary.ContainsKey(value);
        }

        private Dictionary<TValue, TKey> ReverseDictionary { get; } = new Dictionary<TValue, TKey>();
    }
}
