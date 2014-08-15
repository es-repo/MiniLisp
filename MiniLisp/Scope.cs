using System.Collections.Generic;
using MiniLisp.Exceptions;
using MiniLisp.Expressions;

namespace MiniLisp
{
    public class Scope
    {
        private readonly IDictionary<string, LispValueElement> _variables;

        public Scope Parent { get; private set; }

        public Scope(Scope parent = null)
        {
            Parent = parent; 
            _variables = new Dictionary<string, LispValueElement>();
        }

        public LispValueElement this[LispIdentifier identifier]
        {
            get { return this[identifier.Value]; }
            set { this[identifier.Value] = value; }
        }

        public LispValueElement this[string identifier]
        {
            get
            {
                if (_variables.ContainsKey(identifier))
                    return _variables[identifier];
                if (Parent != null)
                    return Parent[identifier];

                throw new LispUnboundIdentifierException(identifier);
            }
            set
            {
                if(_variables.ContainsKey(identifier))
                {
                    _variables[identifier] = value;
                    ElementAdded(identifier, value);
                }
                else if (Parent != null)
                    Parent[identifier] = value;
                else
                    throw new LispUnboundIdentifierException(identifier);
            }
        }

        public void Add(LispIdentifier identifier, LispValueElement element)
        {
            Add(identifier.Value, element);
        }

        public void Add(string identifier, LispValueElement element)
        {
            if (_variables.ContainsKey(identifier))
                throw new LispDuplicateIdentifierDefinitionException(identifier);
            _variables.Add(identifier, element);
            ElementAdded(identifier, element);
        }

        private static void ElementAdded(string identifier, LispValueElement element)
        {
            if (element is LispProcedureBase)
            {
                LispProcedureBase procedure = (LispProcedureBase)element;
                procedure.Signature.Identifier = identifier;
            }
        }
    }
}
