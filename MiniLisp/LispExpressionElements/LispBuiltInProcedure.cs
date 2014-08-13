using System;

namespace MiniLisp.LispExpressionElements
{
    public class LispBuiltInProcedure : LispProcedureBase
    {
        public new Func<LispValue[], LispValue> Value 
        {
            get { return (Func<LispValue[], LispValue>)base.Value; }
        }

        public LispBuiltInProcedure(ProcedureSignature signature, Func<LispValue[], LispValue> value)
            : base(signature, value)
        {
        }
    }
}