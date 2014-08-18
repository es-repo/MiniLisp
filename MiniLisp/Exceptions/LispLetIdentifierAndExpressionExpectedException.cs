namespace MiniLisp.Exceptions
{
    public class LispLetIdentifierAndExpressionExpectedException : LispEvaluationException
    {
        public LispLetIdentifierAndExpressionExpectedException(LispExpression given)
            : base(string.Format("\"Let\" expects an identifier and expression for a binding. Given: " + given))
        {
        }
    }
}