namespace MiniLisp.Exceptions
{
    public class LispProcedureSignatureExpressionExpectedException : LispEvaluationException
    {
        public LispProcedureSignatureExpressionExpectedException(LispObject given = null)
            : base("Procedure signature expresion is expected." + (given != null ? given.ToString() : ""))
        {
        }
    }
}
