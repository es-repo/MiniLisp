using MiniLisp.LispObjects;

namespace MiniLisp
{
    public class LispUnboundIdentifierException : LispEvaluationException
    {
        public LispUnboundIdentifierException(LispIdentifier identifier)
            : base("Unbound identifier: " + identifier.Value)
        {
        }
    }
}
