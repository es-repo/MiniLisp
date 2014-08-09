using System.Collections.Generic;

namespace MiniLisp.LispObjects
{
    public class LispPair : LispValue
    {
        public new KeyValuePair<LispObject, LispObject> Value 
        {
            get { return (KeyValuePair<LispObject, LispObject>)base.Value; }
        }

        public LispPair(KeyValuePair<LispObject, LispObject> pair) : base(pair)
        {
        }

        public override string ToString()
        {
            return string.Format("'({0} . {1})", Value.Key, Value.Value);
        }
    }
}
