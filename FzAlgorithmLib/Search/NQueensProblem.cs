using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FzLib.Algorithm.Search
{
    public class NQueensProblem
    {
        /// <summary>
        /// 皇后个数、棋盘边长
        /// </summary>
       public int N { get; private set; }
        /// <summary>
        /// lines[n]表示第n行棋子所在的列的index
        /// </summary>
        private int[] lines;
        /// <summary>
        /// 解的总数
        /// </summary>
        int count = 0;
        //  public int NoRepeatedCount { get => noRepeteadCount; }
        // int noRepeteadCount=0;
        /// <summary>
        /// 解的总数
        /// </summary>
        public int Count { get=>count; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="n"></param>
        public NQueensProblem(int n)
        {
            N = n;
            lines = new int[n];
            Solve();
        }
        /// <summary>
        /// 解
        /// </summary>
        private void Solve()
        {
            int i = 0;
            while (true)
            {
                if (lines[i] < N)
                {
                    if (IsConflict(i))
                    {
                        lines[i]++;
                        continue;
                    }
                    if (i >= N - 1)
                    {
                        count++;
                        result.Add(lines.Clone() as int[]);
                        lines[N - 1]++;
                        continue;
                    }
                    i++;
                    continue;
                }
                else
                {
                    lines[i] = 0;
                    i--;
                    if(i<0)
                    {
                        return;
                    }
                    lines[i]++;
                    continue;
                }
            }
        }
        //private bool IsRepeated()
        //{
        //    int[] newLines = new int[N];
        //    int[] temp = lines.Clone() as int[];
        //    for (int i = 0; i < 3; i++)
        //    {
        //        newLines = temp.Clone() as int[];
        //        //旋转
        //        for (int j = 0; j < N; j++)
        //        {
        //            newLines[lines[j]] = j;
        //        }
        //        temp=newLines.Clone() as int[];
        //        if (result.Where(p => p.SequenceEqual(newLines)).Count() != 0)
        //        {
        //            return true;
        //        }

        //    }
        //    ////旋转
        //    //for (int j = 0; j < N; j++)
        //    //{
        //    //    newLines[lines[j]] = j;
        //    //}

        //    //if (result.Where(p => p.SequenceEqual(newLines)).Count() != 0)
        //    //{
        //    //    return true;
        //    //}


        //    //转置
        //    for (int i = 0; i < N; i++)
        //    {
        //        newLines[i] = lines[N-i-1];
        //    }

        //    if (result.Where(p => p.SequenceEqual(newLines)).Count() != 0)
        //    {
        //        return true;
        //    }

        //    //对称
        //    for (int i = 0; i < N; i++)
        //    {
        //        newLines[i] = N-lines[i]-1;
        //    }

        //    if (result.Where(p => p.SequenceEqual(newLines)).Count() != 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        public List<int[]> Result { get => result; }
        public List<int[]> result = new List<int[]>();
      //  public List<int[]> noRepeatedResult = new List<int[]>();
      /// <summary>
      /// 是否与现有棋局冲突
      /// </summary>
      /// <param name="lastLine"></param>
      /// <returns></returns>
        private bool IsConflict(int lastLine)
        {
            for (int i = 0; i < lastLine; i++)
            { 
                int temp = lines[i] - lines[lastLine];
                if (temp==0||temp== lastLine - i||temp==i- lastLine)
                {
                    return true;
                }
            }
            return false;
        }
        public bool[,] ToTwoDimensionalArray(int[] array)
        {
            bool[,] result= new bool[N, N];
            for(int i=0;i<N;i++)
            {
                result[i, array[i]] = true;
            }
            return result;
        }
    }
}
