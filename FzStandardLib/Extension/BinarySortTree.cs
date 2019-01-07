using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Extension
{
    public class BinarySortTree<T>
    {
        public BinarySortTree(Func<T, IComparable> key)
        {
            Key = key;
        }

        public BinarySortTree()
        {
            if (typeof(T).GetInterface(nameof(IComparable)) == null)
            {
                throw new Exception("只有当类型实现" + nameof(IComparable) + "接口才可以调用无参构造函数");
            }
            Key = p => (IComparable)p;
        }

        private bool IsLeftSmallerThanOrEqualToRight(T left, T right) => Key(left).CompareTo(Key(right)) <= 0;

        public Func<T, IComparable> Key { get; private set; }
        private BinaryTreeNode root = null; //创建二叉排序树的根节点

        public void Add(T item)
        {
            BinaryTreeNode newNode = new BinaryTreeNode(item);
            if (root == null)
            {
                root = newNode;
            }
            else
            {
                BinaryTreeNode temp = root;
                while (true)
                {
                    //放在temp的左边
                    if (Key(item).CompareTo(temp.Data) <= 0)
                    {
                        if (temp.LeftChild == null)
                        {
                            //父子相认
                            temp.LeftChild = newNode;
                            newNode.Parent = temp;
                            break;
                        }
                        else
                        {
                            temp = temp.LeftChild;
                        }
                    }
                    //放在temp的右边
                    else
                    {
                        if (temp.RightChild == null)
                        {
                            //父子相认
                            temp.RightChild = newNode;
                            newNode.Parent = temp;
                            break;
                        }
                        else
                        {
                            temp = temp.RightChild;
                        }
                    }
                }
            }
        }

        //中序遍历，使得二叉排序树，从小到大输出。
        public T[] ToArray()
        {
            List<T> list = new List<T>();
            MiddleTraversal(root);
            void MiddleTraversal(BinaryTreeNode node)
            {
                if (node == null)
                {
                    return;
                }
                MiddleTraversal(node.LeftChild);
                list.Add(node.Data);
                MiddleTraversal(node.RightChild);

            }

            return list.ToArray();
        }


        //查找是否存在指定元素
        public bool FindItem(T item)
        {
            //递归方式查找
            //return FindItem(item,root);
            //循环方式查找
            return FindByWhile(item, root);
        }

        //递归方式查找
        private bool FindItem(T item, BinaryTreeNode node)
        {
            if (node == null)
                return false;
            if (Key(node.Data).CompareTo(Key(item)) == 0)
            {
                return true;
            }
            else if (Key(node.Data).CompareTo(Key(item)) > 0)
            {
                return FindItem(item, node.LeftChild);
            }
            else
            {
                return FindItem(item, node.RightChild);
            }
        }
        //循环方式进行查找
        private bool FindByWhile(T item, BinaryTreeNode node)
        {
            BinaryTreeNode temp = root;
            //下列代码逻辑上有返回值
            while (true)
            {
                if (temp == null) return false;
                else if (Key(node.Data).CompareTo(Key(item)) == 0) return true;
                else if (Key(node.Data).CompareTo(Key(item)) < 0)
                    temp = temp.RightChild;
                else
                    temp = temp.LeftChild;
            }
        }

        public bool DeleteBSTree(T item)
        {
            BinaryTreeNode temp = root;
            //下列代码逻辑上有返回值
            while (true)
            {
                if (temp == null)
                {
                    return false;
                }
                else if (Key(temp.Data).CompareTo(Key(item)) == 0)
                {
                    Delete(temp);
                }
                else if (Key(temp.Data).CompareTo(Key(item)) < 0)
                {
                    temp = temp.RightChild;
                }
                else
                {
                    temp = temp.LeftChild;
                }
            }
        }

        private void Delete(BinaryTreeNode node)
        {
            //1.叶子结点删除情况
            if (node.LeftChild == null && node.RightChild == null)
            {
                //删除为根节点
                if (node.Parent == null)
                {
                    root = null;
                }
                else if (node.Parent.LeftChild == node)
                {
                    //断开父子关系
                    node.Parent.LeftChild = null;
                }
                else if (node.Parent.RightChild == node)
                {
                    //断开父子关系
                    node.Parent.RightChild = null;
                }
                return;
            }
            //2.仅有右子树的结点
            else if (node.LeftChild == null && node.RightChild != null)
            {
                node.Data = node.RightChild.Data;
                node.RightChild = null;
                return;
            }
            //3.仅有左子树的结点
            else if (node.RightChild == null && node.LeftChild != null)
            {
                node.Data = node.LeftChild.Data;
                node.LeftChild = null;
                return;
            }
            //左右子树都有的结点
            else
            {
                BinaryTreeNode temp = node.RightChild;
                //找删除结点的右子树的最左边结点，即右子树的最小值
                while (true)
                {
                    if (temp.LeftChild != null)
                    {
                        temp = temp.LeftChild;
                    }
                    else
                    {
                        break;
                    }
                }
                //直接覆盖数据
                node.Data = temp.Data;
                //递归删除
                Delete(temp);
            }
        }



        public class BinaryTreeNode
        {
            public BinaryTreeNode LeftChild { get; set; }
            public BinaryTreeNode RightChild { get; set; }
            public BinaryTreeNode Parent { get; set; }
            public T Data { get; set; }

            public BinaryTreeNode()
            {

            }

            public BinaryTreeNode(T item)
            {
                Data = item;
            }
        }
    }
}
