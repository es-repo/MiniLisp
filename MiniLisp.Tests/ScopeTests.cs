using MbUnit.Framework;
using MiniLisp.LispExpressionElements;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class ScopeTests
    {
        [Test]
        public void TestAdd()
        {
            Scope scope = new Scope();

            scope["+"] = new LispBuiltInProcedure(new ProcedureSignature(), o => o[0]);
            LispBuiltInProcedure proc = scope["+"] as LispBuiltInProcedure;
            Assert.IsNotNull(proc);
            Assert.AreEqual("+", proc.Signature.Identifier);

            scope["d"] = new LispNumber(5);
            
            LispNumber d = scope["d"] as LispNumber;
            Assert.IsNotNull(d);
            Assert.AreEqual(5, d.Value);

            Scope childScope = new Scope(scope);
            childScope["d"] = new LispNumber(3);

            d = childScope["d"] as LispNumber;
            Assert.IsNotNull(d);
            Assert.AreEqual(3, d.Value);

            proc = childScope["+"] as LispBuiltInProcedure;
            Assert.IsNotNull(proc);
        }
    }
}
