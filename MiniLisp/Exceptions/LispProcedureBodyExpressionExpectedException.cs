namespace MiniLisp.Exceptions
{
    public class LispProcedureBodyExpressionExpectedException : LispEvaluationException
    {
        public LispProcedureBodyExpressionExpectedException()
            : base(string.Format("Procedure body expression is expected."))
        {
        }
    }
}
