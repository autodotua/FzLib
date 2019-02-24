using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Algorithm.DataStructure.Tree
{
    public abstract class TreeNodeBase<TData, TNode> where TNode : TreeNodeBase<TData, TNode>
    {
        public virtual TData Data { get; set; }
        public virtual TNode Parent { get; set; }


        public virtual IList<TNode> Children { get; } = new List<TNode>();

        public virtual bool IsLeaf { get; set; }
    }
}
