namespace MiniLisp.Exceptions
{
    public class LispDuplicateIdentifierDefinitionException : LispEvaluationException
    {
        public LispDuplicateIdentifierDefinitionException(string identifierl)
            : base(string.Format("Duplicate definition for identifier: {0}", identifierl))
        {
        }
    }
}