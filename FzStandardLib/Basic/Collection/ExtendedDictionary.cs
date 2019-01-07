using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Basic.Collection
{
    public class ExtendedDictionary<TKey,TValue> : Dictionary<TKey, TValue>
    {
        public bool AddIfValueNotExist { get; set; } = true;
        public bool ReturnDefaultIfValueNotExist { get; set; } = true;

        public new TValue this[TKey key]
        {
            get
            {
                if(!AddIfValueNotExist)
                {
                    return base[key];
                }
                if(!ContainsKey(key))
                {
                    return default;
                }
                return base[key];
            }
            set
            {
                if (!AddIfValueNotExist)
                {
                    base[key] = value;
                }
                else
                {
                    if (!ContainsKey(key))
                    {
                        Add(key, value);
                    }
                    else
                    {
                        base[key] = value;
                    }
                }
            }

        }

    }
}
