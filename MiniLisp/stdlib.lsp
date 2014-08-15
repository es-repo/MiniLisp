(define pi 3.14159265358979)

(define fact (lambda (n) (if (<= n 1) 1 (* n (fact (- n 1))))))