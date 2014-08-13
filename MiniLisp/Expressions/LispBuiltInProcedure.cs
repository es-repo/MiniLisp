using System;

namespace MiniLisp.Expressions
{
    public class LispBuiltInProcedure : LispProcedureBase
    {
        public new Func<LispValueElement[], LispValueElement> Value 
        {
            get { return (Func<LispValueElement[], LispValueElement>)base.Value; }
        }

        public LispBuiltInProcedure(LispProcedureSignature signature, Func<LispValueElement[], LispValueElement> value)
            : base(signature, value)
        {
        }
    }
}