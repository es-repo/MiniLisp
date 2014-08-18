using System.Linq;
using MbUnit.Framework;
using MiniLisp.Exceptions;
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

        [Row(@"
(define d 4)
(set! d (lambda (d) (define d 3) (set! d (* d d)) (* d d)))
d
(d 15)", "#<procedure:d> 81")]

        [Row(@"(define fn (lambda () (define fn2 (lambda () (define mul *) mul)) mul)) (fn)", "", ExpectedException = typeof(LispUnboundIdentifierException))]

        [Row(@"(define fn (lambda () (fn))) 3", "3")]

        [Row(@"(define (fn fn) fn) (fn 3)", "3")]

        [Row(@"
(define (mul a) 
  (lambda (b) (* a b)))

(define mul2 (mul 2))
(define mul3 (mul 4))
(set! mul3 (mul 3))

(mul2 4)
(mul3 4)", "8 12")]

        [Row(@"
(define (fib n)
  (if (< n 2) 
      n
      (+ (fib (- n 1)) (fib (- n 2)))))

(fib 0) (fib 1) (fib 2) (fib 3) (fib 4) (fib 5) (fib 6) (fib 7) (fib 8)", "0 1 1 2 3 5 8 13 21")]

        [Row(@"
(define d 2)
(if #t (set! d 5 ) (* 3 4))
d", "5")]

        [Row(@"(cond (#t) (6))", "#t")]

        [Row(@"(define d 1)
(cond (#t) (#f 3))
(cond (#t (set! d 3) (set! d 5) 7) (#f (set! d 11)))
d
(cond (#f (set! d 3) (set! d 5) 7) ((set! d 11)))
d", "#t 7 5 11")]

        [Row(@"(let () 3 4)  (let ((a (+ 3 4))) (* a a)) ((let ((a (+ 3 4))) (lambda(b) (* a b))) 3)", "4 49 21")]
        [Row(@"(let ((x 5)) (define y x) y) (let ((x 5)) (let ((x x)) x))", "5 5")]

        [Row(@"
(let ((x 5))
    (let ((x 2)
          (y x))
      (cons y x)))", "(5 . 2)")]

        public void Test(string input, string expectedOutput)
        {
            Interpretator interpretator = new Interpretator();
            string output = string.Join(" ", interpretator.Read(input).Where(o => !(o is LispVoid)).Select(o => o.ToString()).ToArray());
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        [Row("+ (+ 2) (+ 2 3 4)", "#<procedure:+> 2 9")]
        [Row("(- 2) (- 2 3)", "-2 -1")]
        [Row("(* 2) (* 2 3)", "2 6")]
        [Row("(/ 2) (/ 3 2)", "2 1.5")]
        [Row("(= 2 2 2) (= 3 3 2)", "#t #f")]
        [Row("(!= 2 2 2) (!= 3 3 2) (!= 3 4 4)", "#f #f #t")]
        [Row("(> 2 2 2) (> 3 2 4) (> 3 1 2)", "#f #f #t")]
        [Row("(>= 2 2 3) (>= 3 3 3) (> 3 1 2)", "#f #t #t")]
        [Row("(< 2 2 2) (< 3 2 4) (< 3 6 5)", "#f #f #t")]
        [Row("(<= 2 2 1) (<= 3 3 3) (< 3 4 5)", "#f #t #t")]
        [Row("(not #f) (not true)", "#t #f")]
        [Row("(or #f) (or #t) (or true true false) (or true true)", "#f #t #t #t")]
        [Row("(and #f) (and #t) (and true true false) (and true true)", "#f #t #f #t")]
        [Row("(equal? 1 1) (equal? \"ab\" \"ab\") (equal? 2 1) (equal? #t 1)", "#t #t #f #f")]
        [Row("(cons (cons 1 2) 3) (car (cons (cons 1 2) 3)) (cdr (cons 2 3))", "((1 . 2) . 3) (1 . 2) 3")]
        public void TestDefaults(string input, string expectedOutput)
        {
            Interpretator interpretator = new Interpretator();
            string output = string.Join(" ", interpretator.Read(input).Where(o => !(o is LispVoid)).Select(o => o.ToString()).ToArray());
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        [Row("pi", "3.14159265358979")]
        [Row("(fact 7)", "5040")]
        [Row("(define p (cons 1 2)) (car p) (cdr p)", "1 2")]
        [Row("(sqrt 81)", "9.00001129879022")]
        public void TestStdLib(string input, string expectedOutput)
        {
            Interpretator interpretator = new Interpretator();
            string output = string.Join(" ", interpretator.Read(input).Where(o => !(o is LispVoid)).Select(o => o.ToString()).ToArray());
            Assert.AreEqual(expectedOutput, output);
        }
    }
}