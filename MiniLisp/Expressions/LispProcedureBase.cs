using System;

namespace MiniLisp.Expressions
{
    public abstract class LispProcedureBase : LispValueElement
    {
        public LispProcedureSignature Signature { get; set; }

        protected LispProcedureBase(LispProcedureSignature signature, object value) : base(value)
        {
            if (signature == null)
                throw new ArgumentNullException("signature");

            Signature = signature;
        }

        public override string ToString()
        {
            return string.Format("#<{0}:{1}>", TypeToString(GetType()), Signature.Identifier);
        }
    }
}
