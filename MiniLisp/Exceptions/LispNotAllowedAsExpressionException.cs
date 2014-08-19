namespace MiniLisp.Exceptions
{
    public class LispNotAllowedAsExpressionException : LispEvaluationException
    {
        public LispNotAllowedAsExpressionException(string what)
            : base(string.Format("\"{0}\" not allowed as an expression.", what))
        {
        }
    }
}