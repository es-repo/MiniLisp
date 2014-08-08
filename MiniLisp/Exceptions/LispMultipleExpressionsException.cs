namespace MiniLisp.Exceptions
{
    public class LispMultipleExpressionsException : LispEvaluationException
    {
        public LispMultipleExpressionsException(LispObject after)
            : base("Expected single expression after " + after)
        {
        }
    }
}
