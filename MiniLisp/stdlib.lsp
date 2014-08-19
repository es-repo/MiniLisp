(define (null? o) (equal? o null))

(define (map p l)
  (if (null? l)
      null
      (cons (p (car l))
            (map p (cdr l)))))

(define (for-each proc list)
  (cond ((null? list) (void))
        ((proc (car list))
              (for-each proc
                        (cdr list)))))

(define pi 3.14159265358979)

(define fact (lambda (n) (if (<= n 1) 1 (* n (fact (- n 1))))))

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