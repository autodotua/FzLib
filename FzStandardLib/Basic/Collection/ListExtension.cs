using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Basic.Collection
{
    public static class ListExtension
    {
        public static bool ListEndWith<T>(this IList<T> source, IList<T> withWhat, int length) where T : IEquatable<T>
        {
            for (int i = length - withWhat.Count, j = 0; j < withWhat.Count; i++, j++)
            {
                if (!source[i].Equals(withWhat[j]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}