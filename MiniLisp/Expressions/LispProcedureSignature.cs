using System;

namespace MiniLisp.Expressions
{
    public class LispProcedureSignature
    {        
        public string Identifier { get; set; }
        public int Arity { get; private set; }
        public bool AtLeastArity { get { return NonNamedParametersType != null; } }
        public LispProcedureParameter[] NamedParameters { get; private set; }
        public Type NonNamedParametersType { get; private set; }

        public LispProcedureSignature()
            : this(new LispProcedureParameter[]{})
        {
        }

        public LispProcedureSignature(LispProcedureParameter[] namedParameters, Type nonNamedParametersType, int arity = -1)
            : this(null, namedParameters, nonNamedParametersType, arity)
        {
        }

        public LispProcedureSignature(LispProcedureParameter[] namedParameters)
            : this(null, namedParameters)
        {
        }

        public LispProcedureSignature(string identifier, LispProcedureParameter[] namedParameters)
            : this(identifier, namedParameters, null, namedParameters != null ? namedParameters.Length : -1) 
        {
        }

        public LispProcedureSignature(string identifier, LispProcedureParameter[] namedParameters, Type nonNamedParametersType, int arity = -1)
        {
            Identifier = identifier;
            Arity = arity;
            NamedParameters = namedParameters;
            NonNamedParametersType = nonNamedParametersType;
        }
    }
}