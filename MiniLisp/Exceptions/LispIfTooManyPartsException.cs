namespace MiniLisp.Exceptions
{
    public class LispIfTooManyPartsException : LispEvaluationException
    {
        public LispIfTooManyPartsException(int partsCount)
            : base(string.Format("\"if\" has too many parts: {0}", partsCount))
        {
        }
    }
}