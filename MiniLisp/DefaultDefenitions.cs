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

            defenitionsCollection.Add("pi", new LispNumber(3.141592653589793));
        }

        private LispObject Sum(LispObject[] arguments)
        {
            return new LispNumber(arguments.Cast<LispNumber>().Sum(o => o.Value));
        }

        private LispObject Sub(LispObject[] arguments)
        {
           return new LispNumber(((LispNumber) arguments[0]).Value - arguments.Skip(1).Cast<LispNumber>().Sum(o => o.Value));
        }

        private LispObject Mul(LispObject[] arguments)
        {
            return new LispNumber(arguments.Select(o => ((LispNumber) o).Value).Aggregate(1.0, (n1, n2) => n1*n2));
        }

        private LispObject Div(LispObject[] arguments)
        {
            double first = ((LispNumber) arguments[0]).Value;
            return new LispNumber(arguments.Skip(1).Select(o => ((LispNumber)o).Value).Aggregate(first, (n1, n2) => n1 / n2));
        }
    }
}