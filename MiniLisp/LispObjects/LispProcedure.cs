using System;

namespace MiniLisp.LispObjects
{
    public class LispProcedure : LispProcedureBase
    {
        public LispProcedureParameters Parameters { get; private set; }

        public LispExpression[] Body 
        {
            get { return (LispExpression[])Value; }
        }

        public LispProcedure(LispProcedureSignature signature, LispProcedureParameters parameters, LispExpression[] value)
            : base(signature, value)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            Parameters = parameters;
        }
    }
}
