using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FzLib.Extension
{
   public class AutoSortList<T> : List<T>
    {
    
        public AutoSortList(Func<T, IComparable> compareFunc)
        {
            CompareFunc = compareFunc ?? throw new ArgumentNullException(nameof(compareFunc));
        }
        public AutoSortList(Func<T, IComparable> compareFunc,IEnumerable<T> items)
        {
            CompareFunc = compareFunc ?? throw new ArgumentNullException(nameof(compareFunc));
            foreach (var item in items.OrderBy(p=>CompareFunc(p)))
            {
                base.Add(item);
            }
        }

        Func<T,IComparable> CompareFunc { get;  set; }
        public new void Add(T item)
        {
            Insert(item);
        }
        public void Insert(T item)
        {
            if(Count==0)
            {
                base.Add(item);
            }

            else
            {
                for(int i=0;i<Count;i++)
                {
                    if(CompareFunc(item).CompareTo(CompareFunc(this[i]))<0)
                    {
                        base.Insert(i, item);
                        return;
                    }
                }
                base.Add(item);
            }
        }
        public new void Insert(int index,T item)
        {
            throw new Exception("不允许此方法");
        }
        public override string ToString()
        {
            return string.Join(",", this);
        }

    }
}
