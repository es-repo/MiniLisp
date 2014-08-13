using System;
using System.Collections.Generic;

namespace MiniLisp.Expressions
{
    public class LispProcedureParameterTypes
    {
        public IDictionary<int, Type> ParameterPositionAndType { get; private set; }
        public Type OtherParametersType { get; private set; }

        public LispProcedureParameterTypes(Type otherArgumentsType)
            : this(null, otherArgumentsType)
        {
        }

        public LispProcedureParameterTypes(IDictionary<int, Type> parameterPositionAndType, Type otherParametersType = null)
        {
            ParameterPositionAndType = parameterPositionAndType;
            OtherParametersType = otherParametersType;
        }
    }
}