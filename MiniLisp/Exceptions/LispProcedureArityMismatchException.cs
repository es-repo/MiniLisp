namespace MiniLisp.Exceptions
{
    public class LispProcedureArityMismatchException : LispEvaluationException
    {
        public LispProcedureArityMismatchException(string procedureIdentifier, int givenArity, int expectedArity, bool atLeast = false)
            : base(string.Format("{0}: arity mismatch\r\nexpected: {1} {2}\r\ngiven: {3}", procedureIdentifier, atLeast ? "at least" : "", expectedArity, givenArity))
        {
        }
    }
}