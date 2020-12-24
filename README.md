# Fractions
A simple fraction library for C#

### The contents
This library describes a fraction as a struct containing a numerator (`ulong`), a denominator (`ulong`) and a sign (`int`). 
It also contains a number of overloaded operators and some additional functions.

### Using the library
The struct contains different constructors that allow you to describe the numerator and the denominator of the fraction: 
```
Fraction f1 = new Fraction(12);
Fraction f2 = new Fraction(12, 13);
```
Or you can use a double instead:
```
Fraction f1 = new Fraction(0.25);
Fraction f2 = new Fraction(17/13);
```
These constructors will return the fractions 1/4 and 17/13, without explicitly specifying the denominator.
This is accomplished using continuous fractions.

### Overloaded operations
The overloaded operations are: 
```
Unary: -;
Binary: +, -, *, /, ==, !=, >, <, >=, <=;
```
All of these operators are overloaded for `double` and `long` values as well.

### Approximating irrational numbers
Some numbers, for example Pi and the Sqrt(2), cannot be described using fractions. 
Because of this, we will be approximating the fraction if we think that it is an irrational number.
The constructor for Fractions is using the `DoubleToFraction` function, in which we can specify the epsilon and the maximum amount of iterations that the function will check.
For example:
```
Fraction f = new Fraction(Math.Pi, 1e-6, 20);
```
This will return an extremely close approximation for Pi, the error approximately equals 0.
You can also decrease the amount of iterations, which will decrease the accuracy.
```
Fraction f = new Fraction(Math.Pi, 1e-6, 5);
```
The error will approximately equal to 1,224e-10.

### Shortcomings of the DoubleToFraction algorithm
The algorithm is very good at calculating the exact value for simple fractions, like 1/3 or 11/13.
But it becomes inaccurate with more complicated fractions like 19231312 / 81912123 or fractions that are very close to a whole number like 999999/1000000.
This is partially because of floating point errors and partially because the program will think that the number is an irrational number when it isn't because it doesn't go through enough iterations.
