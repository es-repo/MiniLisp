using System.Linq;
using MiniLisp.Expressions;

namespace MiniLisp
{
    public class DefaultDefenitions
    {
        public void Fill(Scope scope)
        {
            scope.Add("+", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispNumber)), 
                Sum));

            scope.Add("-", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispNumber), 1),
                Sub));

            scope.Add("*", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispNumber)),
                Mul));

            scope.Add("/", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispNumber), 1),
                Div));

            scope.Add("=",  new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispNumber), 2),
                MathEqaul));

            scope.Add("not", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispBoolean), 1),
                Not));

            // TODO: cos sin tan atan log exp sqrt > < >= <= != string? boolean? number? equal set! if
        }

        private LispValueElement Sum(LispValueElement[] arguments)
        {
            return new LispNumber(arguments.Cast<LispNumber>().Sum(o => o.Value));
        }

        private LispValueElement Sub(LispValueElement[] arguments)
        {
            LispNumber firstArg = (LispNumber) arguments[0];
            if (arguments.Length == 1)
                return new LispNumber(-firstArg.Value);

            return new LispNumber(firstArg.Value - arguments.Skip(1).Cast<LispNumber>().Sum(o => o.Value));
        }

        private LispValueElement Mul(LispValueElement[] arguments)
        {
            return new LispNumber(arguments.Select(o => ((LispNumber) o).Value).Aggregate(1.0, (n1, n2) => n1*n2));
        }

        private LispValueElement Div(LispValueElement[] arguments)
        {
            double first = ((LispNumber) arguments[0]).Value;
            return new LispNumber(arguments.Skip(1).Select(o => ((LispNumber)o).Value).Aggregate(first, (n1, n2) => n1 / n2));
        }

        private LispValueElement MathEqaul(LispValueElement[] arguments)
        {
            double first = ((LispNumber)arguments[0]).Value;
            return new LispBoolean(arguments.Skip(1).Select(o => ((LispNumber)o).Value).All(n => n == first));
        }

        private LispValueElement Not(LispValueElement[] arguments)
        {
            return new LispBoolean(!((LispBoolean)arguments[0]).Value);
        }
    }
}