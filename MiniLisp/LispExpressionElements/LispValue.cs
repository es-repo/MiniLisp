namespace MiniLisp.LispExpressionElements
{
    public abstract class LispValue : LispExpressionElement
    {
        public object Value { get; private set; }

        protected LispValue(object value)
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

            if (GetType() != obj.GetType() || !(obj is LispValue))
                return false;

            LispValue lispValue = (LispValue)obj;

            if (Value == null)
                return lispValue.Value == null;

            return Value.Equals(lispValue.Value);
        }
    }
}
