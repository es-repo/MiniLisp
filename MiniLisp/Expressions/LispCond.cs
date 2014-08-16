using System.Collections.Generic;

namespace MiniLisp.Expressions
{
    public class LispCond : LispExpressionElement
    {
        public KeyValuePair<LispProcedure, LispProcedure>[] Body { get; private set; }

        public LispCond()
        {
        }

        public LispCond(KeyValuePair<LispProcedure, LispProcedure>[] body)
        {
            Body = body;
        }
    }
}