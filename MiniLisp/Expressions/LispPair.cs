using System.Collections.Generic;

namespace MiniLisp.Expressions
{
    public class LispPair : LispValueElement
    {
        public new KeyValuePair<LispValueElement, LispValueElement> Value 
        {
            get { return (KeyValuePair<LispValueElement, LispValueElement>)base.Value; }
        }

        public LispPair(KeyValuePair<LispValueElement, LispValueElement> pair)
            : base(pair)
        {
        }

        public override string ToString()
        {
            return string.Format("({0} . {1})", Value.Key, Value.Value);
        }
    }
}
