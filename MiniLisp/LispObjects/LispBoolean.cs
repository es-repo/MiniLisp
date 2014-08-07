namespace MiniLisp.LispObjects
{
    public class LispBoolean : LispObject
    {
        public LispBoolean(bool value)
            : base(value)
        {
        }

        public override string ToString()
        {
            return ((bool) Value) ? "#t" : "#f";
        }
    }
}