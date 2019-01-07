using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FzLib.Basic.Collection
{
    public static class Loop
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
        }

        public static void ForRange(int start, int smallerThan, Action<int> action)
        {
            for (int i = start; i < smallerThan; i++)
            {
                action(i);
            }
        }
        public static void ForRange(int smallerThan, Action<int> action)
        {
            ForRange(0, smallerThan, action);
        }
    }
}
