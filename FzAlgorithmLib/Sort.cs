using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Algorithm
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
        private static void Swap<T>(T[] array, int index1, int index2)
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
        private static void Swap<T>(ref T[] a, ref T[] b)
        {
            T[] temp = a;
            a = b;
            b = temp;
        }
        public static SortResult<T> BubbleSort<T>(IList<T> source) where T : IComparable<T>
        {
            return BubbleSort(source, Comparer<T>.Default);
        }
        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="order">排序的顺序，默认值为增序</param>
        /// <returns>排序完成的数组</returns>
        public static SortResult<T> BubbleSort<T>(IList<T> source, Comparer<T> comparer)
        {
            T[] array = source.ToArray();
            int comparisonTimes = 0;
            int swapTimes = 0;
            for (int i = 0; i < array.Length; i++)
            {
                bool exchange = false;//改进的冒泡排序，用于记录此轮是否进行了交换
                for (int j = array.Length - 1; j > i; j--)
                {
                    comparisonTimes++;
                    if (comparer.Compare(array[j - 1], array[j]) > 0)
                    {
                        swapTimes++;
                        Swap(array, j - 1, j);
                        exchange = true;
                    }
                }
                if (!exchange)
                {
                    break;
                }
            }
            return new SortResult<T>(array, swapTimes, comparisonTimes);
        }
        public static SortResult<T> SelectionSort<T>(IList<T> source) where T : IComparable<T>
        {
            return SelectionSort(source, Comparer<T>.Default);
        }
        /// <summary>
        /// 选择排序
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="source"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static SortResult<T> SelectionSort<T>(IList<T> source, Comparer<T> comparer)
        {
            T[] array = source.ToArray();
            int comparisonTimes = 0;
            int swapTimes = 0;
            for (int i = 0; i < array.Length; i++)
            {
                int minNumIndex = i;
                for (int j = i + 1; j < array.Length; j++)
                {
                    comparisonTimes++;
                    if (comparer.Compare(array[j], array[minNumIndex]) < 0)
                    {
                        minNumIndex = j;
                    }
                }
                if (minNumIndex != i)
                {
                    swapTimes++;
                    Swap(array, i, minNumIndex);
                }
            }
            return new SortResult<T>(array, swapTimes, comparisonTimes);
        }
        public static SortResult<T> InsertSort<T>(IList<T> source) where T : IComparable<T>
        {
            return InsertSort(source, Comparer<T>.Default);
        }
        /// <summary>
        /// 插入排序 
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="source"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static SortResult<T> InsertSort<T>(IList<T> source, Comparer<T> comparer)
        {
            T[] array = source.ToArray();
            int comparisonTimes = 0;
            int swapTimes = 0;
            for (int i = 0; i < array.Length; i++)
            {
                T temp = array[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    comparisonTimes++;
                    if (comparer.Compare(array[j], temp) <= 0)
                    {
                        break;
                    }
                    //如果前一个数比后一个数大
                    array[j + 1] = array[j];//前一个数赋值给后一个数
                    array[j] = temp;//temp在这里就是后一个数，后一个数赋值给前一个数
                                    //以上两行实现比较序列最后两个数的交换
                }
            }
            return new SortResult<T>(array, swapTimes, comparisonTimes);
        }
        public static SortResult<T> BinaryInsertionSort<T>(IList<T> source) where T : IComparable<T>
        {
            return BinaryInsertionSort(source, Comparer<T>.Default);
        }
        /// <summary>
        /// 折半插入排序 
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="source"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static SortResult<T> BinaryInsertionSort<T>(IList<T> source, Comparer<T> comparer)
        {
            T[] array = source.ToArray();
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
                    if (comparer.Compare(temp, array[mid]) < 0)//如果要插入的值比中间值要小
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
                }
                array[left] = temp;
            }
            return new SortResult<T>(array);
        }
        public static SortResult<T> ShellSort<T>(IList<T> source) where T : IComparable<T>
        {
            return ShellSort(source, Comparer<T>.Default);
        }
        /// <summary>
        /// 希尔排序 
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="source"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static SortResult<T> ShellSort<T>(IList<T> source, Comparer<T> comparer)
        {

            T[] array = source.ToArray();
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
                        if (comparer.Compare(array[j], temp) <= 0)
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
            return new SortResult<T>(array, swapTimes, comparisonTimes);
        }
        public static SortResult<T> MergeSortWithRecursiveTimes<T>(IList<T> source) where T : IComparable<T>
        {
            return MergeSortWithRecursiveTimes(source, Comparer<T>.Default);
        }
        /// <summary>
        /// 归并排序（递归）
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="source"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static SortResult<T> MergeSortWithRecursiveTimes<T>(IList<T> source, Comparer<T> comparer)
        {
            int comparisonTimes = 0;
            int recursiveTimes = 0;
            T[] array = Recursive(new List<T>(source)).ToArray();
            return new SortResult<T>(array, comparisonTimes, recursiveTimes);

            List<T> Recursive(List<T> lst)
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
                left = Recursive(left);
                right = Recursive(right);
                List<T> temp = new List<T>();
                while (left.Count > 0 && right.Count > 0)
                {
                    comparisonTimes++;
                    if (comparer.Compare(left[0], right[0]) <= 0)
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
        }

        public static SortResult<T> MergeSortWithoutRecursiveTimes<T>(IList<T> source) where T : IComparable<T>
        {
            return MergeSortWithoutRecursiveTimes(source, Comparer<T>.Default);
        }
        /// <summary>
        /// 归并排序（非递归）
        /// </summary>
        /// <typeparam name="T">可以进行排序的数据类型</typeparam>
        /// <param name="array"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static SortResult<T> MergeSortWithoutRecursiveTimes<T>(IList<T> source, Comparer<T> comparer)
        {
            T[] array = source.ToArray();
            int length = array.Length;
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
                        result[low++] = comparer.Compare(array[start1], array[start2]) < 0 ? array[start1++] : array[start2++];
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
            return new SortResult<T>(result);
        }
        public static SortResult<T> HeapSort<T>(IList<T> source)
        {
            return HeapSort(source, Comparer<T>.Default);
        }

        public static SortResult<T> HeapSort<T>(IList<T> source, Comparer<T> comparer)
        {
            T[] array = source.ToArray();
            BuildMaxHeapify();
            int j = array.Length;
            for (int i = 0; i < j;)
            {
                Swap(array, i, --j);
                if (j - 2 < 0)  //只剩下1个数 j代表余下要排列的数的个数
                    break;
                int k = 0;
                while (true)
                {
                    if (k > (j - 2) / 2) break;  //即：k > ((j-1)-1)/2 超出最后一个父节点的位置  
                    else
                    {
                        int temp = k;
                        k = ReSortMaxBranch(k, 2 * k + 1, 2 * k + 2, j - 1);
                        if (temp == k) break;
                    }
                }
            }

            return new SortResult<T>(array);

            void BuildMaxHeapify()
            {
                for (int i = array.Length / 2 - 1; i >= 0; i--)  //(data.Count-1)-1)/2为数列最大父节点索引
                {
                    int temp = i;
                    temp = ReSortMaxBranch(i, 2 * i + 1, 2 * i + 2, array.Length - 1);
                    if (temp != i)
                    {
                        int k = i;
                        while (k != temp && temp <= array.Length / 2 - 1)
                        {
                            k = temp;
                            temp = ReSortMaxBranch(temp, 2 * temp + 1, 2 * temp + 2, array.Length - 1);
                        }
                    }
                }
            }

            int ReSortMaxBranch(int maxIndex, int left, int right, int lastIndex)
            {
                int temp;
                if (right > lastIndex)  //父节点只有一个子节点
                    temp = left;
                else
                {
                    if (comparer.Compare(array[left], array[right]) > 0)
                        temp = left;
                    else temp = right;
                }

                if (comparer.Compare(array[maxIndex], array[temp]) < 0)
                    Swap(array, maxIndex, temp);
                else temp = maxIndex;
                return temp;
            }
        }

        public static SortResult<T> QuickSort<T>(IList<T> source)
        {
            return QuickSort(source, Comparer<T>.Default);
        }

        public static SortResult<T> QuickSort<T>(IList<T> source, Comparer<T> comparer)
        {

            T[] data = source.ToArray();
            int swap = 0;

            Recursive(0, data.Length - 1);

            return new SortResult<T>(data, swap);

            void Recursive(int low, int high)
            {
                if (low >= high)
                {
                    return;
                }
                T temp = data[low];
                int i = low + 1, j = high;
                while (true)
                {
                    while (comparer.Compare(data[j], temp) > 0)
                    {
                        j--;
                    }
                    while (comparer.Compare(data[i], temp) < 0 && i < j)
                    {
                        i++;
                    }
                    if (i >= j) break;
                    Swap(data, i, j);
                    swap++;
                    i++;
                    j--;
                }
                if (j != low)
                {
                    Swap(data, low, j);
                }
                Recursive(j + 1, high);
                Recursive(low, j - 1);
            }
        }

        public static SortResult<T> QuickSort2<T>(IList<T> source)
        {
            return QuickSort2(source, Comparer<T>.Default);
        }

        public static SortResult<T> QuickSort2<T>(IList<T> source, Comparer<T> comparer)
        {

            T[] data = source.ToArray();
            int swap = 0;

            Recursive(0, data.Length - 1);

            return new SortResult<T>(data, swap);

            void Recursive(int low, int high)
            {
                if (low >= high)
                {
                    return;
                }
                T temp = data[(low + high) / 2];
                int i = low - 1, j = high + 1;
                while (true)
                {
                    while (comparer.Compare(data[++i], temp) < 0) ;
                    while (comparer.Compare(data[--j], temp) > 0) ;
                    if (i >= j)
                    {
                        break;
                    }
                    Swap(data, i, j);
                }
                Recursive(j + 1, high);
                Recursive(low, j - 1);
            }
        }

        public static SortResult<T> QuickSort3<T>(IList<T> source)
        {
            return QuickSort3(source, Comparer<T>.Default);
        }

        public static SortResult<T> QuickSort3<T>(IList<T> source, Comparer<T> comparer)
        {

            T[] data = source.ToArray();
            int swap = 0;

            Recursive(0, data.Length - 1);

            return new SortResult<T>(data, swap);

            void Recursive(int low, int high)
            {
                if (low >= high) return;
                T temp = data[(low + high) / 2];
                int i = low - 1, j = high + 1;
                int index = (low + high) / 2;
                while (true)
                {
                    while (comparer.Compare(data[++i], temp) < 0) ;
                    while (comparer.Compare(data[--j], temp) > 0) ;
                    if (i >= j) break;
                    Swap(data, i, j);
                    if (i == index) index = j;
                    else if (j == index) index = i;
                }
                if (j == i)
                {
                    Recursive(j + 1, high);
                    Recursive(low, i - 1);
                }
                else //i-j==1
                {
                    if (index >= i)
                    {
                        if (index != i)
                            Swap(data, index, i);
                        Recursive(i + 1, high);
                        Recursive(low, i - 1);
                    }
                    else //index < i
                    {
                        if (index != j)
                            Swap(data, index, j);
                        Recursive(j + 1, high);
                        Recursive(low, j - 1);
                    }
                }
            }
        }
    }
    public class SortResult<T>
    {
        public SortResult()
        {

        }

        public SortResult(T[] result, int? swapTimes = null, int? comparisonTimes = null, int? recursiveTimes = null)
        {
            Result = result;
            SwapTimes = swapTimes;
            ComparisonTimes = comparisonTimes;
            RecursiveTimes = recursiveTimes;
        }
        /// <summary>
        /// 排序结果数组
        /// </summary>
        public T[] Result { get; set; }
        /// <summary>
        /// 数组内的两个数的交换次数
        /// </summary>
        public int? SwapTimes { get; set; }
        /// <summary>
        /// 数组内的数的比较次数
        /// </summary>
        public int? ComparisonTimes { get; set; }
        /// <summary>
        /// 调用的函数的次数，用于递归类型的排序中
        /// </summary>
        public int? RecursiveTimes { get; set; }
    }
}