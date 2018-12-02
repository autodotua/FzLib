using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Algorithm.Search
{
    public static class BinarySearch
    {
        /// <summary>
        /// 查找已经排序完成的数列中第一个大于key的值的位置
        /// </summary>
        /// <param name="source">源数组</param>
        /// <param name="key">要查找的值</param>
        /// <returns>大于key的值的位置，不存在返回-1</returns>
        public static int GetUpperBound(int[] source, int key)
        {
            if (source[source.Length - 1] < key)
            {
                //如果最大的还是比key小，就说明不存在
                return -1;
            }
            int first = 0;//头
            int currentLength = source.Length - 1;//正在分的长度
            int half;//左半部分的长度
            int middle;

            while (currentLength > 0)//如果还没分完
            {
                half = currentLength >> 1;//相当于/2
                middle = first + half;
                if (source[middle] > key)     //中位数大于key,在包含last的左半边序列中查找。
                {
                    currentLength = half;
                }
                else
                {
                    first = middle + 1;    //中位数小于等于key,在右半边序列中查找。
                    currentLength = currentLength - half - 1;
                }
            }
            return first;
        }
        /// <summary>
        /// 为了获取一个字符串中连续的另一个字符串需要删除的字符个数的最小值
        /// </summary>
        /// <param name="source">被查找的字符串</param>
        /// <param name="target">要查找的字符串</param>
        /// <param name="targetPosition">target在source中的位置的数组</param>
        /// <returns>最少需要删除的字符的个数</returns>
        public static int MinimumNumberOfDeletes(string source, string target, out int[] targetPosition)
        {
            //每一个目标字符存在于源字符的所有位置的列表数组
            List<int>[] position = new List<int>[target.Length];
            for (int i = 0; i < position.Length; i++)
            {
                //初始化列表数组
                position[i] = new List<int>();
            }
            for (int i = 0; i < source.Length; i++)
            {
                //遍历源字符串，如果和目标字符串有相等的则记录下位置
                for (int j = 0; j < target.Length; j++)
                {
                    if (source[i] == target[j])
                    {
                        position[j].Add(i);
                    }
                }
            }
            foreach (var i in position)
            {
                //再次遍历数组，如果有一个目标字符串中的字符没有在源字符串中出现过，说明肯定不符合条件。
                if (i.Count == 0)
                {
                    targetPosition = null;
                    return -1;
                }
            }
            int shortestDeletedLength = int.MaxValue;//要删除的最少的字符
            List<int> targetPositionList = new List<int>();//记录目标字符位置的临时列表
            for (int i = 0; i < position[0].Count; i++)
            {
                //循环目标字符串中第一个字符所有的位置
                targetPositionList = new List<int>();//清空位置列表
                int[] tempPosition = new int[position.Length];//记录所求的字符在该字符的索引中排第几个，及索引的索引
                tempPosition[0] = i; //首先单独记录第一个字符的位置// GetUpperBound(position[0].ToArray(), position[0][i]);
                targetPositionList.Add(position[0][i]);//把第一个位置加入位置列表中
                for (int j = 1; j < position.Length; j++)
                {
                    //内层循环，循环每一个目标字符
                    tempPosition[j] =
                        GetUpperBound(position[j].ToArray(),
                        position[j - 1][tempPosition[j - 1]]);
                    //寻找是否存在当前字符，该字符的位置比前一个字符的位置更后
                    if (tempPosition[j] == -1)
                    {
                        //如果没找到就跳到下一次循环
                        //这里我有问题，因为C++里这么写的所以我也这么写了
                        //但是我觉得这里找不到以后说明再也找不到了，可以直接跳到底。
                        goto next;
                    }
                    targetPositionList.Add(position[j][tempPosition[j]]);//把当前的字符位置加入位置列表中
                }
                int temp = position[position.Length - 1][tempPosition[tempPosition.Length - 1]] - position[0][i] - target.Length + 1;
                //比较临时变量    最后一个字符的位置                                                             第一个字符的位置    目标字符的长度
                if (shortestDeletedLength > temp)
                {
                    //如果发现新的长度比之前最短的还要短，那么
                    shortestDeletedLength = temp;//更新最短长度
                    targetPosition = targetPositionList.ToArray();//更新位置数组
                }
                next:
                ;
            }
            if (shortestDeletedLength != int.MaxValue)//如果最小长度变了，说明存在符合的序列
            {
                targetPosition = targetPositionList.ToArray();//更新位置数组
                return shortestDeletedLength;//返回长度
            }
            //如果最小长度没变，说明不存在符合的序列
            targetPosition = null;
            return -1;
        }
    }
}
