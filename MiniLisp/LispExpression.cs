using System.Collections.Generic;
using System.Linq;
using MiniLisp.Expressions;
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
            return ToString(this);
        }

        public static string ToString(LispExpression expression)
        {
            IEnumerable<string> i = Enumerable.Repeat(expression.Value.ToString(), expression.Value.ToString() == "" ? 0 : 1).Concat(expression.Select(e => e.ToString()));
            string s = string.Join(" ", i.ToArray());
            if (!(expression.Value is LispValueElement || expression.Value is LispIdentifier))
                s = "(" + s + ")";
            return s;
        }
    }
}