using System;

namespace MiniLisp.Expressions
{
    public class LispProcedureParameter
    {
        public string Identifier { get; private set; }
        public Type Type { get; private set; }

        public LispProcedureParameter(string identifier, Type type)
        {
            Identifier = identifier;
            Type = type;
        }
    }
}