using System;
using System.Collections.Generic;
using System.Linq;
using MiniLisp.LispObjects;
using MiniLisp.Trees;

namespace MiniLisp
{
    public class Evaluator
    {
        private readonly IDictionary<string, LispObject> _defenitions;

        public Evaluator()
        {
            _defenitions = new Dictionary<string, LispObject>(new DefaultDefenitions());
        }

        public LispObject Eval(LispExpression expression)
        {
            return Tree<LispObject>.Fold(expression,
                (TreeNodeInfo<LispObject> expr, LispObject[] objects) =>
                {
                    LispObject lispObject = expr.Node.Value;
                    if (lispObject is LispEval)
                    {
                        if (objects.Length == 0)
                            throw new LispProcedureExpectedException();

                        LispObject firstObj = objects[0];

                        if (firstObj is LispDefine)
                        {
                            // TODO:
                            return new LispVoid();
                        }
                        
                        if (!(firstObj is LispIdentifier))
                            throw new LispProcedureExpectedException(firstObj);

                        LispIdentifier identifier = (LispIdentifier) firstObj;
                        if (!_defenitions.ContainsKey((identifier.Value)))
                            throw new LispUnboundIdentifierException(identifier);

                        LispObject definedObject = _defenitions[identifier.Value];
                        if (!(definedObject is LispProcedure))
                            throw new LispProcedureExpectedException(definedObject);

                        LispProcedure procedure = (LispProcedure) definedObject;
                        LispObject[] args = objects.Skip(1).Where(o => !(o is LispVoid)).ToArray();
                        LispProcedureContractVerification.Assert(procedure.Signature, args);
                        return procedure.Value(args);
                    }
                    
                    if (objects != null && objects.Length > 0)
                            throw new InvalidOperationException("Expected no arguments.");
                    
                    return lispObject;
                });
        }
    }
}