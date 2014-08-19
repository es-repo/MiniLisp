using System.Collections.Generic;

namespace MiniLisp.Expressions
{
    public class LispNull : LispPair
    {
        public LispNull() : base(new KeyValuePair<LispValueElement, LispValueElement>(null, null))
        {
        }
    }
}
