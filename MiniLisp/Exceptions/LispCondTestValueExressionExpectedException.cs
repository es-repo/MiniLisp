namespace MiniLisp.Exceptions
{
    public class LispCondTestValueExressionExpectedException : LispEvaluationException
    {
        public LispCondTestValueExressionExpectedException()
            : base("Cond has not a test-value pair.")
        {
        }
    }
}