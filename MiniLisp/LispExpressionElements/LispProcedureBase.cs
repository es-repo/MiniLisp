using System;

namespace MiniLisp.LispExpressionElements
{
    public abstract class LispProcedureBase : LispValue
    {
        public ProcedureSignature Signature { get; set; }

        protected LispProcedureBase(ProcedureSignature signature, object value) : base(value)
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
