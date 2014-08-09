namespace MiniLisp.LispObjects
{
    public class LispExpressionObject : LispValue
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