using System;

namespace MiniLisp.Exceptions
{
    public class LispProcedureContractViolationException : LispEvaluationException
    {
        public LispProcedureContractViolationException(string procedureIdentifier, Type expectedType, LispObject givenObject, int argumentPosition)
            : base (string.Format("{0}: contract violation. Expected: {1}. Given: {2}. Argument position: {3}",
                procedureIdentifier, LispObject.TypeToString(expectedType), givenObject, argumentPosition))
        {
        }
    }
}