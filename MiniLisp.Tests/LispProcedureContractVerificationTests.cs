using System;
using System.Collections.Generic;
using MbUnit.Framework;
using MiniLisp.Exceptions;
using MiniLisp.Expressions;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class LispProcedureContractVerificationTests
    {
        [Test]
        [Row(2, 3, false, ExpectedException = typeof(LispProcedureArityMismatchException))]
        [Row(4, 3, false, ExpectedException = typeof(LispProcedureArityMismatchException))]
        [Row(4, 3, true)]
        [Row(4, -1, false)]
        public void TestAssertAriry(int givenArgumentsCount, int arity, bool atLeast)
        {
            LispProcedureSignature signature = new LispProcedureSignature(null, arity, atLeast);
            LispProcedureContractVerification.Assert(signature, new LispExpressionElement[givenArgumentsCount]);
        }

        [Test]
        public void TestAssertArgumentTypes()
        {
            LispProcedureSignature signature = new LispProcedureSignature(
                new LispProcedureParameterTypes(new Dictionary<int, Type>
                {
                    {1, typeof (LispNumber)},
                    {4, typeof (LispString)}
                }, typeof (LispBoolean)));

            LispProcedureContractVerification.Assert(signature, new LispExpressionElement[] { new LispBoolean(false), new LispNumber(0), new LispBoolean(false), new LispBoolean(false), new LispString("") });

            try
            {
                LispProcedureContractVerification.Assert(signature, new LispExpressionElement[] { new LispNumber(0), new LispNumber(0), new LispBoolean(false), new LispBoolean(false), new LispString("") });
                Assert.Fail(typeof(LispProcedureContractViolationException).Name + " is expected.");
            }
            catch (LispProcedureContractViolationException) { }

            try
            {
                LispProcedureContractVerification.Assert(signature, new LispExpressionElement[] { new LispBoolean(false), new LispNumber(0), new LispBoolean(false), new LispBoolean(false), new LispNumber(0) });
                Assert.Fail(typeof(LispProcedureContractViolationException).Name + " is expected.");

            }
            catch (LispProcedureContractViolationException) { }
        }
    }
}
