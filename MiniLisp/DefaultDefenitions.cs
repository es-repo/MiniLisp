using System.Collections.Generic;
using System.Linq;
using MiniLisp.LispObjects;

namespace MiniLisp
{
    public class DefaultDefenitions : Dictionary<string, LispObject>
    {
        public DefaultDefenitions()
        {
            Add("+", new LispProcedure("+", Sum));
            Add("-", new LispProcedure("-", Sub));
            Add("*", new LispProcedure("*", Mul));
            Add("/", new LispProcedure("/", Div));
        }

        private LispObject Sum(LispObject[] arguments)
        {
            new LispProcedureContract("+", new LispProcedureArgumentTypes(typeof(LispNumber))).Assert(arguments);

            return new LispNumber(arguments.Cast<LispNumber>().Sum(o => o.Value));
        }

        private LispObject Sub(LispObject[] arguments)
        {
            new LispProcedureContract("-", new LispProcedureArgumentTypes(typeof(LispNumber)), 1, true).Assert(arguments);

            return new LispNumber(((LispNumber) arguments[0]).Value - arguments.Skip(1).Cast<LispNumber>().Sum(o => o.Value));
        }

        private LispObject Mul(LispObject[] arguments)
        {
            new LispProcedureContract("*", new LispProcedureArgumentTypes(typeof(LispNumber))).Assert(arguments);

            return new LispNumber(arguments.Select(o => ((LispNumber) o).Value).Aggregate(1.0, (n1, n2) => n1*n2));
        }

        private LispObject Div(LispObject[] arguments)
        {
            new LispProcedureContract("/", new LispProcedureArgumentTypes(typeof(LispNumber)), 1, true).Assert(arguments);

            double first = ((LispNumber) arguments[0]).Value;
            return new LispNumber(arguments.Skip(1).Select(o => ((LispNumber)o).Value).Aggregate(first, (n1, n2) => n1 / n2));
        }
    }
}