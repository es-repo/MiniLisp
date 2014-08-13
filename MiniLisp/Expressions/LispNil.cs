namespace MiniLisp.Expressions
{
    public class LispNil : LispValueElement
    {
        public LispNil() : base(null)
        {
        }
        public override string ToString()
        {
            return TypeToString(GetType());
        }
    }
}
