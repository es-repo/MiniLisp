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
        [Row("(define var '(cons 2.45 #f))\r\n(* 3 var) \"\" \"(ab 3 4)\"", new[] { "(", "define", "var", "'", "(", "cons", "2.45", "#f", ")", ")", "(", "*", "3", "var", ")", "\"\"", "\"(ab 3 4)\"" })]
        [Row("\"\" \"(ab)\"", new[] { "\"\"", "\"(ab)\"" })]
        public void TestTokenize(string input, string[] expectedTokens)
        {
            string[] tokens = Parser.Tokenize(input).ToArray();
            Assert.AreElementsEqual(expectedTokens, tokens);
        }

        [Test]
        [Row("", "")]
        [Row("5", "5")]
        [Row("5 false 3 nil #t define id \"\" \"ab c\" \"(+ 2 3)\"", "5 #f 3 nil #t define id \"\" \"ab c\" \"(+ 2 3)\"")]
        [Row("()", "()")]
        [Row("(+ 3 2)", "(+ 3 2)")]
        [Row("(+ 3 (* 2 5))", "(+ 3 (* 2 5))")]
        [Row("(+ 3 (* (-  2)(+ 5)))", "(+ 3 (* (- 2) (+ 5)))")]
        [Row("(define d 5) (+ d d) (define str \"hello world!\")", "(define d 5) (+ d d) (define str \"hello world!\")")]
        public void TestParse(string input, string expectedOutput)
        {
            LispExpression[] expressions = Parser.Parse(input).ToArray();
            string output = string.Join(" ", expressions.Select(e => e.ToString()).ToArray());
            Assert.AreEqual(expectedOutput, output);
        }
    }
}
