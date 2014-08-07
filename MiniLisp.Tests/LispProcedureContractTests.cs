using System;
using System.Collections.Generic;
using MbUnit.Framework;
using MiniLisp.LispObjects;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class LispProcedureContractTests
    {
        [Test]
        [Row(2, 3, false, ExpectedException = typeof(LispProcedureArityMismatchException))]
        [Row(4, 3, false, ExpectedException = typeof(LispProcedureArityMismatchException))]
        [Row(4, 3, true)]
        [Row(4, -1, false)]
        public void TestAssertAriry(int givenArgumentsCount, int arity, bool atLeast)
        {
            new LispProcedureContract("proc", null, 2).Assert(new LispObject[givenArgumentsCount]);
        }

        [Test]
        public void TestAssertArgumentTypes()
        {
            LispProcedureContract contract = new LispProcedureContract("proc",
                new LispProcedureArgumentTypes(new Dictionary<int, Type>
                {
                    {1, typeof (LispNumber)},
                    {3, typeof (LispString)}
                }, typeof (LispBoolean)));

            contract.Assert(new LispObject[] { new LispBoolean(false), new LispNumber(0), new LispBoolean(false), new LispBoolean(false), new LispString("") });

            try
            {
                contract.Assert(new LispObject[] { new LispNumber(0), new LispNumber(0), new LispBoolean(false), new LispBoolean(false), new LispString("") });
                Assert.Fail(typeof(LispProcedureContractViolationException).Name + " is expected.");
            }
            catch (LispProcedureContractViolationException) { }

            try
            {
                contract.Assert(new LispObject[] { new LispBoolean(false), new LispNumber(0), new LispBoolean(false), new LispBoolean(false), new LispNumber(0) });
                Assert.Fail(typeof(LispProcedureContractViolationException).Name + " is expected.");

            }
            catch (LispProcedureContractViolationException) { }
        }
    }
}
