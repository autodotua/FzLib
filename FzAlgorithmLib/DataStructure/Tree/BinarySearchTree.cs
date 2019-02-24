using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Algorithm.DataStructure.Tree
{
    public class BinarySearchTree<T> : BinaryTreeBase<T, BinarySearchTree<T>.Node>
    {
        public Comparer<T> Comparer { get; set; } = Comparer<T>.Default;
        public BinarySearchTree()
        {
        }
        public void Add(T data)
        {
            if (Root == null)
            {
                Root = new Node();
                Root.Data = data;
                return;
            }
            Node node = Root;
            while (true)
            {
                switch (Comparer.Compare(data, node.Data))
                {
                    case int i when i > 0:
                        if (node.RightChild == null)
                        {
                            node.SetRightChild(data);
                            return;
                        }
                        node = node.RightChild;
                        break;
                    case int i when i < 0:
                        if (node.LeftChild == null)
                        {
                            node.SetLeftChild(data);
                            return;
                        }
                        node = node.LeftChild;

                        break;
                    default:
                        return;
                }
            }
        }

        public Node Search(T data)
        {
            if (Root == null)
            {
                return null;
            }
            Node node = Root;
            while (true)
            {
                switch (Comparer.Compare(data, node.Data))
                {
                    case int i when i > 0:
                        node = node.RightChild;
                        break;
                    case int i when i < 0:
                        node = node.LeftChild;
                        break;
                    default:
                        return node;
                }
                if (node == null)
                {
                    return null;
                }
            }


        }

        public void Remove(Node node)
        {
            if (node.IsLeaf)//叶子节点
            {
                if (node == Root)
                {
                    Root = null;
                }
                else if (node.Parent.LeftChild == node)
                {
                    node.Parent.LeftChild = null;
                }
                else if (node.Parent.RightChild == node)
                {
                    node.Parent.RightChild = null;
                }
                else
                {
                    throw new Exception("树结构有问题");
                }
            }
            else if (node.LeftChild == null || node.RightChild == null)//只有一个孩子的节点
            {
                Node childNode = node.LeftChild ?? node.RightChild;
                if (node == Root)
                {
                    Root = Root.LeftChild ?? Root.RightChild;
                    Root.Parent = null;
                }
                else if (node.Parent.LeftChild == node)
                {
                    node.Parent.LeftChild = childNode;
                }
                else if (node.Parent.RightChild == node)
                {
                    node.Parent.RightChild = childNode;
                }
                else
                {
                    throw new Exception("树结构有问题");
                }
            }
            else
            {
                T data = FindMaxInLeftTree(node.LeftChild);
                node.Data = data;
                Remove(node.LeftChild);
            }
        }
        private T FindMaxInLeftTree(Node left)
        {
            if (left == null)
            {
                throw new Exception();
            }
            if (left.RightChild == null)
            {
                return left.Data;
            }
            if (left.RightChild == null && left.LeftChild == null)
            {
                return left.Data;
            }
            return FindMaxInLeftTree(left.RightChild);
        }

        private void Replace(Node newNode)
        {
        }

        public Node MinNode
        {
            get
            {
                Node node = Root;
                while (node != null)
                {
                    node = node.LeftChild;
                }
                return node;
            }
        }

        public Node MaxNode
        {
            get
            {
                Node node = Root;
                while (node != null)
                {
                    node = node.RightChild;
                }
                return node;
            }
        }

        public class Node : BinaryTreeNodeBase<T, Node>
        {

        }
    }
}
