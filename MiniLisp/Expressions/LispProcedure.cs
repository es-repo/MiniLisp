namespace MiniLisp.Expressions
{
    public class LispProcedure : LispProcedureBase
    {        
        public Scope Scope { get; private set; }

        public LispExpression[] Body 
        {
            get { return (LispExpression[])Value; }
        }

        public LispProcedure(LispProcedureSignature signature, LispExpression[] body, Scope scope = null)
            : base(signature, body)
        {
            Scope = scope;
        }

        public LispProcedure Copy(Scope scope)
        {
            return new LispProcedure(Signature, Body, scope);
        }
    }
}
