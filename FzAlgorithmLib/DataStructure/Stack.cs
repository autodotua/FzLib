using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Algorithm.DataStructure
{
    /// <summary>
    /// 数据结构
    /// </summary>
    public static class Stack<T>
    {
        /// <summary>
        /// 栈
        /// </summary>
        public static T[] GetStackOrder(T[] stackIn,T[] stackOut)
        {
            List<T> order = new List<T>();
            int indexOfOut = 0;
            for (int indexOfIn = 0; indexOfIn < stackIn.Length; indexOfIn++)
            {
                Console.WriteLine($"in[{indexOfIn}]:{stackIn[indexOfIn]}");
                int tempIndex = indexOfIn;//这个临时索引是
                                          //如果出栈顺序符合了
                while (tempIndex >= 0 && indexOfOut < stackOut.Length && stackOut[indexOfOut].Equals( stackIn[tempIndex]))
                {
                    order.Add(stackOut[indexOfOut]);
                   // Console.WriteLine($"out[{indexOfOut}]:{stackOut[indexOfOut]}");
                    indexOfOut++;//下一个出栈的数
                    tempIndex--;//入栈的索引往前一个
                }
            }
            return order.ToArray();
         //   Console.ReadKey();
        }

    }
}
