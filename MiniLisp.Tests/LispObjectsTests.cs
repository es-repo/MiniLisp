using System.Collections.Generic;
using MbUnit.Framework;
using MiniLisp.LispObjects;

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
            Assert.AreEqual("#<procedure:+>", new LispProcedure(new LispProcedureSignature { Identifier = "+"},  o => o[0]).ToString());
            Assert.AreEqual("'(1 . #f)", new LispPair(new KeyValuePair<LispObject, LispObject>(new LispNumber(1), new LispBoolean(false))).ToString());
            Assert.AreEqual("nil", new LispNil().ToString());
            Assert.AreEqual("", new LispVoid().ToString());
            Assert.AreEqual("id", new LispIdentifier("id").ToString());

            Assert.AreEqual("'()", new LispExpressionObject(new LispExpression(new LispEval())).ToString());
            Assert.AreEqual("'5", new LispExpressionObject(new LispExpression(new LispNumber(5))).ToString());

            Assert.AreEqual("'(+ 2 (* 3 4)", new LispExpressionObject(new LispExpression(new LispEval())
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
        }

        [Test]
        public void TestEquals()
        {
            Assert.AreEqual(new LispNumber(5), new LispNumber(5));
            Assert.AreEqual(new LispString("ab"), new LispString("ab"));
            Assert.AreEqual(new LispBoolean(true), new LispBoolean(true));
        }
    }
}
