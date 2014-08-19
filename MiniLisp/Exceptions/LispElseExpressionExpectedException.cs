namespace MiniLisp.Exceptions
{
    public class LispExpressionsInElseExpectedException : LispEvaluationException
    {
        public LispExpressionsInElseExpectedException()
            : base("Missing expressions in \"else\" clause.")
        {
        }
    }
}