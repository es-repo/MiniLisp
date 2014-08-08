using System;
using MiniLisp.LispObjects;

namespace MiniLisp
{
    public abstract class LispObject
    {        
        public object Value { get; private set; }

        protected LispObject(object value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 1) ^ GetType().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType() || !(obj is LispObject))
                return false;

            LispObject lispObj = (LispObject) obj;

            if (Value == null )
                return lispObj.Value == null;

            return Value.Equals(lispObj.Value);
        }

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