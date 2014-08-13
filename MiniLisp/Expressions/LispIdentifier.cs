namespace MiniLisp.Expressions
{
    public class LispIdentifier : LispExpressionElement
    {
        public string Value { get; private set; }

        public LispIdentifier(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}