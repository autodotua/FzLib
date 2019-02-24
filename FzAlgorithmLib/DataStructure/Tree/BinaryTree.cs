using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Algorithm.DataStructure.Tree
{
    public class BinaryTree<T>:BinaryTreeBase<T,BinaryTree<T>.Node>
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
        /// 子节点
        /// </summary>
        public enum Child
        {
            Left,
            Right,
        }


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
            Root = new Node(node);
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
        /// 通过层序建立二叉树
        /// </summary>
        /// <param name="tree"></param>
        public void Rebuild(T[] tree)
        {
            Root = new Node(tree[0]);
            for (int i = 1; i < tree.Length; i++)
            {
                List<Child> path = GetPath(i + 1);//获取每一个值应该在的位置
                var node = Root;
                for (int j = 0; j < path.Count; j++)
                {
                    switch (path[j])
                    {
                        case Child.Left:
                            if (node.LeftChild == null)
                            {
                                node.LeftChild = new Node(tree[i]);
                            }
                            node = node.LeftChild;
                            break;
                        case Child.Right:
                            if (node.RightChild == null)
                            {
                                node.RightChild = new Node(tree[i]);
                            }
                            node = node.RightChild;
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
        /// 二叉树的本身，继承关系，因为要递归所以要单独写
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Node : BinaryTreeNodeBase<T, Node>
        {
            public Node()
            {

            }
            /// <summary>
            /// 定义二叉树的根节点
            /// </summary>
            /// <param name="nodeValue">二叉树的根节点</param>
            public Node(T nodeValue)
            {
                Data = nodeValue;
                LeftChild = null;
                RightChild = null;
            }

        }
    }
}