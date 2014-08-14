using System.Collections.Generic;
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

        public bool Contains(LispIdentifier identifier)
        {
            return Contains(identifier.Value);
        }

        public bool Contains(string identifier)
        {
            return _defenitions.ContainsKey(identifier) || (Parent != null && Parent.Contains(identifier));
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
                    : Parent != null
                        ? Parent[identifier]
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
