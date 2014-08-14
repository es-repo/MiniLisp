namespace MiniLisp.Exceptions
{
    public class LispUnboundIdentifierException : LispEvaluationException
    {
        public LispUnboundIdentifierException(string identifier)
            : base("Unbound identifier: " + identifier)
        {
        }
    }
}
