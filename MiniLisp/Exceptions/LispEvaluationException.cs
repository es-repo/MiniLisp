namespace MiniLisp.Exceptions
{
    public class LispEvaluationException : LispException
    {
        public LispEvaluationException(string message) : base (message)
        {
        }
    }
}