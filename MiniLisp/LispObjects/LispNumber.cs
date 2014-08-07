namespace MiniLisp.LispObjects
{
    public class LispNumber : LispObject
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