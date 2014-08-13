using System;

namespace MiniLisp.LispObjects
{
    public class LispProcedure : LispProcedureBase
    {
        //TODO: parameters => signature
        public LispProcedureSignature Parameters { get; private set; }

        public LispExpression[] Body 
        {
            get { return (LispExpression[])Value; }
        }

        public LispProcedure(LispProcedureSignature signature, LispExpression[] value)
            : base(new ProcedureSignature(), value)
        {
            if (signature == null)
                throw new ArgumentNullException("signature");

            Parameters = signature;
        }
    }
}
