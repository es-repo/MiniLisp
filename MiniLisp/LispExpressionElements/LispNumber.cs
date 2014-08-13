namespace MiniLisp.LispExpressionElements
{
    public class LispNumber : LispValue
    {
        public new double Value 
        {
            get { return (double) base.Value; }
        }

        public LispNumber(double value) : base(value)
        {
        }
    }
}