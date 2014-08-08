using MbUnit.Framework;
using MiniLisp.LispObjects;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class EvaluatorTests
    {
        private readonly Evaluator _evaluator = new Evaluator();

        [Test]
        public void TestEval()
        {
            LispObject evalResult = _evaluator.Eval(new LispExpression(new LispNumber(5)));
            Assert.AreEqual(new LispNumber(5), evalResult);

            evalResult = _evaluator.Eval(new LispExpression(
                new LispEval())
                {
                    new LispExpression(new LispIdentifier("+")), new LispExpression(new LispNumber(4)), new LispExpression(new LispNumber(8))
                });
            Assert.AreEqual(new LispNumber(12), evalResult);

            evalResult = _evaluator.Eval(new LispExpression(new LispEval())
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
            _evaluator.Eval(new LispExpression(new LispEval()));
        }

        [Test, ExpectedException(typeof(LispProcedureExpectedException))]
        public void TestNotIdenifier()
        {
            _evaluator.Eval(new LispExpression( new LispEval()) { new LispExpression(new LispNumber(5)) });
        }

        [Test, ExpectedException(typeof(LispUnboundIdentifierException))]
        public void TestUnboundIdentifier()
        {
            _evaluator.Eval(new LispExpression(
                new LispEval())
                {
                    new LispExpression(new LispIdentifier("$#F#@F")), new LispExpression(new LispNumber(4)), new LispExpression(new LispNumber(8))
                });
        }

        [Test, ExpectedException(typeof (LispProcedureExpectedException))]
        public void TestIdentifierIsNotProcedure()
        {
            _evaluator.Eval(new LispExpression(
                new LispEval())
                {
                    new LispExpression(new LispIdentifier("pi")), new LispExpression(new LispNumber(4)), new LispExpression(new LispNumber(8))
                });
        }
    }
}
