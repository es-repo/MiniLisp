namespace MiniLisp.LispObjects
{
    public class LispLambdaArgs : LispValue
    {
        public new LispExpression Value 
        {
            get { return (LispExpression)base.Value; }
        }

        public LispLambdaArgs(LispExpression expression) 
            : base(expression)
        {
        }
    }
}
