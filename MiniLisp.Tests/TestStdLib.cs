using System.Linq;
using MbUnit.Framework;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class TestStdLib
    {
        [Test]
        [Row("pi", "3.14159265358979")]
        public void Test(string input, string expectedOutput)
        {
            Interpretator interpretator = new Interpretator();

            string output = string.Join(" ", interpretator.Read(input).Select(o => o.ToString()).ToArray());
            Assert.AreEqual(output, expectedOutput);
        }
    }
}
