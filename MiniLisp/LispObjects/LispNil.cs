namespace MiniLisp.LispObjects
{
    public class LispNil : LispValue
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
