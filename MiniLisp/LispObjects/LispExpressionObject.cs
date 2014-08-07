namespace MiniLisp.LispObjects
{
    public class LispExpressionObject : LispObject
    {
        public LispExpressionObject(LispExpression value)
            : base(value)
        {
        }

        public override string ToString()
        {
            return "'expression"; // TODO: convert expression to real s-expression
        }
    }
}