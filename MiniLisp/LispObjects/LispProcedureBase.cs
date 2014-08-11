using System;
using System.Runtime.InteropServices;

namespace MiniLisp.LispObjects
{
    public abstract class LispProcedureBase : LispValue
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
