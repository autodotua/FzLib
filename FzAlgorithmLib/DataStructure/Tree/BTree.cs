using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Algorithm.DataStructure.Tree
{
    public class BTree<T> : TreeBase<T, BTree<T>.Node>
    {
        public Comparer<T> Comparer { get; set; } = Comparer<T>.Default;

        public const int ElementCountPerNode = 4;
        public class Node : TreeNodeBase<T, Node>
        {
            public int DataCount { get; set; } = 0;
            public override IList<Node> Children { get; } = new Node[ElementCountPerNode + 1];
            public new T[] Data { get; } = new T[ElementCountPerNode];
        }

        public void BTreeSplitNode(Node FatherNode, int position, Node node)
        {
            Node newNode = new Node();//创建新节点，容纳分裂后被移动的元素
            newNode.IsLeaf = node.IsLeaf;//新节点的层级和原节点位于同一层
            newNode.DataCount = ElementCountPerNode - (ElementCountPerNode / 2 + 1);//新节点元素的个数大约为分裂节点的一半
            for (int i = 1; i < ElementCountPerNode - (ElementCountPerNode / 2 + 1); i++)
            {
                //将原页中后半部分复制到新页中
                newNode.Data[i - 1] = node.Data[i + ElementCountPerNode / 2];
            }
            if (!node.IsLeaf)//如果不是叶子节点，将指针也复制过去
            {
                for (int j = 1; j < ElementCountPerNode / 2 + 1; j++)
                {
                    newNode.Children[j - 1] = node.Children[ElementCountPerNode / 2];
                }
            }
            node.DataCount = ElementCountPerNode / 2;//原节点剩余元素个数

            //将父节点指向子节点的指针向后推一位
            for (int k = FatherNode.DataCount + 1; k > position + 1; k--)
            {
                FatherNode.Children[k] = FatherNode.Children[k - 1];
            }
            //将父节点的元素向后推一位
            for (int k = FatherNode.DataCount; k > position + 1; k--)
            {
                FatherNode.Data[k] = FatherNode.Data[k - 1];
            }
            //将被分裂的页的中间节点插入父节点
            FatherNode.Data[position - 1] = node.Data[ElementCountPerNode / 2];
            //父节点元素大小+1
            FatherNode.DataCount += 1;
            //将FatherNode,NodeToBeSplit,newNode写回磁盘,三次IO写操作

        }

        public void BTreeInsertNotFull(Node Node, T KeyWord)
        {
            int i = Node.DataCount;
            //如果是叶子节点，则寻找合适的位置直接插入
            if (Node.IsLeaf)
            {

                while (i >= 1 && Comparer.Compare(KeyWord, Node.Data[i - 1]) < 0)
                {
                    Node.Data[i] = Node.Data[i - 1];//所有的元素后推一位
                    i -= 1;
                }
                Node.Data[i ] = KeyWord;//将关键字插入节点
                Node.DataCount += 1;
                //将节点写入磁盘，IO写+1
            }
            //如果是非叶子节点
            else
            {
                while (i >= 1 && Comparer.Compare(KeyWord, Node.Data[i - 1]) < 0)
                {
                    i -= 1;
                }
                //这步将指针所指向的节点读入内存,IO读+1
                if (Node.Children[i].DataCount == ElementCountPerNode)
                {
                    //如果子节点已满，进行节点分裂
                    BTreeSplitNode(Node, i, Node.Children[i]);

                }
                if (Comparer.Compare(KeyWord, Node.Data[i - 1]) > 0)
                {
                    //根据关键字的值决定插入分裂后的左孩子还是右孩子
                    i += 1;
                }
                //迭代找叶子，找到叶子节点后插入
                BTreeInsertNotFull(Node.Children[i], KeyWord);


            }
        }
        public void BtreeInsert(T KeyWord)
        {
            if (Root == null)
            {
                Root = new Node();
                Root.IsLeaf = true;
            }
            if (Root.DataCount == ElementCountPerNode)
            {

                //如果根节点满了，则对跟节点进行分裂
                Node newRoot = new Node();
                newRoot.DataCount = 0;
                newRoot.IsLeaf = false;
                //将newRoot节点变为根节点
                BTreeSplitNode(newRoot, 1, Root);
                //分裂后插入新根的树
                BTreeInsertNotFull(newRoot, KeyWord);
                //将树的根进行变换
                Root = newRoot;
            }
            else
            {
                //如果根节点没有满，直接插入
                BTreeInsertNotFull(Root, KeyWord);
            }
        }
    }
}
