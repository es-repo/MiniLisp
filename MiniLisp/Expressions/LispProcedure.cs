namespace MiniLisp.Expressions
{
    public class LispProcedure : LispProcedureBase
    {        
        public Scope Scope { get; private set; }

        public LispExpression[] Body 
        {
            get { return (LispExpression[])Value; }
        }

        public LispProcedure(LispProcedureSignature signature, LispExpression[] value, Scope scope = null)
            : base(signature, value)
        {
            Scope = scope;
        }

        public LispProcedure Copy(Scope scope)
        {
            return new LispProcedure(Signature, Body, scope);
        }
    }
}
