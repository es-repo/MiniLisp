using System.Collections.Generic;
using System.Linq;
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
            bool wasEval = false;
            foreach (TreeNodeInfo<LispObject> ni in nodes)
            {
                if (ni.IndexAmongSiblings > 0)
                {
                    s.Append(" ");
                }

                if (currentDepth > ni.Depth)
                {
                    s.Append(")");
                }

                if (ni.Node.Value is LispEval)
                {
                    s.Append("(");
                    wasEval = true;
                }
                else
                {
                    s.Append(ni.Node.Value);
                }

                currentDepth = ni.Depth;
            }

            if (wasEval)
                s.Append(")");

            return s.ToString();
        }
    }
}