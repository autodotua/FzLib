using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Algorithm
{
    /// <summary>
    /// 动态规划
    /// </summary>
    public static class DynamicProgramming
    {
        /// <summary>
        /// 二分/对分查找
        /// </summary>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static int BinarySearch(int[] array,int value,int length)
        {
            int begin = 0;
            int end = length - 1;
            while(begin<=end)
            {
                int mid = begin + (end - begin) / 2;
                if(array[mid]==value)
                {
                    return mid;
                }
                if(array[mid]>value)
                {
                    end = mid - 1;
                }
                else
                {
                    begin = mid + 1;
                }
            }
            return begin;
        }
        /// <summary>
        /// 打印背包数组
        /// </summary>
        /// <param name="dp"></param>
        public static void PrintIntArray(int[,] dp)
        {
            Console.Write("\t");
            for (int i = 1; i < dp.GetLength(1); i++)
            {
                Console.Write($"c{i}\t");
            }
            Console.WriteLine();

            for (int i = 1; i < dp.GetLength(0); i++)
            {
                Console.Write($"r{i}\t");
                for (int j = 1; j < dp.GetLength(1); j++)
                {
                    Console.Write($"{dp[i, j]}\t");
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// 最长上升子序列（二分搜索，NlogN复杂度）
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="longestArray"></param>
        /// <returns></returns>
        public static int LongestIncreasingSubsequenceUsingBinarySearch(int[] nums)
        {
           int[] dp = new int[nums.Length];
            int length = 1;
            dp[0] = nums[0];
            for(int i=1;i<nums.Length;i++)
            {
                if(nums[i]>dp[length-1])//如果这个数比之前所有的数都要大
                {
                    //将这个数加到数组末尾
                    dp[length] = nums[i];
                    length++;
                }
                else
                {
                    //搜索第一个在dp中大于这个数的索引，替换成这个更小的数
                    dp[BinarySearch(dp, nums[i],length)] = nums[i];

                }
            }

            return length;
        }
        /// <summary>
        /// 最长上升子序列，优先和更大
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="longestArray"></param>
        /// <returns></returns>
        public static int LongestIncreasingSubsequenceBiggerPriority(int[] nums, out int[] longestArray)
        {
            int[] dp = new int[nums.Length];
            int[] backtracking= new int[nums.Length];//回溯数组
            dp[0] = nums[0];
            for (int i = 1; i < nums.Length; i++)
            {
                for (int j = 0; j < i; j++)//升序求出来的是和最大，降序求出来的是和最小
                {
                    if (nums[i] > nums[j] )//如果后面的数比前面的数大
                    {
                     if(dp[j] + 1>dp[i])//如果第j个再加上这个以后上升序列的数量比当前要大
                        {
                            dp[i] = dp[j] + 1;
                            backtracking[i] = j;//指向上一个的索引
                        }
                    }

                }
            }
            List<int> longestList = new List<int>();
            int maxLength = dp.Max();
            int backtrackingIndex = dp.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
           
            //回溯
            for (int i=0;i<maxLength;i++)
            {
                longestList.Add(nums[backtrackingIndex]);
                backtrackingIndex = backtracking[backtrackingIndex];
            }
            longestArray = longestList.Reverse<int>().ToArray();
            return maxLength;
        }
        /// <summary>
        /// 最长上升子序列，优先和更小
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="longestArray"></param>
        /// <returns></returns>
        public static int LongestIncreasingSubsequenceSmallPriority(int[] nums, out int[] longestArray)
        {
            int[] dp = new int[nums.Length];
            int[] backtracking = new int[nums.Length];
            dp[0] = nums[0];
            for (int i = 1; i < nums.Length; i++)
            {
                for (int j = i-1; j >=0; j--)
                {
                    if (nums[i] > nums[j] )
                    {
                        if (dp[j] + 1 > dp[i])
                        {
                            dp[i] = dp[j] + 1;
                            backtracking[i] = j;
                        }
                    }

                }
            }
            List<int> longestList = new List<int>();
            int maxLength = dp.Max();
            int backtrackingIndex = dp.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;

            for (int i = 0; i < maxLength; i++)
            {
                longestList.Add(nums[backtrackingIndex]);
                backtrackingIndex = backtracking[backtrackingIndex];
            }
            longestArray = longestList.Reverse<int>().ToArray();
            return maxLength;
        }
        /// <summary>
        /// 完全背包求最小
        /// </summary>
        /// <param name="maxVolumn"></param>
        /// <param name="occupancy"></param>
        /// <param name="value"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static int CompleteKnapsackForMinimun(int maxVolumn, List<int> occupancy, List<int> value,out int[] dp)
        {
           dp = new int[maxVolumn+1];
            for (int i = 0; i <=maxVolumn; i++)
            {
                //用一个较大的数来填充。若使用int.MaxValue再加上价值则会溢出，所以采用了最大的一半。
                dp[i] = int.MaxValue/2;
            }
            dp[0] = 0;
            for (int i =0; i < occupancy.Count; i++)
            {
                for (int j = occupancy[i]; j <= maxVolumn; j++)
                {
                    dp[j] = Math.Min(dp[j], dp[j - occupancy[i]] + value[i]);
                }
            }
            return dp[maxVolumn] == int.MaxValue/2 ? -1 : dp[maxVolumn];
        }
        /// <summary>
        /// 01背包（二维数组版本）
        /// </summary>
        /// <param name="maxVolumn">最大容量</param>
        /// <param name="stuffCount">东西的件数</param>
        /// <param name="occupancy">每件物品占用空间的数组</param>
        /// <param name="value">每件物品的价值的数组</param>
        /// <param name="dp">返回背包</param>
        /// <returns>最大价值，以放入0件开始</returns>
        public static int ZeroOneKnapsack(int maxVolumn, List<int> occupancy, List<int> value, out int[,] dp)
        {
            int stuffCount = occupancy.Count;
            occupancy.Insert(0, 0);
            value.Insert(0, 0);
            if (occupancy.Count != value.Count)
            {
                dp = null;
                return -1;
            }
            dp = new int[stuffCount + 1, maxVolumn + 1];
            for (int i = 1; i <= stuffCount; i++)//循环放入的物品数量
            {
                for (int j = 1; j <= maxVolumn; j++)//循环占用的体积
                {
                    if (j >= occupancy[i])//如果体积允许放入这件物品
                    {
                        dp[i, j] //在只允许j的体积，放入i件物品时，
                            = Math.Max(//取两者的更大的
                                dp[i - 1, j - occupancy[i]] + value[i],//放这件物品，
                                dp[i - 1, j]);//不放这件物品
                    }
                    else//如果剩下的体积不够了
                    {
                        dp[i, j] = dp[i - 1, j];//取未放入时的价值
                    }
                }
            }
            return dp[stuffCount, maxVolumn];
        }
        /// <summary>
        /// 01背包（一维数组版本）
        /// </summary>
        /// <param name="maxVolumn">最大容量</param>
        /// <param name="stuffCount">东西的件数</param>
        /// <param name="occupancy">每件物品占用空间的数组</param>
        /// <param name="value">每件物品的价值的数组</param>
        /// <param name="lastDp">返回最后一次的（每个体积最大价值的）背包</param>
        /// <returns>最大价值，以放入0件开始</returns>
        public static int ZeroOneKnapsack(int maxVolumn, List<int> occupancy, List<int> value, out int[] lastDp)
        {
            int stuffCount = occupancy.Count;
            occupancy.Insert(0, 0);
            value.Insert(0, 0);
            if (occupancy.Count != value.Count)
            {
                lastDp = null;
                return -1;
            }
            lastDp = new int[maxVolumn + 1];
            for (int i = 1; i <= stuffCount; i++)//循环放入的物品数量
            {
                for (int j = maxVolumn; j >= occupancy[i]; j--)//循环占用的体积
                {
                    lastDp[j] //在只允许j的体积，放入i件物品时，
                        = Math.Max(//取两者的更大的
                            lastDp[j - occupancy[i]] + value[i],//放这件物品，
                            lastDp[j]);//不放这件物品
                }
            }
            return lastDp[maxVolumn];
        }
        /// <summary>
        /// 最长公共子串
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="resultString"></param>
        /// <returns></returns>
        public static int LongestCommonSubstring(string str1, string str2, out string[] resultString)
        {
            int[,] length = new int[str1.Length + 1, str2.Length + 1];
            for (int i = 1; i <= str1.Length; i++)
            {
                for (int j = 1; j <= str2.Length; j++)
                {
                    if (str1[i - 1] == str2[j - 1])//行列表示的字符相同
                    {
                        length[i, j] = length[i - 1, j - 1] + 1;//当前的LCS等于左上方的加一
                    }
                    else//不相同，有可能是连续被打断
                    {
                        length[i, j] = 0;
                    }

                }
            }
            int maxLength = 0;
            foreach (var i in length)
            {
                if (i > maxLength)
                {
                    maxLength = i;
                }
            }
            List<string> resultList = new List<string>();

            for (int row = 1; row <= str1.Length; row++)
            {
                for (int column = 1; column <= str2.Length; column++)
                {
                    if (length[row, column] == maxLength)//如果这个子串是最长子串
                    {
                        StringBuilder str = new StringBuilder();
                        int currentRow = row;
                        int currentColumn = column;
                        while (length[currentRow--, currentColumn--] >= 1)//向左上方移动直到为1
                        {
                            str.Insert(0, str1[currentRow]);
                        }
                        resultList.Add(str.ToString());
                    }
                }
            }
            resultString = resultList.ToArray();
            return maxLength;
        }
        /// <summary>
        /// 最长公共子序列
        /// </summary>
        /// <param name="str1">字符串1</param>
        /// <param name="str2">字符串2</param>
        /// <returns></returns>
        public static int LongestCommonSubsequence(string str1, string str2, out string[] resultString)
        {
            //int max = str1.Length > str2.Length ? str1.Length : str2.Length;
            int[,] length = new int[str1.Length + 1, str2.Length + 1];
            BacktrackingDirection[,] flag = new BacktrackingDirection[str1.Length + 1, str2.Length + 1];
            for (int i = 1; i <= str1.Length; i++)
            {
                for (int j = 1; j <= str2.Length; j++)
                {
                    if (str1[i - 1] == str2[j - 1])//行列表示的字符相同
                    {
                        length[i, j] = length[i - 1, j - 1] + 1;//当前的LCS等于左上方的加一
                        flag[i, j] = BacktrackingDirection.LeftUp;
                    }
                    else if (length[i - 1, j] == length[i, j - 1])//上方的LCSd等于边的
                    {
                        length[i, j] = length[i - 1, j];//当前的LCS等于上面的（当然左边的也行）
                        flag[i, j] = BacktrackingDirection.LeftOrUp;
                    }
                    else if (length[i - 1, j] > length[i, j - 1])//上方的LCS大于左边的
                    {
                        length[i, j] = length[i - 1, j];//当前的LCS等于上面的
                        flag[i, j] = BacktrackingDirection.Up;
                    }
                    else
                    {
                        length[i, j] = length[i, j - 1];
                        flag[i, j] = BacktrackingDirection.Left;
                    }
                }
            }
            //PrintIntArray(length);
            //PrintFlagArray(flag);
            int maxLength = length[str1.Length, str2.Length];


            List<string> resultList = new List<string>();
            // int rowIndex = 0;
            // int columnIndex = str2.Length;
            //// StringBuilder tempResult = new StringBuilder();
            // while (columnIndex>0)
            // {
            //     if(rowIndex==str1.Length)
            //     {
            //         columnIndex--;
            //     }
            //     else
            //     {
            //         rowIndex++;
            //     }
            //     if (length[rowIndex, columnIndex] ==maxLength)
            //     {
            //tempResult.Clear();
            //output(rowIndex, columnIndex,new StringBuilder());
            //string temp = tempResult.ToString();
            //if(!resultList.Contains(temp))
            //{
            //    resultList.Add(temp);
            //}
            //}
            // }
            output(str1.Length, str2.Length, "");

            void output(int i, int j, string str)
            {
                //C# 7.0新特性，本地方法，就是Method In Method
                while (i > 0 && j > 0)
                {
                    switch (flag[i, j])
                    {
                        case BacktrackingDirection.LeftUp:
                            str = str1[i-- - 1] + str;
                            j--;
                            break;
                        case BacktrackingDirection.Left:
                            j--;
                            break;
                        case BacktrackingDirection.Up:
                            i--;
                            break;
                        case BacktrackingDirection.LeftOrUp:
                            //var tempStr = string.Copy(str);
                            //output(i - 1, j, str);
                            //output(i, j - 1, tempStr);

                            //不知道为什么，C#中string感觉自带ref效果，导致外面的值都被改变了，所以只能新建实例。
                            output(i - 1, j, string.Copy(str));
                            output(i, j - 1, string.Copy(str));
                            return;
                    }
                }
                //string temp = str.ToString();// new string(str.ToString().ToCharArray().Reverse().ToArray());

                if (!resultList.Contains(str))
                {
                    resultList.Add(str);
                }
            }
            //以下方法只能够得到其中一个最大子序列
            //void output(int i, int j)
            //{
            //    if (i == 0 || j == 0)
            //    {
            //        return;
            //    }
            //    if (flag[i, j] ==BacktrackingDirection.LeftUp)
            //    {
            //        output(i - 1, j - 1);
            //        tempResult.Append(str1[i - 1]);
            //    }
            //    else if (flag[i, j] == BacktrackingDirection.Up)
            //    {
            //        output(i - 1, j);
            //    }
            //    else
            //    {
            //        output(i, j - 1);
            //    }
            //}
            resultString = resultList.ToArray();
            return maxLength;
        }
        /// <summary>
        /// 最长回文子序列
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="resultString"></param>
        /// <returns></returns>
        public static int LongestPalindromicSubsequence(string str1, out string[] resultString)
        {
            string str2 = new string(str1.Reverse().ToArray());
            int length = LongestCommonSubsequence(str1, str2, out string[] result);
            List<string> resultList = new List<string>(result);
            foreach (var str in result)
            {
                for (int i = 0; i < str.Length / 2; i++)
                {
                    if (str[i] != str[str.Length - 1 - i])
                    {
                        resultList.Remove(str);
                        break;
                    }
                }
            }
            resultString = result.ToArray();
            return length;
        }
        /// <summary>
        /// 最长回文子串
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="resultString"></param>
        /// <returns></returns>
        public static int LongestPalindromicSubstring(string str1, out string[] resultString)
        {
            string str2 = new string(str1.Reverse().ToArray());
            int length = LongestCommonSubstring(str1, str2, out string[] result);
            List<string> resultList = new List<string>(result);
            foreach (var str in result)
            {
                for (int i = 0; i < str.Length / 2; i++)
                {
                    if (str[i] != str[str.Length - 1 - i])
                    {
                        resultList.Remove(str);
                        break;
                    }
                }
            }
            resultString = result.ToArray();
            return length;
        }

        private enum BacktrackingDirection
        {
            Left,
            LeftUp,
            LeftOrUp,
            Up
        }
    }
}