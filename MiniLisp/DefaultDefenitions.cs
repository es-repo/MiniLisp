using System.Linq;
using MiniLisp.LispObjects;

namespace MiniLisp
{
    public class DefaultDefenitions
    {
        public void Fill(DefenitionsCollection defenitionsCollection)
        {
            defenitionsCollection.Add("+", new LispProcedure(
                new LispProcedureSignature(new LispProcedureArgumentTypes(typeof(LispNumber))), 
                Sum));

            defenitionsCollection.Add("-", new LispProcedure(
                new LispProcedureSignature(new LispProcedureArgumentTypes(typeof(LispNumber)), 1, true),
                Sub));

            defenitionsCollection.Add("*", new LispProcedure(
                new LispProcedureSignature(new LispProcedureArgumentTypes(typeof(LispNumber))),
                Mul));

            defenitionsCollection.Add("/", new LispProcedure(
                new LispProcedureSignature(new LispProcedureArgumentTypes(typeof(LispNumber)), 1, true),
                Div));
        }

        private LispValue Sum(LispValue[] arguments)
        {
            return new LispNumber(arguments.Cast<LispNumber>().Sum(o => o.Value));
        }

        private LispValue Sub(LispValue[] arguments)
        {
            LispNumber firstArg = (LispNumber) arguments[0];
            if (arguments.Length == 1)
                return new LispNumber(-firstArg.Value);

            return new LispNumber(firstArg.Value - arguments.Skip(1).Cast<LispNumber>().Sum(o => o.Value));
        }

        private LispValue Mul(LispValue[] arguments)
        {
            return new LispNumber(arguments.Select(o => ((LispNumber) o).Value).Aggregate(1.0, (n1, n2) => n1*n2));
        }

        private LispValue Div(LispValue[] arguments)
        {
            double first = ((LispNumber) arguments[0]).Value;
            return new LispNumber(arguments.Skip(1).Select(o => ((LispNumber)o).Value).Aggregate(first, (n1, n2) => n1 / n2));
        }
    }
}