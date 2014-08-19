using System;
using System.Collections.Generic;
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

            scope.Add("!=", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispNumber), 2),
                MathNotEqaul));

            scope.Add(">", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispNumber), 2),
                MathMore));

            scope.Add(">=", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispNumber), 2),
                MathMoreEqual));

            scope.Add("<", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispNumber), 2),
                MathLess));

            scope.Add("<=", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispNumber), 2),
                MathLessEqual));

            scope.Add("not", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispBoolean), 1),
                Not));

            scope.Add("or", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispBoolean), 1),
                Or));

            scope.Add("and", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispBoolean), 1),
                And));
            
            scope.Add("equal?", new LispBuiltInProcedure(
                new LispProcedureSignature(new []
                {
                    new LispProcedureParameter("", typeof(LispValueElement)), new LispProcedureParameter("", typeof(LispValueElement))
                }), Equal));

            scope.Add("cons", new LispBuiltInProcedure(
                new LispProcedureSignature(new[]
                {
                    new LispProcedureParameter("", typeof(LispValueElement)), new LispProcedureParameter("", typeof(LispValueElement))
                }), Cons));

            scope.Add("car", new LispBuiltInProcedure(
                new LispProcedureSignature(new[]
                {
                    new LispProcedureParameter("", typeof(LispPair))
                }), Car));

            scope.Add("cdr", new LispBuiltInProcedure(
                new LispProcedureSignature(new[]
                {
                    new LispProcedureParameter("", typeof(LispPair))
                }), Cdr));

            scope.Add("list", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispValueElement)), List));

            scope.Add("void", new LispBuiltInProcedure(
                new LispProcedureSignature(null, typeof(LispValueElement)), args => new LispVoid()));
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
            return MathConditional(arguments, (f, n) => f == n);
        }

        private LispValueElement MathNotEqaul(LispValueElement[] arguments)
        {
            return MathConditional(arguments, (f, n) => f != n);
        }

        private LispValueElement MathMore(LispValueElement[] arguments)
        {
            return MathConditional(arguments, (f, n) => f > n);
        }

        private LispValueElement MathMoreEqual(LispValueElement[] arguments)
        {
            return MathConditional(arguments, (f, n) => f >= n);
        }

        private LispValueElement MathLess(LispValueElement[] arguments)
        {
            return MathConditional(arguments, (f, n) => f < n);
        }

        private LispValueElement MathLessEqual(LispValueElement[] arguments)
        {
            return MathConditional(arguments, (f, n) => f <= n);
        }

        private LispValueElement MathConditional(LispValueElement[] arguments, Func<double, double, bool> predicate)
        {
            double first = ((LispNumber)arguments[0]).Value;
            return new LispBoolean(arguments.Skip(1).Select(o => ((LispNumber)o).Value).All(n => predicate(first, n)));
        }

        private LispValueElement Not(LispValueElement[] arguments)
        {
            return new LispBoolean(!((LispBoolean)arguments[0]).Value);
        }

        private LispValueElement Or(LispValueElement[] arguments)
        {
            return new LispBoolean(arguments.Aggregate(false, (r, a) => r || ((LispBoolean)a).Value));
        }

        private LispValueElement And(LispValueElement[] arguments)
        {
            return new LispBoolean(arguments.Aggregate(true, (r, a) => r && ((LispBoolean)a).Value));
        }

        private LispValueElement Equal(LispValueElement[] arguments)
        {
            bool r = arguments[0].GetType() == arguments[1].GetType() && arguments[0].Value.Equals(arguments[1].Value);
            return new LispBoolean(r);
        }

        private LispValueElement Cons(LispValueElement[] arguments)
        {
            return new LispPair(new KeyValuePair<LispValueElement, LispValueElement>(arguments[0], arguments[1]));
        }

        private LispValueElement Car(LispValueElement[] arguments)
        {
            return ((LispPair)arguments[0]).Value.Key;
        }

        private LispValueElement Cdr(LispValueElement[] arguments)
        {
            return ((LispPair)arguments[0]).Value.Value;
        }

        private LispPair List(LispValueElement[] arguments)
        {
            return arguments.Reverse().Aggregate(((LispPair)new LispNull()), (r, e) => new LispPair(e, r));
        }
    }
}