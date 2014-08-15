using System.Collections.Generic;
using MbUnit.Framework;
using MiniLisp.Expressions;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class LispObjectsTests
    {
        [Test]
        public void TestToString()
        {
            Assert.AreEqual("5", new LispNumber(5).ToString());
            Assert.AreEqual("\"ab\"", new LispString("ab").ToString());
            Assert.AreEqual("#t", new LispBoolean(true).ToString());
            Assert.AreEqual("#<procedure:+>", new LispBuiltInProcedure(new LispProcedureSignature { Identifier = "+" }, o => o[0]).ToString());
            Assert.AreEqual("'(1 . #f)", new LispPair(new KeyValuePair<LispExpressionElement, LispExpressionElement>(new LispNumber(1), new LispBoolean(false))).ToString());
            Assert.AreEqual("nil", new LispNil().ToString());
            Assert.AreEqual("#<void>", new LispVoid().ToString());
            Assert.AreEqual("id", new LispIdentifier("id").ToString());

            Assert.AreEqual("'()", new LispExpressionValue(new LispExpression(new LispEval())).ToString());
            Assert.AreEqual("'5", new LispExpressionValue(new LispExpression(new LispNumber(5))).ToString());

            Assert.AreEqual("'(1)", new LispExpressionValue(new LispExpression(new LispEval())
            {
                new LispExpression(new LispNumber(1))
            }).ToString());

            Assert.AreEqual("'(- 1)", new LispExpressionValue(new LispExpression(new LispEval())
            {
                new LispExpression(new LispIdentifier("-")), 
                new LispExpression(new LispNumber(1))
            }).ToString());

            Assert.AreEqual("'(+ 2 (* 3 4))", new LispExpressionValue(new LispExpression(new LispEval())
            {
                new LispExpression(new LispIdentifier("+")), 
                new LispExpression(new LispNumber(2)),
                new LispExpression(new LispEval())
                {
                    new LispExpression(new LispIdentifier("*")), 
                    new LispExpression(new LispNumber(3)),
                    new LispExpression(new LispNumber(4))
                }
            }).ToString());

            Assert.AreEqual("'(+ 2 (* (+ 3) (- 4)))", new LispExpressionValue(new LispExpression(new LispEval())
            {
                new LispExpression(new LispIdentifier("+")), 
                new LispExpression(new LispNumber(2)),
                new LispExpression(new LispEval())
                {
                    new LispExpression(new LispIdentifier("*")), 
                    new LispExpression(new LispEval())
                    {
                        new LispExpression(new LispIdentifier("+")), 
                        new LispExpression(new LispNumber(3))    
                    },
                    new LispExpression(new LispEval())
                    {
                        new LispExpression(new LispIdentifier("-")), 
                        new LispExpression(new LispNumber(4))    
                    }
                }
            }).ToString());

            Assert.AreEqual("''(+ 2 3)", new LispExpressionValue(new LispExpression(new LispExpressionValue(new LispExpression(new LispEval())
            {
                new LispExpression(new LispIdentifier("+")), 
                new LispExpression(new LispNumber(2)),
                new LispExpression(new LispNumber(3))
            }))).ToString());
        }
    }
}
