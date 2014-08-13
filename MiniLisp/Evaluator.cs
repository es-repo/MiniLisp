using System;
using System.Linq;
using MiniLisp.Exceptions;
using MiniLisp.LispObjects;
using MiniLisp.Trees;

namespace MiniLisp
{
    public class Evaluator
    {
        private readonly Scope _mainScope;

        public Evaluator()
        {
            _mainScope = new Scope();
            new DefaultDefenitions().Fill(_mainScope);
        }

        public LispObject Eval(LispExpression expression)
        {
            LispExpression foldedLambdas = Tree<LispObject>.Fold<LispExpression>(expression,
                (ni, children) =>
                {
                    if (ni.Node.Value is LispLambda)
                    {
                        if (children.Length < 1 || !(children[0].Value is LispProcedureSignature))
                            throw new LispProcedureSignatureExpressionExpectedException(children.Length > 0
                                ? children[0].Value
                                : null);

                        LispProcedureSignature signature = (LispProcedureSignature) children[0].Value;

                        if (children.Length < 2)
                            throw new LispProcedureBodyExpressionExpectedException();

                        LispExpression[] bodyExpressions = children.Skip(1).ToArray();

                        return new LispExpression(new LispProcedure(signature, bodyExpressions));
                    }

                    LispExpression e = new LispExpression(ni.Node.Value);
                    e.AddRange(children);
                    return e;
                });

            return Eval(foldedLambdas, _mainScope);
        }

        private LispObject Eval(LispExpression expression, Scope scope)
        {
            //TODO: duplicate definition for identifier
            // (define e 5) (define e 5)

            //TODO: (define fn (lambda (d) (define d 3) d)) - that's OK!

            //TODO: duplicate argument name
            // (define fn (lambda (d d) d))

            return Tree<LispObject>.Fold<LispObject>(expression,
                (ni, objects) =>
                {
                    LispObject lispObject = ni.Node.Value;
                    if (lispObject is LispEval)
                    {
                        if (objects.Length == 0)
                            throw new LispProcedureExpectedException();

                        LispObject firstObj = objects[0];

                        if (!(firstObj is LispProcedureBase))
                            throw new LispProcedureExpectedException(firstObj);

                        return firstObj is LispBuiltInProcedure 
                            ? EvalBuiltInProcedure(objects) 
                            : EvalProcedure(objects);
                    }
                    
                    if (lispObject is LispDefine)
                    {
                        return EvalDefine(objects, scope);
                    }

                    if (objects != null && objects.Length > 0)
                        throw new InvalidOperationException("Expected no arguments.");

                    if (lispObject is LispIdentifier)
                    {
                        bool passIdentifer = ni.ParentNode != null && ni.ParentNode.Value is LispDefine && ni.IndexAmongSiblings == 0;
                        if (!passIdentifer)
                        {
                            LispIdentifier identifier = (LispIdentifier) lispObject;
                            if (!scope.Contains(identifier))
                                throw new LispUnboundIdentifierException(identifier);
                            lispObject = scope[identifier];
                        }
                    }

                    if (lispObject is LispProcedure)
                    {
                        lispObject = ((LispProcedure) lispObject).Copy(new Scope(scope));
                    }

                    return lispObject;
                });
        }

        private LispValue EvalBuiltInProcedure(LispObject[] objects)
        {
            LispBuiltInProcedure procedure = (LispBuiltInProcedure)objects[0];
            //TODO: (+ define 4)
            //TODO: define inside expression is not allowed 
            LispValue[] args = objects.Skip(1).Where(o => !(o is LispVoid)).Cast<LispValue>().ToArray();
            LispProcedureContractVerification.Assert(procedure.Signature, args);
            return procedure.Value(args);
        }

        private LispObject EvalProcedure(LispObject[] objects)
        {
            LispProcedure procedure = (LispProcedure)objects[0];
            //TODO: check args
            //TODO: assert contract
            return procedure.Body.Aggregate((LispObject)null, (a, e) => Eval(e, procedure.Scope));
        }

        private LispVoid EvalDefine(LispObject[] objects, Scope scope)
        {
            LispObject firstObject = objects.Length > 0 ? objects[0] : null;
            if (!(firstObject is LispIdentifier))
                throw new LispIdentifierExpectedException(firstObject);

            LispIdentifier identifier = (LispIdentifier)firstObject;

            if (objects.Length > 2)
                throw new LispMultipleExpressionsException(identifier);

            LispObject value = objects.Length > 1 ? objects[1] : null;
            if (!(value is LispValue))
                throw new LispValueExpectedException(value);

            scope[identifier] = (LispValue)value;
            return new LispVoid();
        }
    }
}