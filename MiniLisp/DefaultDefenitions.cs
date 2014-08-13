using System.Linq;
using MiniLisp.LispObjects;

namespace MiniLisp
{
    public class DefaultDefenitions
    {
        public void Fill(Scope scope)
        {
            scope["+"] = new LispBuiltInProcedure(
                new ProcedureSignature(new ProcedureParameterTypes(typeof(LispNumber))), 
                Sum);

            scope["-"] = new LispBuiltInProcedure(
                new ProcedureSignature(new ProcedureParameterTypes(typeof(LispNumber)), 1, true),
                Sub);

            scope["*"] = new LispBuiltInProcedure(
                new ProcedureSignature(new ProcedureParameterTypes(typeof(LispNumber))),
                Mul);

            scope["/"] = new LispBuiltInProcedure(
                new ProcedureSignature(new ProcedureParameterTypes(typeof(LispNumber)), 1, true),
                Div);

            scope["="] =  new LispBuiltInProcedure(
                new ProcedureSignature(new ProcedureParameterTypes(typeof(LispNumber)), 2, true),
                MathEqaul);

            scope["not"] = new LispBuiltInProcedure(
                new ProcedureSignature(new ProcedureParameterTypes(typeof(LispBoolean)), 1),
                Not);

            // TODO: cos sin tan atan log exp sqrt > < != string? boolean? number? equal
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

        private LispValue MathEqaul(LispValue[] arguments)
        {
            double first = ((LispNumber)arguments[0]).Value;
            return new LispBoolean(arguments.Skip(1).Select(o => ((LispNumber)o).Value).All(n => n == first));
        }

        private LispValue Not(LispValue[] arguments)
        {
            return new LispBoolean(!((LispBoolean)arguments[0]).Value);
        }
    }
}