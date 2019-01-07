using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            //var a = new double[] { 9,8,7,6,1,2,3,5,4,4, 9, 8, 7, 6, 1, 2, 3, 5, 4, 4 };
            ////Sort.ShellSort(a);
            //MatrixAndArray.TraversingMatrixClockwise(new double[4, 4] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 }, { 13, 14, 15, 16 } });
            //double[] l = new double[]{ 1,2,3,6,5,4,9,8,7};
            //Sort.MergeSortWithoutRecursiveTimes(l);
            //Game.BashGame(100, 19,Game.Winner.LastEmpty);
            //while (true)
            //{
            //    Game.NimmGame(new int[] { int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()) });
            //}
            //Console.ReadKey();
            //DynamicProgramming.ZeroOneKnapsack(15,new List<int> { 2,4,5,7},new List<int> { 1,2,3,5},out int[,] arr);
            //DynamicProgramming.PrintDpArray(arr);
            //Console.WriteLine(DynamicProgramming.ZeroOneKnapsack(15, new List<int> { 2, 4, 5, 7 }, new List<int> { 1, 2, 3, 5 }, out int[] array));

            // Console.WriteLine( DataStructure.BinaryTreeOfFallingBall(42, 2));

            //BinaryTree<int> tree = new BinaryTree<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            //tree.OrderTree(BinaryTree<int>.OrderType.PreOrderTree, true);
            //tree.OrderTree(BinaryTree<int>.OrderType.InOrderTree, true);


            //tree.OrderTree(BinaryTree<int>.OrderType.PostOrderTree, true);

            //t.Insert(2);
            //t.Insert(-1);
            //t.Insert(3);
            //t.Insert(-3);
            //t.Insert(-2);
            //t.WalkTree();//二叉树的遍历  

            // var l=BinaryTree<int>.GetPath(5);
            //var tree = new BinaryTree<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 11, 12, 13, 14, 15, 16, 17 });
            //tree.Tree.RightTree.RightTree.RightTree = new BinaryTree<int>.TreeCore<int>(15);
            //tree.OrderTree(BinaryTree<int>.OrderType.PreOrderTree, true);
            //tree.OrderTree(BinaryTree<int>.OrderType.InOrderTree, true);
            //tree.OrderTree(BinaryTree<int>.OrderType.PostOrderTree, true);
            //Console.WriteLine(tree.Count);
            //tree.OrderTree(BinaryTree<int>.OrderType.WideOrderTree, true);

            //var tree = new BinaryTree<int>();
            //tree.Tree = new BinaryTree<int>.TreeCore<int>(0);
            //tree.Tree.LeftTree = new BinaryTree<int>.TreeCore<int>(1);
            //tree.Tree.LeftTree.LeftTree = new BinaryTree<int>.TreeCore<int>(3);
            //tree.Tree.LeftTree.LeftTree.LeftTree = new BinaryTree<int>.TreeCore<int>(6);
            //tree.Tree.LeftTree.RightTree = new BinaryTree<int>.TreeCore<int>(4);
            //tree.Tree.LeftTree.RightTree.LeftTree = new BinaryTree<int>.TreeCore<int>(7);
            //tree.Tree.RightTree = new BinaryTree<int>.TreeCore<int>(2);

            //tree.Tree.RightTree.RightTree = new BinaryTree<int>.TreeCore<int>(5);
            //tree.Tree.RightTree.RightTree.RightTree = new BinaryTree<int>.TreeCore<int>(55);
            //tree.Tree.LeftTree.LeftTree.LeftTree.LeftTree = new BinaryTree<int>.TreeCore<int>(555);
            //tree.Tree.LeftTree.LeftTree.LeftTree.LeftTree.LeftTree = new BinaryTree<int>.TreeCore<int>(5555);

            //Program p = new Program();
            //p.PrintArray(tree.ToVisualArray());
            //Console.WriteLine(tree.ToString());
            //Console.WriteLine( Basic.DynamicProgramming.LongestCommonSubsequence("abcdefg", "bacdgfe",out string[] x));
            // foreach (var i in x)
            // {
            //     Console.WriteLine(i);
            // }
            //Basic.DynamicProgramming.LongestPalindromicSubstring( "7878786712346756",out string[] str);
            //Console.WriteLine();
            //foreach (var i in str)
            //{
            //    Console.WriteLine(i);
            //}

            // Basic.BinarySearch.MinimumNumberOfDeletes("abjdbkhbbnsjdnsjc", "abc");
            //string a = Console.ReadLine();
            //string b = Console.ReadLine();
            //Console.WriteLine(Basic.Search.MinimumNumberOfDeletes(a, b,out int[] i));
            //int n = 20;
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //Coding.GrayCodeWithRecursion(n);
            //sw.Stop();
            //    Console.Write("递归:"+sw.ElapsedMilliseconds+Environment.NewLine);
            //sw.Restart();
            //Coding.GrayCodeWithoutRecursion(n);
            //sw.Stop();
            //Console.Write("非递归:" + sw.ElapsedMilliseconds + Environment.NewLine);
            //sw.Restart();
            //Coding.GrayCodeUsingClassification(n);
            //sw.Stop();
            //Console.Write("找规律:" + sw.ElapsedMilliseconds + Environment.NewLine);


            //var b = new Search.GenrateMazeWithRandomPrim();
            //b.PrintWhenGenerating = true;

            //b.Genrate(new System.Drawing.Size(35, 35));
            //bool[,] a = b.Map;

            // //b.PrintMap();
            //     
            //            };
            //            c.BreadthFirstSearchMethod(b.Map, new System.Drawing.Point(1, 1), new System.Drawing.Point(33, 33));

            //var a = new Search.NQueensProblem(13);

            //Console.WriteLine(a.Count);

            //int b=Basic.DynamicProgramming.LongestIncreasingSubsequenceUsingBinarySearch(new int[] { 1, 3, 4, 7, 2, 4, 5, 1, 4, 6 });
            //Console.WriteLine(b);
            // new Search.GenrateMazeWithDepthFirst().PrintMap();

            //Test t = new Test(() =>
            //{
            //    var a = new Search.GenrateMazeWithRecursiveSegmentation();
            //    a.Genrate(new System.Drawing.Size(33, 33));
            //});
            //t.Start(100);
            //Console.WriteLine(t);


            //Console.WriteLine(DynamicProgramming.CompleteKnapsackForMinimun(100, new List<int>(new int[] { 50, 1 }), new List<int>(new int[] { 30, 1 })));



            //GenrateMazeWithDepthFirst maze = new GenrateMazeWithDepthFirst();
            //maze.Genrate(new System.Drawing.Size(25, 25));
            //maze.PrintMap();

          // SolveMaze solve = new SolveMaze();
          // solve.PrintWhenGenerating = true;
          // solve.Map = maze.Map;
          // solve.DepthFirstSearchMethod(maze.Map, new System.Drawing.Point(1, 1), new System.Drawing.Point(18, 18));
          // solve.PrintMap(true);
            //maze.Map


            Console.ReadKey();
        }
        //private void PrintArray(object[,] array)
        //{
        //    for (int i = 0; i < array.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < array.GetLength(1); j++)
        //    {

        //            Console.Write(array[i, j] == null ? string.Format("{0,4}", "") : $"{(array[i, j] as BinaryTree<int>.TreeCore<int>).NodeData,4}");
        //        }
        //        Console.WriteLine();
        //    }
        //}
        public static int GetRandomNumber(int from, int smallerThan)
        {
            RNGCryptoServiceProvider r = new RNGCryptoServiceProvider();
            byte[] b = new byte[8];
            r.GetBytes(b);
            return (b[0] + b[1] + b[2] + b[3] + b[4] + b[5] + b[6] + b[7]) % (smallerThan - from) + from;
        }
    }

}
