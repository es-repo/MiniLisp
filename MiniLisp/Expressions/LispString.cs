namespace MiniLisp.Expressions
{
    public class LispString : LispValueElement
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