namespace MiniLisp.Exceptions
{
    public class LispIdentifierExpectedException : LispEvaluationException
    {
        public LispIdentifierExpectedException(LispExpressionElement given = null)
            : base(string.Format("Expected an identifier. Given: {0}", given != null ? given.ToString() : ""))
        {
        }
    }
}
