namespace MiniLisp.Expressions
{
    public class LispNumber : LispValueElement
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