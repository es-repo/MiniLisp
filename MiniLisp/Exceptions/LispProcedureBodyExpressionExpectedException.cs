namespace MiniLisp.Exceptions
{
    public class LispProcedureBodyExpressionExpectedException : LispEvaluationException
    {
        public LispProcedureBodyExpressionExpectedException(string procedureIdentifier)
            : base(string.Format("{0}: Procedure body expresion is expected.", procedureIdentifier))
        {
        }
    }
}
