namespace MiniLisp.Exceptions
{
    public class LispElseMustBeLastException : LispEvaluationException
    {
        public LispElseMustBeLastException()
            : base("\"else\" clause must be last.")
        {
        }
    }
}