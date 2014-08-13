using System.Collections.Generic;
using MiniLisp.Expressions;

namespace MiniLisp
{
    public class Scope
    {
        private readonly IDictionary<string, LispValueElement> _defenitions;

        private readonly Scope _parent;

        public Scope(Scope parent = null)
        {
            _parent = parent; 
            _defenitions = new Dictionary<string, LispValueElement>();
        }

        public bool Contains(LispIdentifier identifier)
        {
            return Contains(identifier.Value);
        }

        public bool Contains(string identifier)
        {
            return _defenitions.ContainsKey(identifier) || (_parent != null && _parent.Contains(identifier));
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
                return _defenitions.ContainsKey(identifier)
                    ? _defenitions[identifier]
                    : _parent != null
                        ? _parent[identifier]
                        : null;
            }
            set
            {
                if (value is LispProcedureBase)
                {
                    LispProcedureBase procedure = (LispProcedureBase)value;
                    procedure.Signature.Identifier = identifier;
                }
                _defenitions[identifier] = value; 
            }
        }
    }
}
