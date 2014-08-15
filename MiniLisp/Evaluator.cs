using System;
using System.Collections.Generic;
using System.Linq;
using MiniLisp.Exceptions;
using MiniLisp.Expressions;
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

        public LispValueElement Eval(LispExpression expression)
        {
            LispExpression foldedLambdas = Tree<LispExpressionElement>.Fold<LispExpression>(expression,
                (ni, children) =>
                {
                    bool procedureInLambda = ni.Node.Value is LispLambda;
                    if (procedureInLambda)
                    {
                        if (children.Length < 1 || !(children[0].Value is LispProcedureSignatureElement))
                            throw new LispProcedureSignatureExpressionExpectedException(children.Length > 0
                                ? children[0].Value
                                : null);
                    }

                    LispExpression procedureExpression = null;
                    bool procedureInLambdaOrDefine = procedureInLambda || 
                        ni.Node.Value is LispDefine && children.Length > 0 && children[0].Value is LispProcedureSignatureElement;
                    if (procedureInLambdaOrDefine)
                    {
                        LispProcedureSignature signature = EvalProcedureSignature(children[0], !procedureInLambda);

                        if (children.Length < 2)
                            throw new LispProcedureBodyExpressionExpectedException();

                        LispExpression[] bodyExpressions = children.Skip(1).ToArray();
                        procedureExpression = new LispExpression(new LispProcedure(signature, bodyExpressions));
                    }

                    if (procedureExpression != null)
                    {
                        if (procedureInLambda)
                            return procedureExpression;

                        LispExpression procedureIdentificatorExpression = (LispExpression)children[0].Children[0];
                        return new LispExpression(ni.Node.Value)
                        {
                            procedureIdentificatorExpression,
                            procedureExpression
                        };
                    }

                    if (ni.Node.Value is LispIf)
                    {
                        IEnumerable<LispExpression> ifProcedures = children.Select(c => new LispExpression(new LispProcedure(new LispProcedureSignature(), new[] { c })));
                        LispExpression ifExpression = new LispExpression(ni.Node.Value);
                        ifExpression.AddRange(ifProcedures);
                        return ifExpression;
                    }

                    LispExpression e = new LispExpression(ni.Node.Value);
                    e.AddRange(children);
                    return e;
                });

            return Eval(foldedLambdas, _mainScope);
        }

        private LispValueElement Eval(LispExpression expression, Scope scope)
        {
            return Tree<LispExpressionElement>.Fold<LispValueElement>(expression,
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
                            : EvalProcedure(objects, scope);
                    }
                    
                    if (lispElement is LispDefine)
                        return EvalDefine(objects, scope);

                    if (lispElement is LispSet)
                        return EvalSet(objects, scope);

                    if (lispElement is LispIf)
                        return EvalIf(objects, scope);

                    if (objects != null && objects.Length > 0)
                        throw new InvalidOperationException("Expected no arguments.");

                    if (lispElement is LispIdentifier)
                    {
                        bool passIdentifer = ni.ParentNode != null && ni.IndexAmongSiblings == 0
                            && (ni.ParentNode.Value is LispDefine || ni.ParentNode.Value is LispSet);
                        if (!passIdentifer)
                        {
                            LispIdentifier identifier = (LispIdentifier) lispElement;
                            return scope[identifier];
                        }
                    }

                    return lispElement is LispProcedure
                        ? ((LispProcedure) lispElement).Copy(scope)
                        : (LispValueElement)lispElement;
                });
        }

        private LispProcedureSignature EvalProcedureSignature(LispExpression signatureExpression, bool procedureIdentifierFirst)
        {
            IEnumerable<LispExpressionElement> elements = signatureExpression.Children.Select(n => n.Value).ToArray();
            LispExpressionElement notIdentifier = elements.FirstOrDefault(e => !(e is LispIdentifier));
            if (notIdentifier != null)
                throw new LispIdentifierExpectedException(notIdentifier);

            if (procedureIdentifierFirst && signatureExpression.Children.Count == 0)
                throw new LispIdentifierExpectedException();

            LispIdentifier[] identifiers = elements.Skip(procedureIdentifierFirst ? 1 : 0).Cast<LispIdentifier>().ToArray();

            var duplicate = identifiers.GroupBy(e => e.Value).FirstOrDefault(g => g.Count() > 1);
            if (duplicate != null)
                throw new LispProcedureDuplicateParameterException(duplicate.Key);

            LispProcedureParameter[] parameters = identifiers.Select(id => new LispProcedureParameter(id.Value, typeof (LispValueElement))).ToArray();
            return new LispProcedureSignature(parameters);
        }

        private LispValueElement EvalProcedure(LispExpressionElement[] elements, Scope scope)
        {
            LispProcedure procedure = ((LispProcedure)elements[0]);
            Scope argumentsScope = new Scope(procedure.Scope ?? scope);
            procedure = procedure.Copy(new Scope(argumentsScope));
            LispExpressionElement[] arguments = elements.Skip(1).ToArray();
            LispProcedureContractVerification.Assert(procedure.Signature, arguments);
            for (int i = 0; i < arguments.Length; i++)
            {
                argumentsScope.Add(procedure.Signature.NamedParameters[i].Identifier, (LispValueElement)arguments[i]);
            }
            return procedure.Body.Aggregate((LispValueElement)null, (a, e) => Eval(e, procedure.Scope));
        }

        private LispValueElement EvalBuiltInProcedure(LispExpressionElement[] elements)
        {
            LispBuiltInProcedure procedure = (LispBuiltInProcedure)elements[0];
            LispValueElement[] args = elements.Skip(1).Cast<LispValueElement>().ToArray();
            LispProcedureContractVerification.Assert(procedure.Signature, args);
            return procedure.Value(args);
        }

        private LispVoid EvalDefineOrSet(LispExpressionElement[] elements, Scope scope, bool define)
        {
            LispExpressionElement firstElement = elements.Length > 0 ? elements[0] : null;
            if (!(firstElement is LispIdentifier))
                throw new LispIdentifierExpectedException(firstElement);

            LispIdentifier identifier = (LispIdentifier)firstElement;

            if (elements.Length > 2)
                throw new LispMultipleExpressionsException(identifier);

            LispExpressionElement value = elements.Length > 1 ? elements[1] : null;
            if (!(value is LispValueElement))
                throw new LispValueExpectedException(value);

            if (define)
                scope.Add(identifier, (LispValueElement) value);
            else
                scope[identifier] = (LispValueElement) value;
            return new LispVoid();
        }

        private LispVoid EvalDefine(LispExpressionElement[] elements, Scope scope)
        {
            return EvalDefineOrSet(elements, scope, true);
        }

        private LispVoid EvalSet(LispExpressionElement[] elements, Scope scope)
        {
            return EvalDefineOrSet(elements, scope, false);
        }

        private LispValueElement EvalIf(LispExpressionElement[] elements, Scope scope)
        {
            if (elements.Any(e => !(e is LispProcedure)))
                throw new InvalidOperationException("Procedures are expected in \"if\" context");

            if (elements.Length < 1)
                throw new LispIfPartExpectedException("test");

            if (elements.Length < 2)
                throw new LispIfPartExpectedException("then");

            if (elements.Length < 3)
                throw new LispIfPartExpectedException("else");

            if (elements.Length > 3)
                throw new LispIfTooManyPartsException(elements.Length);

            LispValueElement testResult = EvalProcedure(new[] {elements[0]}, scope);

            return (testResult is LispBoolean) && ((LispBoolean) testResult).Value == false
                ? EvalProcedure(new[] {elements[2]}, scope)
                : EvalProcedure(new[] {elements[1]}, scope);
        }
    }
}