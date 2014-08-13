namespace MiniLisp.Exceptions
{
    public class LispMultipleExpressionsException : LispEvaluationException
    {
        public LispMultipleExpressionsException(LispExpressionElement after)
            : base("Expected single expression after " + after)
        {
        }
    }
}
