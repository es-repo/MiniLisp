using System;

namespace MiniLisp.LispObjects
{
    public class LispProcedure : LispObject
    {
        public string Identifier { get; private set; }

        public LispProcedure(string identifier, Func<LispObject[], LispObject> value) : base(value)
        {
            Identifier = identifier;
        }

        public override string ToString()
        {
            return string.Format("#<{0}:{1}>", TypeToString(GetType()), Identifier);
        }
    }
}