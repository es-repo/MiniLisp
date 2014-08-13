namespace MiniLisp.LispExpressionElements
{
    public class LispBoolean : LispValue
    {
        public new bool Value
        {
            get { return (bool)base.Value; }
        }

        public LispBoolean(bool value)
            : base(value)
        {
        }

        public override string ToString()
        {
            return Value ? "#t" : "#f";
        }
    }
}