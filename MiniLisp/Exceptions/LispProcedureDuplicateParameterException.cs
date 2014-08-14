namespace MiniLisp.Exceptions
{
    public class LispProcedureDuplicateParameterException : LispEvaluationException
    {
        public LispProcedureDuplicateParameterException(string duplicatedIdentifier)
            : base("Duplicate argument name: " + duplicatedIdentifier)
        {
        }
    }
}
