using System;
using MbUnit.Framework;
using MiniLisp.Exceptions;
using MiniLisp.Expressions;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class LispProcedureContractVerificationTests
    {
        [Test]
        [Row(2, 3, null, ExpectedException = typeof(LispProcedureArityMismatchException))]
        [Row(4, 3, null, ExpectedException = typeof(LispProcedureArityMismatchException))]
        [Row(4, 3, typeof(LispNumber))]
        [Row(4, -1, null)]
        public void TestAssertAriry(int givenArgumentsCount, int arity, Type nonNamedParametersType)
        {
            LispProcedureSignature signature = new LispProcedureSignature(null, nonNamedParametersType, arity);
            LispProcedureContractVerification.Assert(signature, new LispExpressionElement[givenArgumentsCount]);
        }

        [Test]
        public void TestAssertArgumentTypes()
        {
            LispProcedureSignature signature = new LispProcedureSignature(
                new []
                {
                    new LispProcedureParameter("", typeof(LispBoolean)), 
                    new LispProcedureParameter("", typeof(LispNumber)),
                    new LispProcedureParameter("", typeof(LispValueElement))
                }, typeof (LispBoolean));

            LispProcedureContractVerification.Assert(signature, new LispExpressionElement[] { new LispBoolean(false), new LispNumber(0), new LispBoolean(false), new LispBoolean(false), new LispBoolean(false) });

            try
            {
                LispProcedureContractVerification.Assert(signature, new LispExpressionElement[] { new LispNumber(0), new LispBoolean(false), new LispBoolean(false), new LispBoolean(false), new LispBoolean(false) });
                Assert.Fail(typeof(LispProcedureContractViolationException).Name + " is expected.");
            }
            catch (LispProcedureContractViolationException) { }

            try
            {
                LispProcedureContractVerification.Assert(signature, new LispExpressionElement[] { new LispBoolean(false), new LispNumber(0), new LispNumber(0), new LispBoolean(false), new LispNumber(0) });
                Assert.Fail(typeof(LispProcedureContractViolationException).Name + " is expected.");

            }
            catch (LispProcedureContractViolationException) { }
        }
    }
}
