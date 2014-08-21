namespace MiniLisp.Expressions
{
    public class LispExpressionValue : LispValueElement
    {
        public new LispExpression Value { get { return (LispExpression)base.Value; } }

        public LispExpressionValue(LispExpression value)
            : base(value)
        {
        }

        public override string ToString()
        {
            bool addQuote = !(Value.Value is LispIdentifier || Value.Value is LispBoolean || Value.Value is LispNumber || Value.Value is LispString);
            return (addQuote ? "'" : "") + Value;
        }
    }
}