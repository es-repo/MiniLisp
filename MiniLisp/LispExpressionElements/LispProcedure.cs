using System;

namespace MiniLisp.LispExpressionElements
{
    public class LispProcedure : LispProcedureBase
    {
        //TODO: parameters => signature
        public LispProcedureSignature Parameters { get; private set; }

        public Scope Scope { get; private set; }

        public LispExpression[] Body 
        {
            get { return (LispExpression[])Value; }
        }

        public LispProcedure(LispProcedureSignature signature, LispExpression[] value, Scope scope = null)
            : base(new ProcedureSignature(), value)
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
