using System;
using System.Linq;
using MiniLisp.Exceptions;
using MiniLisp.LispObjects;
using MiniLisp.Trees;

namespace MiniLisp
{
    public class Evaluator
    {
        private readonly DefenitionsCollection _defenitions;

        public Evaluator()
        {
            _defenitions = new DefenitionsCollection();
            new DefaultDefenitions().Fill(_defenitions);
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
                            return EvalDefine(objects);
                        
                        if (!(firstObj is LispProcedure))
                            throw new LispProcedureExpectedException(firstObj);

                        return EvalProcedure(objects);
                    }
                    
                    if (objects != null && objects.Length > 0)
                        throw new InvalidOperationException("Expected no arguments.");

                    if (lispObject is LispIdentifier)
                    {
                        bool identiferRightAfterDefine = expr.IndexAmongSiblings == 1 && expr.ParentNode.Children[0].Value is LispDefine;
                        if (!identiferRightAfterDefine)
                        {
                            LispIdentifier identifier = (LispIdentifier) lispObject;
                            if (!_defenitions.Contains(identifier))
                                throw new LispUnboundIdentifierException(identifier);
                            lispObject = _defenitions[identifier];
                        }
                    }
                    
                    return lispObject;
                });
        }

        private LispValue EvalProcedure(LispObject[] objects)
        {
            LispProcedure procedure = (LispProcedure)objects[0];
            LispValue[] args = objects.Skip(1).Where(o => !(o is LispVoid)).Cast<LispValue>().ToArray();
            LispProcedureContractVerification.Assert(procedure.Signature, args);
            return procedure.Value(args);
        }

        private LispVoid EvalDefine(LispObject[] objects)
        {
            if (objects.Length == 1 || !(objects[1] is LispIdentifier))
                throw new LispIdentifierExpectedException(objects.Length == 1 ? null : objects[1]);

            LispIdentifier identifier = (LispIdentifier)objects[1];

            if (objects.Length > 3)
                throw new LispMultipleExpressionsException(identifier);

            LispObject value = objects[2];
            if (!(value is LispValue))
                throw new LispValueExpectedException(value);

            _defenitions.Add(identifier, (LispValue)value);
            return new LispVoid();
        }
    }
}