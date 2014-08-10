namespace MiniLisp.Exceptions
{
    public class LispProcedureExpectedException : LispEvaluationException
    {
        public LispProcedureExpectedException(LispObject given = null)
            : base(string.Format("Expected a procedure. Given: {0}", given != null ? given.ToString() : ""))
        {
        }
    }
}
