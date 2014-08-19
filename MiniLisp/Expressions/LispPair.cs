using System.Collections.Generic;
using System.Linq;

namespace MiniLisp.Expressions
{
    public class LispPair : LispValueElement
    {
        public new KeyValuePair<LispValueElement, LispValueElement> Value 
        {
            get { return (KeyValuePair<LispValueElement, LispValueElement>)base.Value; }
        }

        public LispPair(LispValueElement key, LispValueElement value)
            : this(new KeyValuePair<LispValueElement, LispValueElement>(key, value))
        {
        }

        public LispPair(KeyValuePair<LispValueElement, LispValueElement> pair)
            : base(pair)
        {
        }

        public override string ToString()
        {
            List<object> list = new List<object>();
            LispValueElement pair = this;
            while (pair is LispPair)
            {
                list.Add(((LispPair)pair).Value.Key);
                pair = ((LispPair)pair).Value.Value;
            }

            if (pair != null)
            {
                list.Add(".");
                list.Add(pair);
            }

            return "(" + string.Join(" ", list.Where(e => e != null).ToArray()) + ")";
        }
    }
}
