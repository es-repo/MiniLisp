using System;

namespace MiniLisp
{
    public class LispProcedureContract
    {
        public string ProcedureIdentifier { get; set; }
        public int Arity { get; private set; }
        public bool AtLeastArity { get; private set; }
        public LispProcedureArgumentTypes ArgumentTypes { get; set; }

        public LispProcedureContract(string procedureIdentifier, LispProcedureArgumentTypes argumentTypes = null, int arity = -1, bool atLeastArity = false)
        {
            ProcedureIdentifier = procedureIdentifier;
            ArgumentTypes = argumentTypes;
            Arity = arity;
            AtLeastArity = atLeastArity;
        }

        public void Assert(LispObject[] arguments)
        {
            if (Arity > -1 && ((AtLeastArity && Arity > arguments.Length) || Arity != arguments.Length))
                throw new LispProcedureArityMismatchException(ProcedureIdentifier, arguments.Length, Arity, AtLeastArity);
            
            if (ArgumentTypes != null)
            {
                if (ArgumentTypes.ArgumentPositionAndType != null)
                {
                    foreach (int pos in ArgumentTypes.ArgumentPositionAndType.Keys)
                    {
                        Type expectedType = ArgumentTypes.ArgumentPositionAndType[pos];
                        if (expectedType != arguments[pos].GetType())
                            throw new LispProcedureContractViolationException(ProcedureIdentifier, expectedType, arguments[pos], pos);
                    }
                }

                if (ArgumentTypes.OtherArgumentsType != null)
                {

                }
            }
        }
    }
}