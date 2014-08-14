using MbUnit.Framework;
using MiniLisp.Exceptions;
using MiniLisp.Expressions;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class ScopeTests
    {
        [Test]
        public void TestAddAndSet()
        {
            Scope scope = new Scope();

            scope.Add("+", new LispBuiltInProcedure(new LispProcedureSignature(), o => o[0]));
            LispBuiltInProcedure proc = scope["+"] as LispBuiltInProcedure;
            Assert.IsNotNull(proc);
            Assert.AreEqual("+", proc.Signature.Identifier);

            scope.Add("d", new LispNumber(5));
            
            LispNumber d = scope["d"] as LispNumber;
            Assert.IsNotNull(d);
            Assert.AreEqual(5, d.Value);

            Scope childScope = new Scope(scope);
            childScope.Add("d", new LispNumber(3));

            d = childScope["d"] as LispNumber;
            Assert.IsNotNull(d);
            Assert.AreEqual(3, d.Value);

            proc = childScope["+"] as LispBuiltInProcedure;
            Assert.IsNotNull(proc);
        }

        [Test, ExpectedException(typeof(LispUnboundIdentifierException))]
        public void TestSetUnbound()
        {
            Scope s = new Scope();
            s["a"] = new LispNumber(5);
        }

        [Test, ExpectedException(typeof(LispDuplicateIdentifierDefinitionException))]
        public void TestDuplicate()
        {
            Scope s = new Scope();
            s.Add("a", new LispNumber(5));
            s.Add("a", new LispNumber(5));
        }
    }
}
