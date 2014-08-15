(define pi 3.14159265358979)

(define fact (lambda (n) (if (<= n 1) 1 (* n (fact (- n 1))))))

(define (cons a b) (lambda (o) (if o a b)))
(define (car p) (p #t))
(define (cdr p) (p #f))

(define (sqr a) (* a a))
(define (abs x) (if (< x 0) (- x) x))
(define (average x y) (/ (+ x y) 2))

(define (mean-sqr x y) (average (sqr x) (sqr y)))

(define (sqrt x)
  (define (improve guess)
    (average guess (/ x guess)))
  (define (good-enough? guess)
    (< (abs (- (sqr guess) x)) .001))
  (define (try guess)
    (if (good-enough? guess)
        guess
        (try (improve guess))))
  (try 1))