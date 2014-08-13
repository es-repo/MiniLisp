using System;
using MiniLisp.Expressions;

namespace MiniLisp
{
    public abstract class LispExpressionElement
    {
        public override string ToString()
        {
            return TypeToString(GetType());
        }
                        
        public static string TypeToString(Type type)
        {
            if (!typeof(LispExpressionElement).IsAssignableFrom(type))
                throw new ArgumentException("Type should be LispObject or derived from LispObject", "type");

            if (type == typeof (LispNumber))
            {
                return "number";
            }

            if (type == typeof(LispString))
            {
                return "string";
            }

            if (type == typeof(LispBoolean))
            {
                return "boolean";
            }

            if (type == typeof(LispExpressionValue))
            {
                return "expression";
            }

            if (typeof(LispProcedureBase).IsAssignableFrom(type))
            {
                return "procedure";
            }

            if (type == typeof(LispNil))
            {
                return "nil";
            }

            if (type == typeof(LispVoid))
            {
                return "void";
            }

            if (type == typeof (LispDefine))
            {
                return "define";
            }

            if (type == typeof (LispLambda))
            {
                return "lambda";
            }

            if (type == typeof (LispEval))
            {
                return "";
            }

            if (type == typeof(LispProcedureSignatureElement))
            {
                return "";
            }

            throw new NotImplementedException();
        }
    }
}