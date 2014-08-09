namespace MiniLisp.LispObjects
{
    public class LispIdentifier : LispObject
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