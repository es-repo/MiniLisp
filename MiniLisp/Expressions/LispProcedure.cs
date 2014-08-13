using System;

namespace MiniLisp.Expressions
{
    public class LispProcedure : LispProcedureBase
    {
        //TODO: parameters => signature
        public LispProcedureSignatureElement Parameters { get; private set; }

        public Scope Scope { get; private set; }

        public LispExpression[] Body 
        {
            get { return (LispExpression[])Value; }
        }

        public LispProcedure(LispProcedureSignatureElement signature, LispExpression[] value, Scope scope = null)
            : base(new LispProcedureSignature(), value)
        {
            if (signature == null)
                throw new ArgumentNullException("signature");

            Parameters = signature;
            Scope = scope;
        }

        public LispProcedure Copy(Scope scope)
        {
            return new LispProcedure(Parameters, Body, scope);
        }
    }
}
