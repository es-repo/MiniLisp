namespace MiniLisp.Exceptions
{
    public class LispProcedureSignatureExpressionExpectedException : LispEvaluationException
    {
        public LispProcedureSignatureExpressionExpectedException(LispExpressionElement given = null)
            : base("Procedure signature expresion is expected." + (given != null ? given.ToString() : ""))
        {
        }
    }
}
