namespace MiniLisp.Exceptions
{
    public class LispIfPartExpectedException : LispEvaluationException
    {
        public LispIfPartExpectedException(string missedPart)
            : base(string.Format("\"if\" has missing an \"{0}\" expression.", missedPart))
        {
        }
    }
}