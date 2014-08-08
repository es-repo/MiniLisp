using System;

namespace MiniLisp.Exceptions
{
    public class LispProcedureContractViolationException : LispEvaluationException
    {
        public LispProcedureContractViolationException(string procedureIdentifier, Type expectedType, LispObject givenObject, int argumentPosition)
            : base (string.Format("{0}: contract violation\r\nexpected: {1}\r\ngiven: {2}\r\nargument position: {3}",
                procedureIdentifier, LispObject.TypeToString(expectedType), givenObject, argumentPosition))
        {
        }
    }
}