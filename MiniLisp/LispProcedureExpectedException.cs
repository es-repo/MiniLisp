namespace MiniLisp
{
    public class LispProcedureExpectedException : LispEvaluationException
    {
        public LispProcedureExpectedException(LispObject given = null)
            : base(string.Format("Expected a procedure.\r\ngiven:{0}", given != null ? given.ToString() : ""))
        {
        }
    }
}
