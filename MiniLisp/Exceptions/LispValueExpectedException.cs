namespace MiniLisp.Exceptions
{
    public class LispValueExpectedException : LispEvaluationException
    {
        public LispValueExpectedException(LispObject given = null)
            : base(string.Format("Expected a value object.\r\ngiven:{0}", given != null ? given.ToString() : ""))
        {
        }
    }
}
