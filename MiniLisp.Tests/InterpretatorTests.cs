using System.Linq;
using MbUnit.Framework;
using MiniLisp.LispObjects;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class InterpretatorTests
    {
        [Test]
        [Row("pi", "3.14159265358979")]
        [Row("+", "#<procedure:+>")]
        [Row("(define x 3) (define y 4) (define z 5) (define sum +) (define mul *) (mul x (sum y z)) (sum x y z)", "27 12")]
        public void Test(string input, string expectedOutput)
        {
            Interpretator interpretator = new Interpretator();

            string output = string.Join(" ", interpretator.Read(input).Where(o => !(o is LispVoid)).Select(o => o.ToString()).ToArray());
            Assert.AreEqual(expectedOutput, output);
        }
    }
}
