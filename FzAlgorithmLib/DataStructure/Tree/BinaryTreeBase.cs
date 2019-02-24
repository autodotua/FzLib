using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FzLib.Algorithm.DataStructure.Tree
{
    public class BinaryTreeBase<TData, TNode>:TreeBase<TData,TNode> where TNode : BinaryTreeNodeBase<TData, TNode>, new()
    {
        /// <summary>
        /// 遍历方法
        /// </summary>
        public enum OrderType
        {
            PreOrder,
            InOrder,
            PostOrder,
            SequenceOrder,
        }
        /// <summary>
        /// 四种方法遍历二叉树
        /// </summary>
        /// <param name="type"></param>
        /// <param name="writeToConsole"></param>
        /// <returns></returns>
        public TData[] Traversal(OrderType type)
        {

            List<TData> list = new List<TData>();
            switch (type)
            {
                case OrderType.PreOrder:
                    PreOrderTraversal(Root, ref list);
                    break;
                case OrderType.InOrder:
                    InOrderTraversal(Root, ref list);
                    break;
                case OrderType.PostOrder:
                    PostOrderTraversal(Root, ref list);
                    break;
                case OrderType.SequenceOrder:
                    return SequenceOrderTraversal();
            }
            /// <summary>
            /// 前序遍历
            /// </summary>
            /// <param name="root"></param>
            void PreOrderTraversal(TNode root, ref List<TData> output)
            {
                if (root != null)
                {
                    //Console.Write(root.NodeData);
                    output.Add(root.Data);
                    PreOrderTraversal(root.LeftChild, ref output);
                    PreOrderTraversal(root.RightChild, ref output);
                }
            }
            /// <summary>
            /// 中序遍历
            /// </summary>
            /// <param name="root"></param>
            void InOrderTraversal(TNode root, ref List<TData> output)
            {
                if (root != null)
                {
                    InOrderTraversal(root.LeftChild, ref output);
                    output.Add(root.Data);
                    InOrderTraversal(root.RightChild, ref output);
                }
            }
            /// <summary>
            /// 后序遍历
            /// </summary>
            /// <param name="root"></param>
            void PostOrderTraversal(TNode root, ref List<TData> output)
            {
                if (root != null)
                {
                    PostOrderTraversal(root.LeftChild, ref output);
                    PostOrderTraversal(root.RightChild, ref output);
                    output.Add(root.Data);

                }
            }
            TData[] SequenceOrderTraversal()
            {
                return SequenceOrderTraversalNodes().Select(p => p == default ? default : p.Data).ToArray();

            }
            return list.ToArray();



        }
        public TNode[] TraversalNode(OrderType type)
        {

            List<TNode> list = new List<TNode>();
            switch (type)
            {
                case OrderType.PreOrder:
                    PreOrderTraversal(Root, ref list);
                    break;
                case OrderType.InOrder:
                    InOrderTraversal(Root, ref list);
                    break;
                case OrderType.PostOrder:
                    PostOrderTraversal(Root, ref list);
                    break;
                case OrderType.SequenceOrder:
                    return SequenceOrderTraversalNodes();
            }
            return list.ToArray();




            /// <summary>
            /// 前序遍历
            /// </summary>
            /// <param name="node"></param>
            void PreOrderTraversal(TNode node, ref List<TNode> output)
            {
                if (node != null)
                {
                    output.Add(node);
                    PreOrderTraversal(node.LeftChild, ref output);
                    PreOrderTraversal(node.RightChild, ref output);
                }
            }
            /// <summary>
            /// 中序遍历
            /// </summary>
            /// <param name="root"></param>
            void InOrderTraversal(TNode node, ref List<TNode> output)
            {
                if (node != null)
                {
                    InOrderTraversal(node.LeftChild, ref output);
                    output.Add(node);
                    InOrderTraversal(node.RightChild, ref output);
                }
            }
            /// <summary>
            /// 后序遍历
            /// </summary>
            /// <param name="root"></param>
            void PostOrderTraversal(TNode node, ref List<TNode> output)
            {
                if (node != null)
                {
                    PostOrderTraversal(node.LeftChild, ref output);
                    PostOrderTraversal(node.RightChild, ref output);
                    output.Add(node);

                }
            }
        }

        /// <summary>
        /// 层序遍历二叉树，不跳过空节点
        /// </summary>
        /// <returns>节点数组，空节点为null</returns>
        private TNode[] SequenceOrderTraversalNodes()
        {
            List<TNode> result = new List<TNode>();
            Queue<TNode> nodeQueue = new Queue<TNode>();
            nodeQueue.Enqueue(Root);
            TNode temp = null;
            while (nodeQueue.Count > 0)
            {
                result.Add(temp = nodeQueue.Dequeue());

                if (temp.LeftChild != null)
                {
                    nodeQueue.Enqueue(temp.LeftChild);
                }

                if (temp.RightChild != null)
                {
                    nodeQueue.Enqueue(temp.RightChild);
                }

            }
            //Console.WriteLine();
            return result.ToArray(); ;
        }

        /// <summary>
        /// 二叉树最大深度
        /// </summary>
        public int Depth
        {
            get
            {
                int depth = GetDepth(Root);
                return depth;
            }
        }
        /// <summary>
        /// 二叉树的节点总数
        /// </summary>
        public int Count => GetCount(Root);
        private int GetCount(TNode root)
        {

            if (root == null)
            {
                return 0;
            }
            else
            {
                return Math.Max(GetCount(root.LeftChild), GetCount(root.RightChild)) + 1;
            }
        }
        /// <summary>
        /// 遍历二叉树来获取深度
        /// </summary>
        /// <param name="node"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        private int GetDepth(BinaryTreeNodeBase<TData, TNode> node)
        {
            return node == null ? 0 : Math.Max(GetDepth(node.LeftChild), GetDepth(node.RightChild)) + 1;
        }
        public bool IsFull
        {
            get
            {
                int depth = Depth;
                if ((int)Math.Pow(2, depth + 1) - 1 == Count)
                {
                    return true;
                }
                return false;
            }
        }


        /// <summary>
        /// 转换为层序遍历的字符串
        /// </summary>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public string ToString(OrderType orderType = OrderType.InOrder, string split = " ")
        {
            return string.Join(split, Traversal(orderType).Select(p => p.ToString()));
        }
        /// <summary>
        /// 转换为阶梯形的数组
        /// </summary>
        /// <returns></returns>
        public TData[,] ToTable()
        {
            TData[,] array = new TData[Depth, (int)(Math.Pow(2, Depth + 1))];
            var nodeList = SequenceOrderTraversalNodes();//获取线性数据
            int level;//当前的层数
            for (int i = 1; i < nodeList.Length; i++)
            {
                if ((level = (int)Math.Floor(Math.Log(i) / Math.Log(2))) >= Depth)//如果当前的层数超过了二叉树深度就跳出循环
                {
                    break;
                }
                int column = i - (int)Math.Pow(2, level);//当前的数量减去这一行前面的所有节点的个数
                                                         // int realColumn = (int)Math.Pow(2, depth - level) + column * (int)Math.Pow(2, depth - level + 1);
                array[level, column] = nodeList[i].Data;
            }
            return array;
        }
        public TData[,] ToVisualTable()
        {
            TNode[,] arr = ToVisualNodeTable();
            int rowLength = arr.GetLength(0);
            int colLength = arr.GetLength(1);
            TData[,] result = new TData[rowLength, colLength];

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    if (arr[i, j] != null)
                    {
                        result[i, j] = arr[i, j].Data;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 转换为类似二叉树形状的数组，输出即是二叉树
        /// </summary>
        /// <returns></returns>
        public TNode[,] ToVisualNodeTable()
        {
            int depth = Depth;
            int width = (int)Math.Pow(2, depth) - 1;
            TNode[,] array = new TNode[depth , width];
            var nodes = SequenceOrderTraversalNodes();//获取线性数据
            foreach (var node in nodes)
            {
                int level = node.Level;
                int column = width / 2;
                var n = node;
                int l = level;
                while (n.Parent != null)
                {
                    if (n.IsRight)
                    {
                        column += (int)Math.Pow(2, depth - l);
                    }
                    else if (n.IsLeft)
                    {
                        column -= (int)Math.Pow(2, depth - l);
                    }
                    n = n.Parent;
                    l--;
                }
                array[level - 1, column] = node;

            }

            return array;
        }

        public string ToVisualString(bool guide = true)
        {
            StringBuilder str = new StringBuilder();
            var arr = ToVisualNodeTable();
            int rowLength = arr.GetLength(0);
            int colLength = arr.GetLength(1);
            if (!guide)
            {
                for (int i = 0; i < rowLength; i++)
                {
                    for (int j = 0; j < colLength; j++)
                    {
                        str.Append(string.Format("{0} ", arr[i, j] == null ? " " : arr[i, j].Data.ToString()));
                    }
                    str.AppendLine().AppendLine();
                }
            }
            else
            {
                string[,] strArray = new string[rowLength * 2 - 1, colLength];
                for (int i = 0; i < rowLength; i++)
                {
                    for (int j = 0; j < colLength; j++)
                    {
                        if (arr[i, j] != null)
                        {
                            strArray[i * 2, j] = arr[i, j].Data.ToString();
                           // if (i + 1 < rowLength)
                           if(!arr[i,j].IsLeaf)
                            {
                                strArray[i * 2+1, j - 1] = "/";
                                strArray[i * 2+1, j + 1] = "\\";

                            }
                        }
                    }
                }
                for (int i = 0; i < rowLength*2-1; i++)
                {
                    for (int j = 0; j < colLength; j++)
                    {
                        str.Append(string.Format("{0} ", strArray[i, j] ?? " "));
                    }
                    str.AppendLine().AppendLine();
                }
            }
            return str.ToString();
        }
    }
}
