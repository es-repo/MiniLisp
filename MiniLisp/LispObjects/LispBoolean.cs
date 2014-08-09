namespace MiniLisp.LispObjects
{
    public class LispBoolean : LispValue
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