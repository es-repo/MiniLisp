namespace MiniLisp.Expressions
{
    public class LispIdentifier : LispValueElement
    {
        public new string Value { get { return (string)base.Value; } }

        public LispIdentifier(string value) 
            : base(value)
        {
        }
    }
}