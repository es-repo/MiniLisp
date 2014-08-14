using System.Linq;
using MbUnit.Framework;
using MiniLisp.Expressions;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class InterpretatorTests
    {
        [Test]
        [Row("(define x 3) (define y 4) (define z 5) (define sum +) (define mul *) (mul x (sum y z)) (sum x y z)", "27 12")]

        [Row(@"
(define fn 
  (lambda () 
    (define v3 7) 
    (lambda () 
      v3)))
((fn))", "7")]

        [Row(@"
(define fn 
  (lambda () 
    (define v3 7) 
    (define intf (lambda () 
      v3)) 
    intf))
((fn))", "7")]

        [Row(@"
(define v1 3) 
(define v2 5) 
(define fn 
  (lambda () 
    (define v3 7) 
    (define v1 9)
    (define inf 
      (lambda () 
        (define v4 11) 
        (define inf2 
          (lambda () (* v3 v1)))
        (+ v1 v2 v3 v4 (inf2))))
    inf))
((fn))", "95")]

        [Row(@"
(define mul (lambda (a) (lambda (b) (* a b))))
(define mul2 (mul 2))
(define mul3 (mul 3))

(mul2 4)
(mul3 4)
(mul2 8)
(mul3 8)", "8 12 16 24")]

        [Row(@"
(define fn (lambda (a) (lambda () a)))
(define fn2 (fn 2))
(define fn3 (fn 3))
(fn2)
(fn3)", "2 3")]

        [Row(@"
(define mul (lambda (a) (lambda (b) (* a b))))
(define mul2 (mul 2))
(mul2 4)", "8")]

        [Row(@"
(define mul (lambda (a b) (lambda () (* a b))))
(define mul2 (mul 2 3))
(mul2)
", "6")]

        [Row(@"
(define a 3)
(define b 4)
(define fn 
  (lambda (a c d) 
    (define fni1 (lambda () (* a c b)))
    (define r (fni1)) 
    (define fni2 
      (lambda (d e)
        (lambda (b) (* r d b))))
     (fni2 a b)))

(define fn1 (fn 2 5 6))
(define fn2 (fn 7 8 4))
(fn1 7)
(fn2 3)", "560 4704")]

        [Row(@"(define fn (lambda (d) (define d 3) d)) (fn 5)", "3")]
        public void Test(string input, string expectedOutput)
        {
            Interpretator interpretator = new Interpretator();
            string output = string.Join(" ", interpretator.Read(input).Where(o => !(o is LispVoid)).Select(o => o.ToString()).ToArray());
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        [Row("pi", "3.14159265358979")]
        [Row("+ (+ 2) (+ 2 3 4)", "#<procedure:+> 2 9")]
        [Row("(- 2) (- 2 3)", "-2 -1")]
        [Row("(* 2) (* 2 3)", "2 6")]
        [Row("(/ 2) (/ 3 2)", "2 1.5")]
        [Row("(= 2 2) (= 3 2)", "#t #f")]
        [Row("(not #f) (not true)", "#t #f")]
        public void TestDefaults(string input, string expectedOutput)
        {
            Interpretator interpretator = new Interpretator();
            string output = string.Join(" ", interpretator.Read(input).Where(o => !(o is LispVoid)).Select(o => o.ToString()).ToArray());
            Assert.AreEqual(expectedOutput, output);
        }
    }
}