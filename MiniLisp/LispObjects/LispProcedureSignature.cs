namespace MiniLisp.LispObjects
{
    public class LispProcedureSignature
    {
        public string Identifier { get; set; }
        public int Arity { get; private set; }
        public bool AtLeastArity { get; private set; }
        public LispProcedureParameterTypes ArgumentTypes { get; private set; }

        public LispProcedureSignature(LispProcedureParameterTypes argumentTypes = null, int arity = -1, bool atLeastArity = false)
            : this(null, argumentTypes, arity, atLeastArity)
        {
        }

        public LispProcedureSignature(string identifier, LispProcedureParameterTypes argumentTypes = null, int arity = -1, bool atLeastArity = false)
        {
            Identifier = identifier;
            ArgumentTypes = argumentTypes;
            Arity = arity;
            AtLeastArity = atLeastArity;
        }
    }
}