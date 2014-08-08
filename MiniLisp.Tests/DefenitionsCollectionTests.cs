﻿using System;
using MbUnit.Framework;
using MiniLisp.LispObjects;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class DefenitionsCollectionTests
    {
        [Test]
        public void TestAdd()
        {
            DefenitionsCollection defenitions = new DefenitionsCollection();

            defenitions.Add("+", new LispProcedure(new LispProcedureSignature(), o => o[0]));
            LispProcedure proc = defenitions["+"] as LispProcedure;
            Assert.IsNotNull(proc);
            Assert.AreEqual("+", proc.Signature.Identifier);

            defenitions.Add("d", new LispNumber(5));
            defenitions.Add("e", new LispIdentifier("d"));

            LispNumber d = defenitions["d"] as LispNumber;
            Assert.IsNotNull(d);
            Assert.AreEqual(5, d.Value);

            LispNumber e = defenitions["e"] as LispNumber;
            Assert.IsNotNull(e);
            Assert.AreEqual(5, e.Value);
        }

        [Test, ExpectedException(typeof(LispUnboundIdentifierException))]
        public void TestAddFailed()
        {
            DefenitionsCollection defenitions = new DefenitionsCollection();
            defenitions.Add("a", new LispIdentifier("b"));
        }
    }
}