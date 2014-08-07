namespace MiniLisp.LispObjects
{
    public class LispVoid : LispObject
    {
        public LispVoid()
            : base(null)
        {
        }

        public override string ToString()
        {
            return "#" + TypeToString(GetType());
        }
    }
}
