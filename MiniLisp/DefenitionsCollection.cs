using System.Collections.Generic;
using MiniLisp.LispObjects;

namespace MiniLisp
{
    public class DefenitionsCollection
    {
        private readonly IDictionary<string, LispObject> _defenitions;

        public DefenitionsCollection()
        {
            _defenitions = new Dictionary<string, LispObject>();
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

        public void Add(LispIdentifier identifier, LispObject lispObject)
        {
            Add(identifier.Value, lispObject);
        }

        public void Add(string identifier, LispObject lispObject)
        {
            if (lispObject is LispIdentifier)
            {
                LispIdentifier anotherIdentifier = (LispIdentifier)lispObject;
                if (!Contains(anotherIdentifier))
                    throw new LispUnboundIdentifierException(anotherIdentifier);

                lispObject = this[anotherIdentifier];
            }
            else if (lispObject is LispProcedure)
            {
                LispProcedure procedure = (LispProcedure) lispObject;
                procedure.Signature.Identifier = identifier;
            }

            _defenitions.Add(identifier, lispObject);
        }
    }
}
