namespace MiniLisp.LispExpressionElements
{
    public class LispExpressionValue : LispValue
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