using System.Collections.Generic;
using System.Text;
using MiniLisp.Trees;

namespace MiniLisp
{
    public class LispExpression : TreeNode<LispExpressionElement>
    {        
        public LispExpression(LispExpressionElement value) : base(value)
        {
        }

        public override string ToString()
        {
            IEnumerable<TreeNodeInfo<LispExpressionElement>> nodes = Tree<LispExpressionElement>.TraverseDepthFirstPreOrder(this);
            StringBuilder s = new StringBuilder();
            int currentDepth = 0;
            foreach (TreeNodeInfo<LispExpressionElement> ni in nodes)
            {
                if (currentDepth > ni.Depth)
                {
                    s.Append(new string(')', currentDepth - ni.Depth));
                    currentDepth = ni.Depth;
                }

                if (s.Length > 0 && s[s.Length-1] != '(')
                {
                    s.Append(" ");
                }

                if (ni.Node.Value is ILispParentObject)
                {
                    s.Append("(" + ni.Node.Value);
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