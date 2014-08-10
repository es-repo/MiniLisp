namespace MiniLisp.Exceptions
{
    public class LispParsingException : LispException
    {
        public LispParsingException(string message)
            : base(message)
        {
        }
    }
}