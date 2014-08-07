namespace MiniLisp
{
    public class LispEvaluationException : LispException
    {
        public LispEvaluationException(string message = null) : base(message)
        {
        }
    }
}