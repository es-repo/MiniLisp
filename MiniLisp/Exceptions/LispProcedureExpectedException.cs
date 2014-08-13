namespace MiniLisp.Exceptions
{
    public class LispProcedureExpectedException : LispEvaluationException
    {
        public LispProcedureExpectedException(LispExpressionElement given = null)
            : base(string.Format("Expected a procedure. Given: {0}", given != null ? given.ToString() : ""))
        {
        }
    }
}
