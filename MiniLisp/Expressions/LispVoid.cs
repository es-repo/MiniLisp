namespace MiniLisp.Expressions
{
    public class LispVoid : LispValueElement
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
