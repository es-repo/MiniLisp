﻿using System.Collections.Generic;
using System.Text;
using MiniLisp.LispObjects;
using MiniLisp.Trees;

namespace MiniLisp
{
    public class LispExpression : TreeNode<LispObject>
    {        
        public LispExpression(LispObject value) : base(value)
        {
        }

        public override string ToString()
        {
            IEnumerable<TreeNodeInfo<LispObject>> nodes = Tree<LispObject>.TraverseDepthFirstPreOrder(this);
            StringBuilder s = new StringBuilder();
            int currentDepth = 0;
            foreach (TreeNodeInfo<LispObject> ni in nodes)
            {
                if (currentDepth > ni.Depth)
                {
                    s.Append(")");
                    currentDepth--;
                }

                if (ni.IndexAmongSiblings > 0)
                {
                    s.Append(" ");
                }

                if (ni.Node.Value is LispEval)
                {
                    s.Append("(");
                    currentDepth++;
                }
                else
                {
                    s.Append(ni.Node.Value);
                }
            }

            s.Append(new string(')', currentDepth));

            return s.ToString();
        }
    }
}