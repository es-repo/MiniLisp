using System;
using MbUnit.Framework;
using MiniLisp.Exceptions;
using MiniLisp.LispObjects;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class EvaluatorTests
    {
        [Test]
        public void TestEval()
        {
            Evaluator evaluator = new Evaluator();
            LispObject evalResult = evaluator.Eval(new LispExpression(new LispNumber(5)));
            Assert.AreEqual(new LispNumber(5), evalResult);

            evalResult = evaluator.Eval(new LispExpression(
                new LispEval())
                {
                    new LispExpression(new LispIdentifier("+")), new LispExpression(new LispNumber(4)), new LispExpression(new LispNumber(8))
                });
            Assert.AreEqual(new LispNumber(12), evalResult);

            evalResult = evaluator.Eval(new LispExpression(new LispEval())
                {
                    new LispExpression(new LispIdentifier("+")), 
                        new LispExpression(new LispEval()) 
                        { 
                            new LispExpression(new LispIdentifier("*")), new LispExpression(new LispNumber(4)), new LispExpression(new LispNumber(3)) 
                        }, 
                        new LispExpression(new LispEval()) 
                        { 
                            new LispExpression(new LispIdentifier("/")), new LispExpression(new LispNumber(32)), new LispExpression(new LispNumber(4)) 
                        }
                });
            Assert.AreEqual(new LispNumber(20), evalResult);
        }

        [Test, ExpectedException(typeof(LispProcedureExpectedException))]
        public void TestEmptyEval()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispEval()));
        }

        [Test, ExpectedException(typeof(LispProcedureExpectedException))]
        public void TestNotIdenifier()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression( new LispEval()) { new LispExpression(new LispNumber(5)) });
        }

        [Test, ExpectedException(typeof(LispUnboundIdentifierException))]
        public void TestUnboundIdentifierInEval()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(
                new LispEval())
                {
                    new LispExpression(new LispIdentifier("$#F#@F")), new LispExpression(new LispNumber(4)), new LispExpression(new LispNumber(8))
                });
        }

        [Test, ExpectedException(typeof(LispUnboundIdentifierException))]
        public void TestUnboundIdentifier()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispIdentifier("$#F#@F")));
        }

        [Test, ExpectedException(typeof(LispProcedureExpectedException))]
        public void TestIdentifierIsNotProcedure()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(
                new LispEval())
                {
                    new LispExpression(new LispNumber(4)), new LispExpression(new LispNumber(8))
                });
        }

        [Test]
        public void TestEvalDefine()
        {
            Evaluator evaluator = new Evaluator();
            LispObject evalResult = evaluator.Eval(new LispExpression(new LispDefine())
            {
                new LispExpression(new LispIdentifier("d")),
                new LispExpression(new LispNumber(5)),
            });
            Assert.IsTrue(evalResult is LispVoid);

            evalResult = evaluator.Eval(new LispExpression(new LispIdentifier("d")));
            Assert.AreEqual(new LispNumber(5), evalResult);

            evaluator.Eval(new LispExpression(new LispDefine())
            {                
                new LispExpression(new LispIdentifier("e")),
                new LispExpression(new LispIdentifier("d")),
            });

            evalResult = evaluator.Eval(new LispExpression(new LispIdentifier("e")));
            Assert.AreEqual(new LispNumber(5), evalResult);

            evaluator.Eval(new LispExpression(new LispDefine())
            {
                new LispExpression(new LispIdentifier("sqrt")),
                new LispExpression(new LispBuiltInProcedure(
                    new ProcedureSignature(new ProcedureParameterTypes(typeof(LispNumber)), 1), 
                    args => new LispNumber(Math.Sqrt((((LispNumber)args[0]).Value)))))
            });

            evalResult = evaluator.Eval(new LispExpression(new LispEval())
            {
                new LispExpression(new LispIdentifier("sqrt")),
                new LispExpression(new LispNumber(9))
            });
            Assert.AreEqual(new LispNumber(3), evalResult);
        }

        [Test, ExpectedException(typeof(LispIdentifierExpectedException))]
        public void TestEvalDefineWithoutArgs()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispDefine()));
        }

        [Test, ExpectedException(typeof(LispIdentifierExpectedException))]
        public void TestEvalDefineWithoutIdentidier()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispDefine())
            {
                new LispExpression(new LispNumber(5))
            });
        }

        [Test, ExpectedException(typeof(LispMultipleExpressionsException))]
        public void TestEvalDefineWithMultipleExpressions()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispDefine())
            {
                new LispExpression(new LispIdentifier("d")),
                new LispExpression(new LispNumber(5)),
                new LispExpression(new LispNumber(5))
            });
        }

        [Test, ExpectedException(typeof(LispValueExpectedException))]
        public void TestEvalDefineWithoutValue()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispDefine())
            {
                new LispExpression(new LispIdentifier("d"))
            });
        }

        [Test]
        public void TestEvalLambda()
        {            
            Evaluator evaluator = new Evaluator();

            LispExpression lambdaExpression = new LispExpression(new LispLambda())
            {
                new LispExpression(new LispProcedureSignature()),

                new LispExpression(new LispDefine())
                {
                    new LispExpression(new LispIdentifier("a")),
                    new LispExpression(new LispNumber(3))
                },

                new LispExpression(new LispEval())
                {
                    new LispExpression(new LispIdentifier("+")),
                    new LispExpression(new LispNumber(2)),
                    new LispExpression(new LispNumber(3))
                },

                new LispExpression(new LispEval())
                {
                    new LispExpression(new LispIdentifier("*")),
                    new LispExpression(new LispNumber(5)),
                    new LispExpression(new LispIdentifier("a"))
                }
            };

            LispObject evalResult = evaluator.Eval(lambdaExpression);
            Assert.IsTrue(evalResult is LispProcedure);

            evalResult = evaluator.Eval(new LispExpression(new LispEval()) { lambdaExpression });
            Assert.AreEqual(new LispNumber(15), evalResult);

            evaluator = new Evaluator();
            evalResult = evaluator.Eval(new LispExpression(new LispEval())
            {
                new LispExpression(new LispEval())
                {
                    new LispExpression(new LispLambda())
                    {
                        new LispExpression(new LispProcedureSignature()),
                        lambdaExpression
                    }    
                }
            });
            Assert.AreEqual(new LispNumber(15), evalResult);
        }

        [Test, ExpectedException(typeof(LispProcedureBodyExpressionExpectedException))]
        public void TestEvalLambdaWithoutBody()
        {
            Evaluator evaluator = new Evaluator();
            LispExpression procedureExpression = new LispExpression(
                new LispLambda())
                {
                    new LispExpression(new LispProcedureSignature())
                };

            evaluator.Eval(new LispExpression(new LispEval()) { procedureExpression });
        }

        [Test, ExpectedException(typeof(LispProcedureSignatureExpressionExpectedException))]
        public void TestEvalLambdaWithoutSignature()
        {
            Evaluator evaluator = new Evaluator();
            LispExpression procedureExpression = new LispExpression(
                new LispLambda())
                {
                    new LispExpression(new LispNumber(3))
                };

            evaluator.Eval(new LispExpression(new LispEval()) { procedureExpression });
        }
    }
}