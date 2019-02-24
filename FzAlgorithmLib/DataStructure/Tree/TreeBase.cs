using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Algorithm.DataStructure.Tree
{
   public abstract class TreeBase<TData, TNode> where TNode : TreeNodeBase<TData, TNode>
    {
        public TNode Root { get; protected set; }
    }
}
