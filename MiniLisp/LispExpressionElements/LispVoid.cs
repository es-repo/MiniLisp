namespace MiniLisp.LispExpressionElements
{
    public class LispVoid : LispValue
    {
        public LispVoid()
            : base(null)
        {
        }

        public override string ToString()
        {
            return "";
        }
    }
}
