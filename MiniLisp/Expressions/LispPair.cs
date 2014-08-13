using System.Collections.Generic;

namespace MiniLisp.Expressions
{
    public class LispPair : LispValueElement
    {
        public new KeyValuePair<LispExpressionElement, LispExpressionElement> Value 
        {
            get { return (KeyValuePair<LispExpressionElement, LispExpressionElement>)base.Value; }
        }

        public LispPair(KeyValuePair<LispExpressionElement, LispExpressionElement> pair) : base(pair)
        {
        }

        public override string ToString()
        {
            return string.Format("'({0} . {1})", Value.Key, Value.Value);
        }
    }
}
