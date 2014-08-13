namespace MiniLisp.LispObjects
{
    public class ProcedureSignature
    {
        public string Identifier { get; set; }
        public int Arity { get; private set; }
        public bool AtLeastArity { get; private set; }
        public ProcedureParameterTypes ArgumentTypes { get; private set; }

        public ProcedureSignature(ProcedureParameterTypes argumentTypes = null, int arity = -1, bool atLeastArity = false)
            : this(null, argumentTypes, arity, atLeastArity)
        {
        }

        public ProcedureSignature(string identifier, ProcedureParameterTypes argumentTypes = null, int arity = -1, bool atLeastArity = false)
        {
            Identifier = identifier;
            ArgumentTypes = argumentTypes;
            Arity = arity;
            AtLeastArity = atLeastArity;
        }
    }
}