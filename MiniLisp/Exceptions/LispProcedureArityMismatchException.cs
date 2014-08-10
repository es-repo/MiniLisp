namespace MiniLisp.Exceptions
{
    public class LispProcedureArityMismatchException : LispEvaluationException
    {
        public LispProcedureArityMismatchException(string procedureIdentifier, int givenArity, int expectedArity, bool atLeast = false)
            : base(string.Format("{0}: arity mismatch. Expected: {1} {2}. Given: {3}", procedureIdentifier, atLeast ? "at least" : "", expectedArity, givenArity))
        {
        }
    }
}