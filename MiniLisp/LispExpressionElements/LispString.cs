namespace MiniLisp.LispExpressionElements
{
    public class LispString : LispValue
    {
        public LispString(string value) : base(value)
        {
        }

        public override string ToString()
        {
            return string.Format("\"{0}\"", Value);
        }
    }
}