using System.Linq;
using MbUnit.Framework;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        [Row("", new string[] { })]
        [Row("()'d", new [] {"(", ")", "'", "d"})]
        [Row("(define var '(cons 2.45 #f))\r\n(* 3 var)", new[] { "(", "define", "var", "'", "(", "cons", "2.45", "#f", ")", ")", "(", "*", "3", "var", ")" })]
        public void TestTokenize(string input, string[] expectedTokens)
        {
            string[] tokens = Parser.Tokenize(input).ToArray();
            Assert.AreElementsEqual(expectedTokens, tokens);
        }

        [Test]
        [Row("", "")]
        [Row("5", "5")]
        [Row("5 false 3 nil #t define id", "5 #f 3 nil #t define id")]
        public void TestParse(string input, string expectedOutput)
        {
            LispExpression[] expressions = Parser.Parse(input).ToArray();
            string output = string.Join(" ", expressions.Select(e => e.ToString()).ToArray());
            Assert.AreEqual(expectedOutput, output);
        }
    }
}
