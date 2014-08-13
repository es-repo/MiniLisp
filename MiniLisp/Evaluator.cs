using System;
using System.Linq;
using MiniLisp.Exceptions;
using MiniLisp.LispExpressionElements;
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

        public LispExpressionElement Eval(LispExpression expression)
        {
            LispExpression foldedLambdas = Tree<LispExpressionElement>.Fold<LispExpression>(expression,
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

        private LispExpressionElement Eval(LispExpression expression, Scope scope)
        {
            //TODO: duplicate definition for identifier
            // (define e 5) (define e 5)

            //TODO: (define fn (lambda (d) (define d 3) d)) - that's OK!

            //TODO: duplicate argument name
            // (define fn (lambda (d d) d))

            return Tree<LispExpressionElement>.Fold<LispExpressionElement>(expression,
                (ni, objects) =>
                {
                    LispExpressionElement lispElement = ni.Node.Value;
                    if (lispElement is LispEval)
                    {
                        if (objects.Length == 0)
                            throw new LispProcedureExpectedException();

                        LispExpressionElement firstObj = objects[0];

                        if (!(firstObj is LispProcedureBase))
                            throw new LispProcedureExpectedException(firstObj);

                        return firstObj is LispBuiltInProcedure 
                            ? EvalBuiltInProcedure(objects) 
                            : EvalProcedure(objects);
                    }
                    
                    if (lispElement is LispDefine)
                    {
                        return EvalDefine(objects, scope);
                    }

                    if (objects != null && objects.Length > 0)
                        throw new InvalidOperationException("Expected no arguments.");

                    if (lispElement is LispIdentifier)
                    {
                        bool passIdentifer = ni.ParentNode != null && ni.ParentNode.Value is LispDefine && ni.IndexAmongSiblings == 0;
                        if (!passIdentifer)
                        {
                            LispIdentifier identifier = (LispIdentifier) lispElement;
                            if (!scope.Contains(identifier))
                                throw new LispUnboundIdentifierException(identifier);
                            lispElement = scope[identifier];
                        }
                    }

                    if (lispElement is LispProcedure)
                    {
                        lispElement = ((LispProcedure) lispElement).Copy(new Scope(scope));
                    }

                    return lispElement;
                });
        }

        private LispValue EvalBuiltInProcedure(LispExpressionElement[] elements)
        {
            LispBuiltInProcedure procedure = (LispBuiltInProcedure)elements[0];
            //TODO: (+ define 4)
            //TODO: define inside expression is not allowed 
            LispValue[] args = elements.Skip(1).Where(o => !(o is LispVoid)).Cast<LispValue>().ToArray();
            LispProcedureContractVerification.Assert(procedure.Signature, args);
            return procedure.Value(args);
        }

        private LispExpressionElement EvalProcedure(LispExpressionElement[] elements)
        {
            LispProcedure procedure = (LispProcedure)elements[0];
            //TODO: check args
            //TODO: assert contract
            return procedure.Body.Aggregate((LispExpressionElement)null, (a, e) => Eval(e, procedure.Scope));
        }

        private LispVoid EvalDefine(LispExpressionElement[] elements, Scope scope)
        {
            LispExpressionElement firstElement = elements.Length > 0 ? elements[0] : null;
            if (!(firstElement is LispIdentifier))
                throw new LispIdentifierExpectedException(firstElement);

            LispIdentifier identifier = (LispIdentifier)firstElement;

            if (elements.Length > 2)
                throw new LispMultipleExpressionsException(identifier);

            LispExpressionElement value = elements.Length > 1 ? elements[1] : null;
            if (!(value is LispValue))
                throw new LispValueExpectedException(value);

            scope[identifier] = (LispValue)value;
            return new LispVoid();
        }
    }
}