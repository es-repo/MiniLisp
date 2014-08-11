using System;

namespace MiniLisp.LispObjects
{
    public class LispBuiltInProcedure : LispProcedureBase
    {
        public new Func<LispValue[], LispValue> Value 
        {
            get { return (Func<LispValue[], LispValue>)base.Value; }
        }

        public LispBuiltInProcedure(LispProcedureSignature signature, Func<LispValue[], LispValue> value)
            : base(signature, value)
        {
        }
    }
}