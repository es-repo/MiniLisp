using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using MiniLisp.LispObjects;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class LispExpressionTests
    {
        [Test]
        public void TestEval()
        {
            LispExpression expression = new LispExpression(
                new LispEval())
                {
                    new LispExpression(new LispIdentifier("+")), new LispExpression(new LispNumber(2)), new LispExpression(new LispNumber(3))
                };
        }
    }
}
