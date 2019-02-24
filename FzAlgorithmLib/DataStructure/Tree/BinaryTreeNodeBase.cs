using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Algorithm.DataStructure.Tree
{
    public abstract class BinaryTreeNodeBase<TData, TNode>:TreeNodeBase<TData,TNode> where  TNode : BinaryTreeNodeBase<TData, TNode>,new()
    {
        public BinaryTreeNodeBase()
        {

        }
        public BinaryTreeNodeBase(TData data)
        {
            Data = data;
        }

        public BinaryTreeNodeBase(TData data, TNode left, TNode right) : this(data)
        {
            LeftChild = left;
            RightChild = right;
        }
        private TNode leftChild;
        private TNode rightChild;
        public virtual TNode LeftChild
        {
            get => leftChild;
            set
            {
                leftChild = value;
                if(value!=null)
                {
                    value.Parent = this as TNode;
                    value.right = false;
                }
            }
        }
        public virtual TNode RightChild
        {
            get => rightChild;
            set
            {
                rightChild = value;
                if (value != null)
                {
                    value.Parent = this as TNode;
                    value.right = true;
                }
            }
        }
        private bool? right = null;
        public bool IsLeft => right == false;
        public bool IsRight => right == true;
        public int Level
        {
            get
            {
                int level = 1;
                var node = this;
                while(node.Parent!=null)
                {
                    node = node.Parent;
                    level++;
                }
                return level;
            }
        }

        public override string ToString()
        {
            return Data.ToString();
        }

        public TNode SetLeftChild(TData data)
        {
            TNode node = new TNode();
            node.Data = data;
            LeftChild = node;
            return node;
        }
        public TNode SetRightChild(TData data)
        {
            TNode node = new TNode();
            node.Data = data;
            RightChild = node;
            return node;
        }

        public override bool IsLeaf => LeftChild == default && RightChild == default;

        public override IList<TNode> Children => new List<TNode>() { LeftChild, RightChild };
    }
}
