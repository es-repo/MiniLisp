namespace MiniLisp.Exceptions
{
    public class LispLetPartExpectedException : LispEvaluationException
    {
        public LispLetPartExpectedException(string missedPart)
            : base(string.Format("\"Let\" has missing an {0} expression.", missedPart))
        {
        }
    }
}