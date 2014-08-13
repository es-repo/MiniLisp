using System;
using System.Collections.Generic;

namespace MiniLisp.LispObjects
{
    public class ProcedureParameterTypes
    {
        public IDictionary<int, Type> ParameterPositionAndType { get; private set; }
        public Type OtherParametersType { get; private set; }

        public ProcedureParameterTypes(Type otherArgumentsType)
            : this(null, otherArgumentsType)
        {
        }

        public ProcedureParameterTypes(IDictionary<int, Type> parameterPositionAndType, Type otherParametersType = null)
        {
            ParameterPositionAndType = parameterPositionAndType;
            OtherParametersType = otherParametersType;
        }
    }
}