using MiniLisp.Trees;

namespace MiniLisp
{
    public class LispExpression : TreeNode<LispObject>
    {
        public LispExpression(LispObject value) : base(value)
        {
        }
    }
}