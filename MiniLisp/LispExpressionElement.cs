using System;
using System.Collections.Generic;
using MiniLisp.Expressions;

namespace MiniLisp
{
    public abstract class LispExpressionElement
    {
        private static readonly Dictionary<Type, string> _typeToStringMap;

        static LispExpressionElement()
        {
            _typeToStringMap = new Dictionary<Type, string>
            {
                {typeof(LispNumber), "number"},
                {typeof(LispString), "string"},
                {typeof(LispBoolean), "boolean"},
                {typeof(LispExpressionValue), "expression"},
                {typeof(LispBuiltInProcedure), "procedure"},
                {typeof(LispProcedure), "procedure"},
                {typeof(LispNull), "null"},
                {typeof(LispVoid), "void"},
                {typeof(LispDefine), "define"},
                {typeof(LispSet), "set!"},
                {typeof(LispIf), "if"},
                {typeof(LispCond), "cond"},
                {typeof(LispElse), "else"},
                {typeof(LispLambda), "lambda"},
                {typeof(LispLet), "let"},
                {typeof(LispEval), ""},
                {typeof(LispGroupElement), ""},
            };
        }

        public override string ToString()
        {
            return TypeToString(GetType());
        }
                        
        public static string TypeToString(Type type)
        {
            if (!typeof(LispExpressionElement).IsAssignableFrom(type))
                throw new ArgumentException("Type should be LispExpressionElement or derived from LispExpressionElement", "type");

            if (_typeToStringMap.ContainsKey(type))
                return _typeToStringMap[type];

            throw new NotImplementedException();
        }
    }
}