using System;
using MiniLisp.Exceptions;
using MiniLisp.LispExpressionElements;

namespace MiniLisp
{
    public static class LispProcedureContractVerification
    {        
        public static void Assert(ProcedureSignature signature, LispExpressionElement[] arguments)
        {
            if (signature.Arity > -1 && ((!signature.AtLeastArity && signature.Arity != arguments.Length) || signature.Arity > arguments.Length))
                throw new LispProcedureArityMismatchException(signature.Identifier, arguments.Length, signature.Arity, signature.AtLeastArity);

            if (signature.ArgumentTypes != null)
            {
                Type[] expectedTypes = new Type[arguments.Length];
                
                for (int i = 0; i < expectedTypes.Length; i++)
                    expectedTypes[i] = signature.ArgumentTypes.OtherParametersType;
                
                if (signature.ArgumentTypes.ParameterPositionAndType != null)
                {
                    foreach (int pos in signature.ArgumentTypes.ParameterPositionAndType.Keys)
                    {
                        expectedTypes[pos] = signature.ArgumentTypes.ParameterPositionAndType[pos];
                    }
                }

                for (int i = 0; i < arguments.Length; i++)
                {
                    if (expectedTypes[i] != null && expectedTypes[i] != arguments[i].GetType())
                    {
                        throw new LispProcedureContractViolationException(signature.Identifier, expectedTypes[i], arguments[i], i);
                    }
                }
            }
        }
    }
}