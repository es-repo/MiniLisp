using System;
using System.Collections.Generic;

namespace MiniLisp
{
    public class LispProcedureArgumentTypes
    {
        public IDictionary<int, Type> ArgumentPositionAndType { get; private set; }
        public Type OtherArgumentsType { get; private set; }

        public LispProcedureArgumentTypes(Type otherArgumentsType)
            : this(null, otherArgumentsType)
        {
        }

        public LispProcedureArgumentTypes(IDictionary<int, Type> argumentPositionAndType, Type otherArgumentsType = null)
        {
            ArgumentPositionAndType = argumentPositionAndType;
            OtherArgumentsType = otherArgumentsType;
        }
    }
}