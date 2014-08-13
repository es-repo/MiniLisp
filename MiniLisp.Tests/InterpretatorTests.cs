using System.Linq;
using MbUnit.Framework;
using MiniLisp.Expressions;

namespace MiniLisp.Tests
{
    [TestFixture]
    public class InterpretatorTests
    {
        [Test]
        [Row("pi", "3.14159265358979")]
        [Row("+", "#<procedure:+>")]
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

        //        [Row(@"
        //(define mul 
        //  (lambda (a) 
        //    (define mulx 
        //      (lambda (b) 
        //        (* a b))) 
        //    mulx))
        //  
        //(define mul3 (mul 3))
        //(define mul5 (mul 5))
        // 
        //(mul3 3)
        //(mul5 3) ")]
        public void Test(string input, string expectedOutput)
        {
            Interpretator interpretator = new Interpretator();
            string output = string.Join(" ", interpretator.Read(input).Where(o => !(o is LispVoid)).Select(o => o.ToString()).ToArray());
            Assert.AreEqual(expectedOutput, output);
        }
    }
}
