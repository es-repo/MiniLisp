using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using MiniLisp.Exceptions;
using MiniLisp.Expressions;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class EvaluatorTests
    {
        [Test]
        public void TestEval()
        {
            Evaluator evaluator = new Evaluator();
            LispValueElement evalResult = evaluator.Eval(new LispExpression(new LispNumber(5)));
            Assert.AreEqual(5.0, evalResult.Value);

            evalResult = evaluator.Eval(new LispExpression(
                new LispEval())
                {
                    new LispExpression(new LispIdentifier("+")), new LispExpression(new LispNumber(4)), new LispExpression(new LispNumber(8))
                });
            Assert.AreEqual(12.0, evalResult.Value);

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
            Assert.AreEqual(20.0, evalResult.Value);
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
            evaluator.Eval(new LispExpression(new LispEval()) { new LispExpression(new LispNumber(5)) });
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

        [Test, ExpectedException(typeof(LispDuplicateIdentifierDefinitionException))]
        public void TestDuplicateIdentifierInEval()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(
                new LispDefine())
                {
                    new LispExpression(new LispIdentifier("a")), 
                    new LispExpression(new LispNumber(4))
                });

            evaluator.Eval(new LispExpression(
                new LispDefine())
                {
                    new LispExpression(new LispIdentifier("a")), 
                    new LispExpression(new LispNumber(4))
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
            LispValueElement evalResult = evaluator.Eval(new LispExpression(new LispDefine())
            {
                new LispExpression(new LispIdentifier("d")),
                new LispExpression(new LispNumber(5)),
            });
            Assert.IsTrue(evalResult is LispVoid);

            evalResult = evaluator.Eval(new LispExpression(new LispIdentifier("d")));
            Assert.AreEqual(5.0, evalResult.Value);

            evaluator.Eval(new LispExpression(new LispDefine())
            {                
                new LispExpression(new LispIdentifier("e")),
                new LispExpression(new LispIdentifier("d")),
            });

            evalResult = evaluator.Eval(new LispExpression(new LispIdentifier("e")));
            Assert.AreEqual(5.0, evalResult.Value);

            evaluator.Eval(new LispExpression(new LispDefine())
            {
                new LispExpression(new LispIdentifier("sqrt")),
                new LispExpression(new LispBuiltInProcedure(
                    new LispProcedureSignature(null, typeof(LispNumber), 1), 
                    args => new LispNumber(Math.Sqrt((((LispNumber)args[0]).Value)))))
            });

            evalResult = evaluator.Eval(new LispExpression(new LispEval())
            {
                new LispExpression(new LispIdentifier("sqrt")),
                new LispExpression(new LispNumber(9))
            });
            Assert.AreEqual(3.0, evalResult.Value);
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
        public void TestEvalDefineWithProcSignature()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispDefine())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispIdentifier("fn")),
                    new LispExpression(new LispIdentifier("a")),
                    new LispExpression(new LispIdentifier("b"))
                },
                new LispExpression(new LispEval())
                {
                    new LispExpression(new LispIdentifier("*")),
                    new LispExpression(new LispIdentifier("a")),
                    new LispExpression(new LispIdentifier("b"))
                }
            });

            LispValueElement evalResult = evaluator.Eval(new LispExpression(new LispIdentifier("fn")));
            Assert.IsInstanceOfType(typeof(LispProcedure), evalResult);

            evalResult = evaluator.Eval(new LispExpression(new LispEval())
            {
                new LispExpression(new LispIdentifier("fn")),
                new LispExpression(new LispNumber(2)),
                new LispExpression(new LispNumber(3))
            });
            Assert.AreEqual(6.0, evalResult.Value);
        }

        [Test, ExpectedException(typeof(LispIdentifierExpectedException))]
        public void TestEvalDefineWithProcSignatureAndNoProcId()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispDefine())
            {
                new LispExpression(new LispGroupElement()),
                new LispExpression(new LispEval())
                {
                    new LispExpression(new LispIdentifier("*")),
                    new LispExpression(new LispNumber(2)),
                    new LispExpression(new LispNumber(3))
                }
            });
        }

        [Test]
        public void TestEvalSet()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispDefine())
            {
                new LispExpression(new LispIdentifier("d")),
                new LispExpression(new LispNumber(5)),
            });
            LispValueElement evalResult = evaluator.Eval(new LispExpression(new LispSet())
            {
                new LispExpression(new LispIdentifier("d")),
                new LispExpression(new LispNumber(3)),
            });
            Assert.IsTrue(evalResult is LispVoid);

            evalResult = evaluator.Eval(new LispExpression(new LispIdentifier("d")));
            Assert.AreEqual(3.0, evalResult.Value);
        }

        [Test]
        public void TestEvalLambda()
        {
            Evaluator evaluator = new Evaluator();

            LispExpression lambdaExpression = new LispExpression(new LispLambda())
            {
                new LispExpression(new LispGroupElement()),

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

            LispValueElement evalResult = evaluator.Eval(lambdaExpression);
            Assert.IsTrue(evalResult is LispProcedure);

            evalResult = evaluator.Eval(new LispExpression(new LispEval()) { lambdaExpression });
            Assert.AreEqual(15.0, evalResult.Value);

            evaluator = new Evaluator();
            evalResult = evaluator.Eval(new LispExpression(new LispEval())
            {
                new LispExpression(new LispEval())
                {
                    new LispExpression(new LispLambda())
                    {
                        new LispExpression(new LispGroupElement()),
                        lambdaExpression
                    }    
                }
            });
            Assert.AreEqual(15.0, evalResult.Value);
        }

        [Test]
        public void TestEvalLambdaWithArguments()
        {
            Evaluator evaluator = new Evaluator();
            LispExpression procedureExpression = new LispExpression(
                new LispLambda())
                {
                    new LispExpression(new LispGroupElement())
                    {
                        new LispExpression(new LispIdentifier("a")),
                        new LispExpression(new LispIdentifier("b"))
                    },
                    new LispExpression(new LispDefine())
                    {
                        new LispExpression(new LispIdentifier("c")),
                        new LispExpression(new LispNumber(5))
                    },
                    new LispExpression(new LispEval())
                    {
                        new LispExpression(new LispIdentifier("+")),
                        new LispExpression(new LispIdentifier("a")),
                        new LispExpression(new LispIdentifier("b")),
                        new LispExpression(new LispIdentifier("c")),
                    } 
                };

            LispExpressionElement result = evaluator.Eval(new LispExpression(new LispEval())
            {
                procedureExpression,
                new LispExpression(new LispNumber(2)),
                new LispExpression(new LispNumber(3))
            });
            Assert.AreEqual(10, ((LispNumber)result).Value);
        }

        [Test, ExpectedException(typeof(LispProcedureBodyExpressionExpectedException))]
        public void TestEvalLambdaWithoutBody()
        {
            Evaluator evaluator = new Evaluator();
            LispExpression procedureExpression = new LispExpression(
                new LispLambda())
                {
                    new LispExpression(new LispGroupElement())
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

        [Test, ExpectedException(typeof(LispIdentifierExpectedException))]
        public void TestEvalLambdaWithNotIdentifierParams()
        {
            Evaluator evaluator = new Evaluator();
            LispExpression procedureExpression = new LispExpression(
                new LispLambda())
                {
                    new LispExpression(new LispGroupElement())
                    {
                        new LispExpression(new LispIdentifier("a")),
                        new LispExpression(new LispString("b"))
                    }
                };

            evaluator.Eval(new LispExpression(new LispEval()) { procedureExpression });
        }

        [Test, ExpectedException(typeof(LispProcedureDuplicateParameterException))]
        public void TestEvalLambdaWithDuplicateParams()
        {
            Evaluator evaluator = new Evaluator();
            LispExpression procedureExpression = new LispExpression(
                new LispLambda())
                {
                    new LispExpression(new LispGroupElement())
                    {
                        new LispExpression(new LispIdentifier("a")),
                        new LispExpression(new LispIdentifier("b")),
                        new LispExpression(new LispIdentifier("c")),
                        new LispExpression(new LispIdentifier("b")),
                    }
                };

            evaluator.Eval(new LispExpression(new LispEval()) { procedureExpression });
        }

        [Test]
        [Row(1, ExpectedException = typeof(LispProcedureArityMismatchException))]
        [Row(2)]
        [Row(3, ExpectedException = typeof(LispProcedureArityMismatchException))]
        public void TestEvalLambdaWithDiffentNumnerOfArgument(int argsCount)
        {
            Evaluator evaluator = new Evaluator();
            LispExpression procedureExpression = new LispExpression(
                new LispLambda())
                {
                    new LispExpression(new LispGroupElement())
                    {
                        new LispExpression(new LispIdentifier("a")),
                        new LispExpression(new LispIdentifier("b"))
                    },
                    new LispExpression( new LispIdentifier("a"))
                };

            IEnumerable<LispExpression> args = Enumerable.Repeat(0, argsCount).Select(i => new LispExpression(new LispNumber(3)));
            LispExpression expr = new LispExpression(new LispEval())
            {
                procedureExpression
            };
            expr.AddRange(args);

            evaluator.Eval(expr);
        }

        [Test]
        public void TestEvalIf()
        {
            Evaluator evaluator = new Evaluator();
            LispValueElement evalResult = evaluator.Eval(new LispExpression(new LispIf())
            {
                new LispExpression((new LispBoolean(true))),
                new LispExpression((new LispNumber(5))),
                new LispExpression((new LispNumber(3)))                
            });

            Assert.AreEqual(5.0, evalResult.Value);

            evalResult = evaluator.Eval(new LispExpression(new LispIf())
            {
                new LispExpression((new LispBoolean(false))),
                new LispExpression((new LispNumber(5))),
                new LispExpression((new LispNumber(3)))                
            });

            Assert.AreEqual(3.0, evalResult.Value);
        }

        [Test]
        [Row(0, ExpectedException = typeof(LispIfPartExpectedException))]
        [Row(1, ExpectedException = typeof(LispIfPartExpectedException))]
        [Row(2, ExpectedException = typeof(LispIfPartExpectedException))]
        [Row(4, ExpectedException = typeof(LispIfTooManyPartsException))]
        public void TestIfWithDifferentPartsCount(int partsCount)
        {
            Evaluator evaluator = new Evaluator();
            LispExpression ifExpression = new LispExpression(new LispIf());
            IEnumerable<LispExpression> parts = Enumerable.Repeat(0, partsCount).Select(i => new LispExpression(new LispNumber(5)));
            ifExpression.AddRange(parts);
            evaluator.Eval(ifExpression);
        }

        [Test, ExpectedException(typeof(LispCondTestValueExressionExpectedException))]
        public void TestCondWithNonGroupChildren()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispCond())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispNumber(2))
                },  
                new LispExpression(new LispNumber(5))
            });
        }

        [Test, ExpectedException(typeof(LispCondTestValueExressionExpectedException))]
        public void TestCondWithEmptyGroupChildren()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispCond())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispNumber(2))
                },  
                new LispExpression(new LispGroupElement())
            });
        }

        [Test]
        public void TestEvalCond()
        {
            Evaluator evaluator = new Evaluator();
            LispValueElement evalResult = evaluator.Eval(new LispExpression(new LispCond()));
            Assert.IsTrue(evalResult is LispVoid);

            evalResult = evaluator.Eval(new LispExpression(new LispCond())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispBoolean(false))
                }
            });
            Assert.IsTrue(evalResult is LispVoid);

            evalResult = evaluator.Eval(new LispExpression(new LispCond())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispBoolean(false)),
                    new LispExpression(new LispNumber(5))
                }
            });
            Assert.IsTrue(evalResult is LispVoid);

            evalResult = evaluator.Eval(new LispExpression(new LispCond())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispBoolean(true))
                }
            });
            Assert.AreEqual(true, evalResult.Value);

            evalResult = evaluator.Eval(new LispExpression(new LispCond())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispBoolean(true)),
                    new LispExpression(new LispNumber(5))
                }
            });
            Assert.AreEqual(5.0, evalResult.Value);

            evalResult = evaluator.Eval(new LispExpression(new LispCond())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispBoolean(false)),
                    new LispExpression(new LispNumber(5))
                },
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispBoolean(true)),
                    new LispExpression(new LispNumber(3))
                }
            });
            Assert.AreEqual(3.0, evalResult.Value);

            evalResult = evaluator.Eval(new LispExpression(new LispCond())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispBoolean(false))                    
                }
            });

            Assert.IsTrue(evalResult is LispVoid);

            evalResult = evaluator.Eval(new LispExpression(new LispCond())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispElse()),   
                    new LispExpression(new LispBoolean(false))                    
                }
            });

            Assert.AreEqual(false, evalResult.Value);

            evalResult = evaluator.Eval(new LispExpression(new LispCond())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispBoolean(false)),   
                    new LispExpression(new LispNumber(5))                    
                },
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispElse()),   
                    new LispExpression(new LispNumber(3))                     
                }
            });

            Assert.AreEqual(3.0, evalResult.Value);
        }

        [Test, ExpectedException(typeof(LispNotAllowedAsExpressionException))]
        public void TestElseAsSingleExpression()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispElse()));
        }

        [Test, ExpectedException(typeof(LispNotAllowedAsExpressionException))]
        public void TestElseNonInCond()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispGroupElement())
            {
                new LispExpression(new LispElse()),
                new LispExpression(new LispNumber(5))
            });
        }

        [Test, ExpectedException(typeof(LispExpressionsInElseExpectedException))]
        public void TestElseWihtouExpressions()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispCond())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispElse())
                }
            });
        }

        [Test, ExpectedException(typeof(LispElseMustBeLastException))]
        public void TestElseNotLast()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispCond())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispElse()),
                    new LispExpression(new LispNil()),
                },
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispNil()),
                }
            });
        }

        [Test]
        public void TestEvalLet()
        {
            Evaluator evaluator = new Evaluator();
            LispValueElement evalResult = evaluator.Eval(new LispExpression(new LispCond()));
            Assert.IsTrue(evalResult is LispVoid);

            evalResult = evaluator.Eval(new LispExpression(new LispLet())
            {
                new LispExpression(new LispGroupElement()),
                new LispExpression(new LispNumber(3))
            });
            Assert.AreEqual(3.0, evalResult.Value);

            evalResult = evaluator.Eval(new LispExpression(new LispLet())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispGroupElement())
                    {
                        new LispExpression(new LispIdentifier("a")),
                        new LispExpression(new LispNumber(5))
                    }
                },
                new LispExpression(new LispIdentifier("a"))
            });
            Assert.AreEqual(5.0, evalResult.Value);

            evalResult = evaluator.Eval(new LispExpression(new LispLet())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispGroupElement())
                    {
                        new LispExpression(new LispIdentifier("a")),
                        new LispExpression(new LispNumber(5))
                    },
                    new LispExpression(new LispGroupElement())
                    {
                        new LispExpression(new LispIdentifier("b")),
                        new LispExpression(new LispNumber(3))
                    }
                },
                new LispExpression(new LispEval())
                {
                    new LispExpression(new LispIdentifier("+")),    
                    new LispExpression(new LispIdentifier("a")),    
                    new LispExpression(new LispIdentifier("b"))
                }
            });
            Assert.AreEqual(8.0, evalResult.Value);
        }

        [Test, ExpectedException(typeof(LispLetPartExpectedException))]
        public void TestEmptyLet()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispLet()));
        }

        [Test, ExpectedException(typeof(LispLetPartExpectedException))]
        public void TestLetWithoutBindigsPairs()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispLet())
            {
                new LispExpression(new LispBoolean(false))
            });
        }

        [Test, ExpectedException(typeof(LispLetPartExpectedException))]
        public void TestLetWithWithoutBody()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispLet())
            {
                new LispExpression(new LispGroupElement())
            });
        }

        [Test, ExpectedException(typeof(LispLetPartExpectedException))]
        public void TestLetWithBindingPairWithoutIdentifier()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispLet())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispNumber(5))
                }
            });
        }

        [Test, ExpectedException(typeof(LispLetPartExpectedException))]
        public void TestLetWithBindingPairWithTooManyBindingExpressions()
        {
            Evaluator evaluator = new Evaluator();
            evaluator.Eval(new LispExpression(new LispLet())
            {
                new LispExpression(new LispGroupElement())
                {
                    new LispExpression(new LispIdentifier("d")),
                    new LispExpression(new LispNumber(5)),
                    new LispExpression(new LispNumber(5))
                }
            });
        }
    }
}