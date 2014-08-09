using System;

namespace MiniLisp.LispObjects
{
    public class LispProcedure : LispValue
    {
        public LispProcedureSignature Signature { get; set; }

        public new Func<LispValue[], LispValue> Value 
        {
            get { return (Func<LispValue[], LispValue>)base.Value; }
        }

        public LispProcedure(LispProcedureSignature signature, Func<LispValue[], LispValue> value)
            : base(value)
        {
            Signature = signature;
        }

        public override string ToString()
        {
            return string.Format("#<{0}:{1}>", TypeToString(GetType()), Signature.Identifier);
        }
    }
}