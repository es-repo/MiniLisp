namespace MiniLisp.LispObjects
{
    public class LispIdentifier : LispObject
    {
        public new string Value { get { return (string)base.Value; } }

        public LispIdentifier(string value)
            : base(value)
        {
        }
    }
}