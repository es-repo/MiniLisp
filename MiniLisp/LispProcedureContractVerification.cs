using System;
using MiniLisp.Exceptions;
using MiniLisp.Expressions;

namespace MiniLisp
{
    public static class LispProcedureContractVerification
    {        
        public static void Assert(LispProcedureSignature signature, LispExpressionElement[] arguments)
        {
            if (signature.Arity > -1 && ((!signature.AtLeastArity && signature.Arity != arguments.Length) || signature.Arity > arguments.Length))
                throw new LispProcedureArityMismatchException(signature.Identifier, arguments.Length, signature.Arity, signature.AtLeastArity);
            
            Type[] expectedTypes = new Type[arguments.Length];
            int i = 0;
            if (signature.NamedParameters != null)
            {
                for (; i < signature.NamedParameters.Length; i++)
                {
                    expectedTypes[i] = signature.NamedParameters[i].Type;
                }
            }

            if (signature.NonNamedParametersType != null)
            {
                for (; i < arguments.Length; i++)
                {
                    expectedTypes[i] = signature.NonNamedParametersType;
                }
            }

            for (i = 0; i < arguments.Length; i++)
            {
                if (expectedTypes[i] != null && arguments[i] != null && !expectedTypes[i].IsInstanceOfType(arguments[i]))
                {
                    throw new LispProcedureContractViolationException(signature.Identifier, expectedTypes[i], arguments[i], i);
                }
            }
        }
    }
}