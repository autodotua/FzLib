using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Algorithm
{
    public static class Coding
    {
        public static int[,] GrayCodeUsingClassification(int n)
        {
            /*
            找规律。
            最右边的是0,1,1,0,0,1,1,……所以是10交替，两个一组
            右2是         0,0,1,1,1,1,0,0,0,0,……所以是四个一组
            深入分析，发现其实是2^n个0，然后2^n个1，下一组是前一组的逆序。
            */
            int[,] arr = new int[(int)Math.Pow(2, n), n];
            for (int c = 0; c < n; c++)
            {
                for (int r = 0; r < (int)Math.Pow(2, n); r++)
                {
                    int g = (int)Math.Pow(2, n - c);
                    arr[r, c] = (r % g < g / 2) ? (r % (g * 2) < g ? 0 : 1) : (r % (g * 2) < g ? 1 : 0);
                }
            }
            return arr;
        }
        public static int[,] GrayCodeWithoutRecursion(int n)
        {
            /*
            第一次改变最右边的数
            第二次改变右数第一个1的左边的数
            */

            int[,] arr = new int[(int)Math.Pow(2, n), n];
            for (int i = 1; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < n; j++)
                {
                    arr[i, j] = arr[i - 1, j];
                }
                if (i % 2 == 1)
                {
                    arr[i, n - 1] = arr[i, n - 1] == 1 ? 0 : 1;
                }
                else
                {
                    for (int j = n - 1; j >= 0; j--)
                    {
                        if (arr[i, j] == 1)
                        {
                            arr[i, j - 1] = arr[i, j - 1] == 1 ? 0 : 1;
                            break;
                        }
                    }
                }
            }

            return arr;
        }
        public static int[,] GrayCodeWithRecursion(int n)
        {
            String[] GrayCode(int tempN)
            {
                String[] grayCodeArr = new String[(int)Math.Pow(2, tempN)];
                if (tempN == 1)
                {
                    grayCodeArr[0] = "0";
                    grayCodeArr[1] = "1";
                }
                else
                {
                    String[] before = GrayCode(tempN - 1);
                    for (int i = 0; i < before.Length; i++)
                    {
                        grayCodeArr[i] = "0" + before[i];
                        grayCodeArr[grayCodeArr.Length - 1 - i] = "1" + before[i];
                    }
                }
                return grayCodeArr;
            }
            int[,] arr = new int[(int)Math.Pow(2, n),n];
            string[] code = GrayCode(n);
            for (int i = 0; i < (int)Math.Pow(2, n); i++)
            {
                for (int j = 0; j < n; j++)
                {
                    arr[i, j] = code[i][j] - '0';
                }
            }
            return arr;
        }


    }
}
