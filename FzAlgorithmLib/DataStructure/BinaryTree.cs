using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Algorithm.DataStructure
{
    public class BinaryTree<T> where T : IComparable<T>
    {
        public static long FallingBall(long depth, long index)
        {/*
            小球从二叉树顶端落下，每次默认经过一个节点时向左子节点。
            每次经过节点时，节点的状态被交换，即下一个球经过该节点时方向相反。
                                        1
                            2                       3
                      4          5           6           7
                   8    9   10  11   12   13   14   15
            */
            long k = 1;//当前节点，从1开始表示根节点
            for (long i = 0; i < depth - 1; i++)
            {
                if (index % 2 == 1)//左节点
                {
                    k *= 2;/*到下一个左节点*/
                    index = (index + 1) / 2;
                }
                else//右节点
                {
                    k = k * 2 + 1;
                    ;/*到下一个右节点*/
                    index /= 2;
                }
            }
            return k;
        }


        /// <summary>
        /// 遍历方法
        /// </summary>
        public enum OrderType
        {
            PreOrderTree,
            InOrderTree,
            PostOrderTree,
            WideOrderTree,
        }
        /// <summary>
        /// 子节点
        /// </summary>
        public enum Child
        {
            Left,
            Right,
        }

        /*
                                    1
                        2                       3
                  4          5           6           7
               8    9   10  11   12   13   14   15
        */
        /// <summary>
        /// 二叉树
        /// </summary>
        public TreeCore<T> Tree { get; set; }
        /// <summary>
        /// 初始化二叉树，树为空树
        /// </summary>
        public BinaryTree()
        {

        }
        /// <summary>
        /// 初始化二叉树，仅有一个根节点
        /// </summary>
        public BinaryTree(T node)
        {
            Tree = new TreeCore<T>(node);
        }
        /// <summary>
        /// 初始化二叉树
        /// </summary>
        /// <param name="treeArray">层序二叉树数组</param>
        public BinaryTree(T[] treeArray)
        {
            //Tree = new TreeClass<T>(treeArray[0]);
            //for (int i = 1; i < treeArray.Length; i++)
            //{
            //    Tree.Insert(treeArray[i]);
            //}
            Rebuild(treeArray);
        }
        /// <summary>
        /// 四种方法遍历二叉树
        /// </summary>
        /// <param name="type"></param>
        /// <param name="writeToConsole"></param>
        /// <returns></returns>
        public List<T> OrderTree(OrderType type, bool writeToConsole = false)
        {

            List<T> list = new List<T>();
            switch (type)
            {
                case OrderType.PreOrderTree:
                    PreOrderTree(Tree, ref list);
                    break;
                case OrderType.InOrderTree:
                    InOrderTree(Tree, ref list);
                    break;
                case OrderType.PostOrderTree:
                    PostOrderTree(Tree, ref list);
                    break;
                case OrderType.WideOrderTree:
                    list = WideOrderTree();
                    break;
            }
            if (writeToConsole)
            {
                foreach (var i in list)
                {
                    Console.Write(i);
                }
                Console.WriteLine();
            }
            return list;
        }
        /// <summary>
        /// 层序遍历二叉树，不跳过空节点
        /// </summary>
        /// <returns>节点数组，空节点为null</returns>
        public List<object> OrderTree()
        {
            int index = 0;//有效项的个数
            List<object> nodeList = new List<object>
                {
                    null,//第0个作废
                   Tree//第一个是根节点
                };
            TreeCore<T> temp = null;
            int count = Count;//有效项的总个数
            int n = 0;//总项的个数
            while (index < count)
            {
                n++;
                if (nodeList[n] == null)//如果当前节点不存在
                {
                    //for (int i = 0; i < (Math.Log(n) / Math.Log(2)); i++)
                    //{
                    nodeList.Add(null);
                    nodeList.Add(null);
                    //}
                    continue;
                }
                temp = nodeList[n] as TreeCore<T>;
                if (temp.LeftTree != null)
                {
                    nodeList.Add(temp.LeftTree);
                }
                else
                {
                    nodeList.Add(null);
                }
                if (temp.RightTree != null)
                {
                    nodeList.Add(temp.RightTree);
                }
                else
                {
                    nodeList.Add(null);
                }
                index++;
            }
            // while(nodeList.RemoveAt())
            return nodeList;
        }
        /// <summary>
        /// 通过层序建立二叉树
        /// </summary>
        /// <param name="tree"></param>
        public void Rebuild(T[] tree)
        {
            Tree = new TreeCore<T>(tree[0]);
            for (int i = 1; i < tree.Length; i++)
            {
                List<Child> path = GetPath(i + 1);//获取每一个值应该在的位置
                var node = Tree;
                for (int j = 0; j < path.Count; j++)
                {
                    switch (path[j])
                    {
                        case Child.Left:
                            if (node.LeftTree == null)
                            {
                                node.LeftTree = new TreeCore<T>(tree[i]);
                            }
                            node = node.LeftTree;
                            break;
                        case Child.Right:
                            if (node.RightTree == null)
                            {
                                node.RightTree = new TreeCore<T>(tree[i]);
                            }
                            node = node.RightTree;
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 获取从根节点到标准序列中的值的路径
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static List<Child> GetPath(int index)
        {
            List<Child> result = new List<Child>();
            while (index != 1)
            {
                if (index % 2 == 0)
                {
                    result.Insert(0, Child.Left);
                    index /= 2;
                }
                else
                {
                    result.Insert(0, Child.Right);
                    index = (index - 1) / 2;
                }
            }
            return result;
        }
        /// <summary>
        /// 转换为层序遍历的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString("");
        }
        /// <summary>
        /// 转换为层序遍历的字符串
        /// </summary>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public string ToString(string split)
        {
            var list = OrderTree(OrderType.WideOrderTree);
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < list.Count - 1; i++)
            {
                str.Append(list[i] + split);
            }
            str.Append(list[list.Count - 1]);
            return str.ToString();
            //return base.ToString();
        }
        /// <summary>
        /// 转换为阶梯形的数组
        /// </summary>
        /// <returns></returns>
        public object[,] ToArray()
        {
            object[,] array = new object[Depth, (int)(Math.Pow(2, Depth + 1))];
            var nodeList = OrderTree();//获取线性数据
            int level;//当前的层数
            for (int i = 1; i < nodeList.Count; i++)
            {
                if ((level = (int)Math.Floor(Math.Log(i) / Math.Log(2))) >= Depth)//如果当前的层数超过了二叉树深度就跳出循环
                {
                    break;
                }
                int column = i - (int)Math.Pow(2, level);//当前的数量减去这一行前面的所有节点的个数
                                                         // int realColumn = (int)Math.Pow(2, depth - level) + column * (int)Math.Pow(2, depth - level + 1);
                array[level, column] = nodeList[i];
            }
            return array;
        }
        /// <summary>
        /// 转换为类似二叉树形状的数组，输出即是二叉树
        /// </summary>
        /// <returns></returns>
        public object[,] ToVisualArray()
        {
            object[,] array = new object[Depth, (int)(Math.Pow(2, Depth))];
            var nodeList = OrderTree();//获取线性数据
            int level;//当前的层数
            for (int i = 1; i < nodeList.Count; i++)
            {
                if ((level = (int)Math.Floor(Math.Log(i) / Math.Log(2))) >= Depth)//如果当前的层数超过了二叉树深度就跳出循环
                {
                    break;
                }
                int column = i - (int)Math.Pow(2, level);//当前的数量减去这一行前面的所有节点的个数
                                                         //真实列，指的是直观图中的列，第一个加数是首项的位置，后面都是列乘以每一列之间的空隙
                int realColumn = (int)Math.Pow(2, Depth - level - 1) + column * (int)Math.Pow(2, Depth - level);
                array[level, realColumn] = nodeList[i];
            }
            return array;
        }
        /// <summary>
        /// 二叉树最大深度
        /// </summary>
        public int Depth
        {
            get
            {
                int depth = GetDepth(Tree);
                return depth;
            }
        }
        /// <summary>
        /// 二叉树的节点总数
        /// </summary>
        public int Count
        {
            get => WideOrderTree().Count;

        }
        /// <summary>
        /// 前序遍历
        /// </summary>
        /// <param name="root"></param>
        private void PreOrderTree(TreeCore<T> root, ref List<T> output)
        {
            if (root != null)
            {
                //Console.Write(root.NodeData);
                output.Add(root.NodeData);
                PreOrderTree(root.LeftTree, ref output);
                PreOrderTree(root.RightTree, ref output);
            }
        }
        /// <summary>
        /// 中序遍历
        /// </summary>
        /// <param name="root"></param>
        private void InOrderTree(TreeCore<T> root, ref List<T> output)
        {
            if (root != null)
            {
                //InOrderTree(root.LeftTree);
                //Console.Write(root.NodeData);
                //InOrderTree(root.RightTree);
                PreOrderTree(root.LeftTree, ref output);
                output.Add(root.NodeData);
                PreOrderTree(root.RightTree, ref output);
            }
        }
        /// <summary>
        /// 后序遍历
        /// </summary>
        /// <param name="root"></param>
        private void PostOrderTree(TreeCore<T> root, ref List<T> output)
        {
            if (root != null)
            {
                //PostOrderTree(root.LeftTree);
                //PostOrderTree(root.RightTree);
                PreOrderTree(root.LeftTree, ref output);
                PreOrderTree(root.RightTree, ref output);
                output.Add(root.NodeData);
                // Console.Write(root.NodeData);

            }
        }
        /// <summary>
        /// 逐层遍历
        /// </summary>
        public List<T> WideOrderTree()
        {
            List<T> result = new List<T>();
            List<TreeCore<T>> nodeList = new List<TreeCore<T>>
                {
                    Tree
                };
            TreeCore<T> temp = null;
            while (nodeList.Count > 0)
            {
                //Console.Write(nodeList[0].NodeData);
                result.Add(nodeList[0].NodeData);
                temp = nodeList[0];
                nodeList.Remove(nodeList[0]);
                if (temp.LeftTree != null)
                {
                    nodeList.Add(temp.LeftTree);
                }
                if (temp.RightTree != null)
                {
                    nodeList.Add(temp.RightTree);
                }
            }
            //Console.WriteLine();
            return result;
        }
        /// <summary>
        /// 遍历二叉树来获取深度
        /// </summary>
        /// <param name="root"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        private int GetDepth(TreeCore<T> root)
        {
            return root == null ? 0 : Math.Max(GetDepth(root.LeftTree), GetDepth(root.RightTree)) + 1;
        }
        /// <summary>
        /// 二叉树的本身，继承关系，因为要递归所以要单独写
        /// </summary>
        /// <typeparam name="t"></typeparam>
        public class TreeCore<t> where t : IComparable<t>//where 指定T从IComparable<T>继承
        {
            /// <summary>
            /// 定义二叉树的根节点
            /// </summary>
            /// <param name="nodeValue">二叉树的根节点</param>
            public TreeCore(t nodeValue)
            {
                NodeData = nodeValue;
                LeftTree = null;
                RightTree = null;
            }
            /// <summary>
            /// 数据节点属性
            /// </summary>
            public t NodeData
            {
                get; set;
            }
            /// <summary>
            /// 左子树
            /// </summary>
            public TreeCore<t> LeftTree
            {
                get; set;
            }
            /// <summary>
            /// 右子树
            /// </summary>
            public TreeCore<t> RightTree
            {
                get; set;
            }
            /// <summary>
            /// 向二叉数中插入一个节点
            /// </summary>
            /// <param name="newItem"></param>
            public void InsertAsSort(t newItem)
            {
                t currentNodeValue = NodeData;
                if (currentNodeValue.CompareTo(newItem) > 0)
                {
                    if (LeftTree == null)
                    {
                        LeftTree = new TreeCore<t>(newItem);
                    }
                    else
                    {
                        LeftTree.InsertAsSort(newItem);
                    }
                }
                else
                {
                    if (RightTree == null)
                    {
                        RightTree = new TreeCore<t>(newItem);
                    }
                    else
                    {
                        RightTree.InsertAsSort(newItem);
                    }
                }
            }
        }
    }
}