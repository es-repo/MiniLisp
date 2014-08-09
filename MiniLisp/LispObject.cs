using System;
using MiniLisp.LispObjects;

namespace MiniLisp
{
    public abstract class LispObject
    {                                
        public static string TypeToString(Type type)
        {
            if (!typeof(LispObject).IsAssignableFrom(type))
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

            if (type == typeof(LispExpressionObject))
            {
                return "expression";
            }

            if (type == typeof(LispProcedure))
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

            throw new NotImplementedException();
        }
    }
}