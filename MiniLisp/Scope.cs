using System.Collections.Generic;
using MiniLisp.Exceptions;
using MiniLisp.Expressions;

namespace MiniLisp
{
    public class Scope
    {
        private readonly IDictionary<string, LispValueElement> _defenitions;

        public Scope Parent { get; private set; }

        public Scope(Scope parent = null)
        {
            Parent = parent; 
            _defenitions = new Dictionary<string, LispValueElement>();
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
                if (_defenitions.ContainsKey(identifier))
                    return _defenitions[identifier];
                if (Parent != null)
                    return Parent[identifier];

                throw new LispUnboundIdentifierException(identifier);
            }
            set
            {
                if (!_defenitions.ContainsKey(identifier))
                    throw new LispUnboundIdentifierException(identifier);

                _defenitions[identifier] = value;
                ElementAdded(identifier, value);
            }
        }

        public void Add(LispIdentifier identifier, LispValueElement element)
        {
            Add(identifier.Value, element);
        }

        public void Add(string identifier, LispValueElement element)
        {
            if (_defenitions.ContainsKey(identifier))
                throw new LispDuplicateIdentifierDefinitionException(identifier);
            _defenitions.Add(identifier, element);
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
