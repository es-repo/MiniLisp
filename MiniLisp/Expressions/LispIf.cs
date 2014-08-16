namespace MiniLisp.Expressions
{
    public class LispIf : LispExpressionElement
    {
        public LispProcedure Test { get; private set; }
        public LispProcedure Then { get; private set; }
        public LispProcedure Else { get; private set; }

        public LispIf()
        {
        }

        public LispIf(LispProcedure test, LispProcedure then, LispProcedure @else)
        {
            Test = test;
            Then = then;
            Else = @else;
        }
    }
}
