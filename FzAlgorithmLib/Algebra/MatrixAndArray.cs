using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Algorithm.Algebra
{
    namespace Algebra
    {
        /// <summary>
        /// 矩阵与数组
        /// </summary>
        public static class MatrixAndArray
        {
            public static T[] TraversingMatrixClockwise<T>(T[,] array)
            {
                List<T> list = new List<T>() { array[0, 0] };
                int level = 0;//外层为0，往内+1
                int count = array.Length;//总次数
                int direction = 0;//0右1下2左3上
                int row = 0;//行标
                int column = 0;//列标
                int length = array.GetLength(0);//单维度的长度
                if (length * length != count)
                {
                    throw new Exception("Row≠Column.");
                }
                //Console.WriteLine("Current Matrix:");
                //for (int i = 0; i < length; i++)
                //{
                //    for (int j = 0; j < length; j++)
                //    {
                //        Console.Write($"{array[i, j],6} ");
                //    }
                //    Console.WriteLine();
                //}
                //Console.WriteLine();
                //Console.WriteLine("After operation:");
                //Console.Write($"{array[0, 0]} ");

                while (count-- > 1)
                {
                    if (column == length - level - 1 && row == level
                        || row == length - level - 1 && column == length - level - 1
                        || column == level && row == length - 1 - level)//需要转弯
                    {
                        direction++;//方向顺时针旋转90°
                    }
                    else if (row == level + 1 && column == level)//需要往内一层
                    {
                        direction = 0;//强制改方向为朝右
                        level++;//向内一层
                    }
                    switch (direction)
                    {
                        case 0:
                            column++;
                            break;
                        case 2:
                            column--;
                            break;
                        case 1:
                            row++;
                            break;
                        case 3:
                            row--;
                            break;
                        default:
                            break;
                    }
                    list.Add(array[row, column]);
                }
                return list.ToArray();
            }
        }
    }
}