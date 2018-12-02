using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Algorithm.Basic
{
    /// <summary>
    /// 排序
    /// </summary>
    public static class Sort
    {
        /// <summary>
        /// 交换数组中的两个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        private static void Swap<T>(ref T[] array, int index1, int index2) where T : IComparable<T>
        {
            T temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }
        /// <summary>
        /// 交换两个数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private static void Swap<T>(ref T[] a, ref T[] b) where T : IComparable<T>
        {
            T[] temp = a;
            a = b;
            b = temp;
        }
        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="order">排序的顺序，默认值为增序</param>
        /// <returns>排序完成的数组</returns>
        public static SortedResult<T> BubbleSort<T>(T[] sourceArray, Order order = Order.Ascending) where T : IComparable<T>
        {
            T[] array = sourceArray;
            int comparisonTimes = 0;
            int swapTimes = 0;
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = array.Length - 1; j > i; j--)
                {
                    comparisonTimes++;
                    if (array[j - 1].CompareTo(array[j]) > 0)
                    {
                        swapTimes++;
                        Swap(ref array, j - 1, j);
                    }
                }
            }
            if (order == Order.Descending)
            {
                Array.Reverse(array);
            }
            return new SortedResult<T>()
            {
                ComparisonTimes = comparisonTimes,
                SwapTimes = swapTimes,
                Result = array
            };
        }
        /// <summary>
        /// 选择排序
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static SortedResult<T> SelectionSort<T>(T[] sourceArray, Order order = Order.Ascending) where T : IComparable<T>
        {
            T[] array = sourceArray;
            int comparisonTimes = 0;
            int swapTimes = 0;
            for (int i = 0; i < array.Length; i++)
            {
                int minNumIndex = i;
                for (int j = i + 1; j < array.Length; j++)
                {
                    comparisonTimes++;
                    if (array[j].CompareTo(array[minNumIndex]) < 0)
                    {
                        minNumIndex = j;
                    }
                }
                if (minNumIndex != i)
                {
                    swapTimes++;
                    Swap(ref array, i, minNumIndex);
                }
            }
            if (order == Order.Descending)
            {
                Array.Reverse(array);
            }
            return new SortedResult<T>()
            {
                ComparisonTimes = comparisonTimes,
                SwapTimes = swapTimes,
                Result = array
            };
        }
        /// <summary>
        /// 插入排序 
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static SortedResult<T> InsertSort<T>(T[] sourceArray, Order order = Order.Ascending) where T : IComparable<T>
        {
            T[] array = sourceArray;
            int comparisonTimes = 0;
            int swapTimes = 0;
            for (int i = 0; i < array.Length; i++)
            {
                T temp = array[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    comparisonTimes++;
                    if (array[j].CompareTo(temp) <= 0)
                    {
                        break;
                    }
                    //如果前一个数比后一个数大
                    array[j + 1] = array[j];//前一个数赋值给后一个数
                    array[j] = temp;//temp在这里就是后一个数，后一个数赋值给前一个数
                                    //以上两行实现比较序列最后两个数的交换
                }
            }
            if (order == Order.Descending)
            {
                Array.Reverse(array);
            }
            return new SortedResult<T>()
            {
                ComparisonTimes = comparisonTimes,
                SwapTimes = swapTimes,
                Result = array
            };
        }
        /// <summary>
        /// 折半插入排序 
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static SortedResult<T> BinaryInsertionSort<T>(T[] sourceArray, Order order = Order.Ascending) where T : IComparable<T>
        {
            T[] array = sourceArray;
            for (int i = 0; i < array.Length; i++)
            {
                int left = 0;
                int right = i - 1;
                T temp = array[i];
                int mid = 0;
                //下面这个While和二分搜索异曲同工
                //下面的While所涉及到的数字都是已经排序完成的数字
                while (left <= right)
                {
                    mid = (left + right) / 2;//取中间值
                    if (temp.CompareTo(array[mid]) < 0)//如果要插入的值比中间值要小
                    {
                        right = mid - 1;//说明目标位置在左边
                    }
                    else//同理
                    {
                        left = mid + 1;
                    }

                }
                //下面的for是移动数组内的数字来给一个空缺插入
                for (int j = i; j > mid; j--)
                {
                    array[j] = array[j - 1];//右移一位
                    array[left] = temp;
                }
            }
            if (order == Order.Descending)
            {
                Array.Reverse(array);
            }
            return new SortedResult<T>()
            {
                Result = array,
            };
        }
        /// <summary>
        /// 希尔排序 
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static SortedResult<T> ShellSort<T>(T[] sourceArray, Order order = Order.Ascending) where T : IComparable<T>
        {
            T[] array = sourceArray;
            int comparisonTimes = 0;
            int swapTimes = 0;
            int h = 1;//步长
            while (h < array.Length / 3)
            {
                //获取最佳步长：1、4、13、40……
                h *= 3;
                h++;
            }
            while (h >= 1)
            {
                for (int i = h; i < array.Length; i++)
                {
                    T temp = array[i];
                    //int j;//用于维基百科版本
                    for (int j = i - h; j >= 0; j -= h)
                    {
                        //跳跃比较
                        comparisonTimes++;
                        if (array[j].CompareTo(temp) <= 0)
                        {
                            break;
                        }
                        array[j + h] = array[j];
                        array[j] = temp;
                    }
                    // array[j + h] = temp;//维基百科上是这一句
                }
                h /= 3;//缩减步长
            }
            if (order == Order.Descending)
            {
                Array.Reverse(array);
            }
            return new SortedResult<T>()
            {
                ComparisonTimes = comparisonTimes,
                SwapTimes = swapTimes,
                Result = array
            };
        }
        /// <summary>
        /// 归并排序（递归）
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static SortedResult<T> MergeSortWithRecursiveTimes<T>(T[] sourceArray, Order order = Order.Ascending) where T : IComparable<T>
        {
            int comparisonTimes = 0;
            int recursiveTimes = 0;
            T[] array = MergeSortWithRecursiveTimes(new List<T>(sourceArray), ref comparisonTimes, ref recursiveTimes).ToArray();
            if (order == Order.Descending)
            {
                Array.Reverse(array);
            }
            return new SortedResult<T>()
            {
                Result = array,
                ComparisonTimes = comparisonTimes,
                RecursiveTimes = recursiveTimes,
            };
        }
        /// <summary>
        /// 归并排序（递归临时List版本）
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="lst"></param>
        /// <param name="comparisonTimes"></param>
        /// <param name="recursiveTimes"></param>
        /// <returns></returns>
        public static List<T> MergeSortWithRecursiveTimes<T>(List<T> lst, ref int comparisonTimes, ref int recursiveTimes) where T : IComparable<T>
        {
            recursiveTimes++;
            if (lst.Count <= 1)
            {
                return lst;
            }
            int mid = lst.Count / 2;
            List<T> left = new List<T>();//定义左侧List
            List<T> right = new List<T>();//定义右侧List

            //以下兩個循環把lst分為左右兩個List
            for (int i = 0; i < mid; i++)
            {
                left.Add(lst[i]);
            }
            for (int j = mid; j < lst.Count; j++)
            {
                right.Add(lst[j]);
            }
            left = MergeSortWithRecursiveTimes(left, ref comparisonTimes, ref recursiveTimes);
            right = MergeSortWithRecursiveTimes(right, ref comparisonTimes, ref recursiveTimes);
            List<T> temp = new List<T>();
            while (left.Count > 0 && right.Count > 0)
            {
                comparisonTimes++;
                if (left[0].CompareTo(right[0]) <= 0)
                {
                    temp.Add(left[0]);
                    left.RemoveAt(0);
                }
                else
                {
                    temp.Add(right[0]);
                    right.RemoveAt(0);
                }
            }
            if (left.Count > 0)
            {
                for (int i = 0; i < left.Count; i++)
                {
                    temp.Add(left[i]);
                }
            }
            if (right.Count > 0)
            {
                for (int i = 0; i < right.Count; i++)
                {
                    temp.Add(right[i]);
                }
            }
            return temp;
        }
        /// <summary>
        /// 归并排序（非递归）
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="array"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static SortedResult<T> MergeSortWithoutRecursiveTimes<T>(T[] sourceArray, Order order = Order.Ascending) where T : IComparable<T>
        {
            int length = sourceArray.Length;
            T[] array = sourceArray;
            T[] result = new T[length];
            for (int block = 1; block < length * 2; block *= 2)
            {
                for (int start = 0; start < length; start += 2 * block)
                {
                    int low = start;
                    int mid = (start + block) < length ? (start + block) : length;
                    int high = (start + 2 * block) < length ? (start + 2 * block) : length;
                    int start1 = low;
                    int end1 = mid;
                    int start2 = mid;
                    int end2 = high;
                    while (start1 < end1 && start2 < end2)
                    {
                        result[low++] = array[start1].CompareTo(array[start2]) < 0 ? array[start1++] : array[start2++];
                    }
                    while (start1 < end1)
                    {
                        result[low++] = array[start1++];
                    }
                    while (start2 < end2)
                    {
                        result[low++] = array[start2++];
                    }
                }
                Swap(ref array, ref result);
            }
            result = array;
            return new SortedResult<T>()
            {
                Result = result,
            };
        }

        public enum Order
        {
            /// <summary>
            /// 降序
            /// </summary>
            Descending,
            /// <summary>
            /// 升序
            /// </summary>
            Ascending,
        }
    }
    public class SortedResult<T> where T : IComparable<T>
    {
        /// <summary>
        /// 排序结果数组
        /// </summary>
        public T[] Result { get => result; set => result = value; }
        /// <summary>
        /// 数组内的两个数的交换次数
        /// </summary>
        public int SwapTimes { get => swapTimes; set => swapTimes = value; }
        /// <summary>
        /// 数组内的数的比较次数
        /// </summary>
        public int ComparisonTimes { get => comparisonTimes; set => comparisonTimes = value; }
        /// <summary>
        /// 调用的函数的次数，用于递归类型的排序中
        /// </summary>
        public int RecursiveTimes { get => recursiveTimes; set => recursiveTimes = value; }

        private T[] result;
        private int swapTimes;
        private int comparisonTimes;
        private int recursiveTimes;
    }
}