using System.Collections.Generic;
using MiniLisp.LispObjects;

namespace MiniLisp
{
    public class DefenitionsCollection
    {
        private readonly IDictionary<string, LispValue> _defenitions;

        public DefenitionsCollection()
        {
            _defenitions = new Dictionary<string, LispValue>();
        }

        public bool Contains(LispIdentifier identifier)
        {
            return Contains(identifier.Value);
        }

        public bool Contains(string identifier)
        {
            return _defenitions.ContainsKey(identifier);
        }

        public LispObject this[LispIdentifier identifier]
        {
            get { return this[identifier.Value]; }
        }

        public LispObject this[string identifier]
        {
            get { return Contains(identifier) ? _defenitions[identifier] : null; }
        }

        public void Add(LispIdentifier identifier, LispValue lispObject)
        {
            Add(identifier.Value, lispObject);
        }

        public void Add(string identifier, LispValue lispObject)
        {
            if (lispObject is LispProcedureBase)
            {
                LispProcedureBase procedure = (LispProcedureBase)lispObject;
                procedure.Signature.Identifier = identifier;
            }

            _defenitions.Add(identifier, lispObject);
        }
    }
}
