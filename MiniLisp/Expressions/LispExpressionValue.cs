namespace MiniLisp.Expressions
{
    public class LispExpressionValue : LispValueElement
    {
        public LispExpressionValue(LispExpression value)
            : base(value)
        {
        }

        public override string ToString()
        {
            return "'" + Value;
        }
    }
}