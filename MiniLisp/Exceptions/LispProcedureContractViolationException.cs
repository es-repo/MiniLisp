using System;

namespace MiniLisp.Exceptions
{
    public class LispProcedureContractViolationException : LispEvaluationException
    {
        public LispProcedureContractViolationException(string procedureIdentifier, Type expectedType, LispExpressionElement givenElement, int argumentPosition)
            : base (string.Format("{0}: contract violation. Expected: {1}. Given: {2}. Argument position: {3}",
                procedureIdentifier, LispExpressionElement.TypeToString(expectedType), givenElement, argumentPosition))
        {
        }
    }
}