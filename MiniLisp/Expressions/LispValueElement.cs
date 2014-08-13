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

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 1) ^ GetType().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType() || !(obj is LispValueElement))
                return false;

            LispValueElement valueElement = (LispValueElement)obj;

            if (Value == null)
                return valueElement.Value == null;

            return Value.Equals(valueElement.Value);
        }
    }
}
