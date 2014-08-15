namespace MiniLisp.Expressions
{
    public abstract class LispValueElement : LispExpressionElement
    {
        public object Value { get; private set; }

        protected LispValueElement(object value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
