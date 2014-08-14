namespace MiniLisp.Exceptions
{
    public class LispProcedureSignatureExpressionExpectedException : LispEvaluationException
    {
        public LispProcedureSignatureExpressionExpectedException(LispExpressionElement given = null)
            : base("Procedure signature expression is expected." + (given != null ? given.ToString() : ""))
        {
        }
    }
}
