namespace MiniLisp.Exceptions
{
    public class LispProcedureBodyExpressionExpectedException : LispEvaluationException
    {
        public LispProcedureBodyExpressionExpectedException()
            : base(string.Format("Procedure body expresion is expected."))
        {
        }
    }
}
