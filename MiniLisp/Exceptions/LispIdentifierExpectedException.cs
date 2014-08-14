namespace MiniLisp.Exceptions
{
    public class LispIdentifierExpectedException : LispEvaluationException
    {
        public LispIdentifierExpectedException(LispExpressionElement given = null)
            : base("Expected an identifier." + (given != null ? " Given: " + given : ""))
        {
        }
    }
}
