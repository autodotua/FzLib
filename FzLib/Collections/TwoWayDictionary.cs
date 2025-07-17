using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FzLib.Collections
{
    public class TwoWayDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> forwardDictionary;
        private readonly Dictionary<TValue, TKey> reverseDictionary;

        public TwoWayDictionary()
        {
            forwardDictionary = new Dictionary<TKey, TValue>();
            reverseDictionary = new Dictionary<TValue, TKey>();
        }

        public TwoWayDictionary(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            forwardDictionary = new Dictionary<TKey, TValue>(keyComparer);
            reverseDictionary = new Dictionary<TValue, TKey>(valueComparer);
        }

        public int Count
        {
            get { return forwardDictionary.Count; }
        }

        public IReadOnlyDictionary<TKey, TValue> ForwardDictionary
        {
            get { return forwardDictionary; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public ICollection<TKey> Keys
        {
            get { return forwardDictionary.Keys; }
        }

        public IReadOnlyDictionary<TValue, TKey> ReverseDictionary
        {
            get { return reverseDictionary; }
        }
        public ICollection<TValue> Values
        {
            get { return forwardDictionary.Values; }
        }

        public TValue this[TKey key]
        {
            get
            {
                return forwardDictionary[key];
            }
            set
            {
                if (forwardDictionary.TryGetValue(key, out var oldValue))
                {
                    reverseDictionary.Remove(oldValue);
                }
                forwardDictionary[key] = value;
                reverseDictionary[value] = key;
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (forwardDictionary.ContainsKey(key))
            {
                throw new ArgumentException("An item with the same key has already been added.");
            }
            if (reverseDictionary.ContainsKey(value))
            {
                throw new ArgumentException("An item with the same value has already been added.");
            }

            forwardDictionary.Add(key, value);
            reverseDictionary.Add(value, key);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            forwardDictionary.Clear();
            reverseDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return forwardDictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return forwardDictionary.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return reverseDictionary.ContainsKey(value);
        }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return forwardDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            if (forwardDictionary.TryGetValue(key, out var value))
            {
                forwardDictionary.Remove(key);
                reverseDictionary.Remove(value);
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (forwardDictionary.TryGetValue(item.Key, out var value) && EqualityComparer<TValue>.Default.Equals(value, item.Value))
            {
                forwardDictionary.Remove(item.Key);
                reverseDictionary.Remove(item.Value);
                return true;
            }
            return false;
        }

        public bool TryGetKey(TValue value, out TKey key)
        {
            return reverseDictionary.TryGetValue(value, out key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return forwardDictionary.TryGetValue(key, out value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new InvalidOperationException();
        }
    }
}