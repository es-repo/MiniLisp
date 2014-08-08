using System;

namespace MiniLisp.LispObjects
{
    public class LispProcedure : LispObject
    {
        public LispProcedureSignature Signature { get; set; }

        public new Func<LispObject[], LispObject> Value 
        {
            get { return (Func<LispObject[], LispObject>) base.Value; }
        }

        public LispProcedure(LispProcedureSignature signature, Func<LispObject[], LispObject> value)
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