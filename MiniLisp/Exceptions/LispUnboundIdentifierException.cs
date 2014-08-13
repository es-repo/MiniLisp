using MiniLisp.LispExpressionElements;

namespace MiniLisp.Exceptions
{
    public class LispUnboundIdentifierException : LispEvaluationException
    {
        public LispUnboundIdentifierException(LispIdentifier identifier)
            : base("Unbound identifier: " + identifier.Value)
        {
        }
    }
}
