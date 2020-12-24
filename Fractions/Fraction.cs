using System;
using System.Collections.Generic;

namespace Fractions
{
    /// <summary>
    /// Represents a fraction with 8 byte unsigned int numerator and denominator with a sign.
    /// </summary>
    public struct Fraction
    {
        ulong numerator, denominator;
        /// <summary>
        /// The current sign of the fraction.
        /// </summary>
        public int sign;
        /// <summary>
        /// The signed numerator of the fraction.
        /// </summary>
        public long Numerator
        {
            get { return (long)numerator * sign; }
            set
            {
                long v = value;

                if (v == 0) {
                    sign = 0;
                    denominator = 1;
                }
                else
                {
                    if(sign == 0)
                        sign = 1;
                    if (v < 0)
                    {
                        v *= -1;
                        sign *= -1;
                    }
                }
                numerator = (ulong)v;

                ApplyDivision();
            }
        }

        /// <summary>
        /// Returns the unsigned numerator of the fraction
        /// </summary>
        public long UNumerator
        {
            get { return (long)numerator; }
        }
        /// <summary>
        /// The unsugned denominator of the fraction. Cannot equal to 0.
        /// </summary>
        public long Denominator
        {
            get { return (long)denominator; }
            set
            {
                long v = value;

                if (v == 0)
                    throw new DivideByZeroException();

                if (v < 0)
                {
                    v *= -1;
                    sign *= -1;
                }
                denominator = (ulong)v;

                ApplyDivision();
            }
        }
        /// <summary>
        /// The unsigned proper numerator of the fraction.
        /// </summary>
        public long ProperNumerator
        {
            get
            {
                return (long)numerator % Denominator;
            }
        }
        /// <summary>
        /// Returns the signed whole number of the mixed fraction
        /// </summary>
        public long WholeNumber
        {
            get
            {
                return Numerator / Denominator;
            }
        }
        /// <summary>
        /// The value that you get by diving the numerator by the denominator. 
        /// </summary>
        public double Value
        {
            get { return (double)numerator / denominator * sign; }
        }
        public Fraction(ulong numerator, ulong denominator): this((long) numerator, (long)denominator) { }
        public Fraction(long numerator) : this(numerator, 1) { }
        public Fraction(ulong numerator) : this(numerator, 1) { }
        public Fraction(long numerator, long denominator)
        {
            if (denominator == 0)
                throw new DivideByZeroException();
            if (numerator == 0)
                denominator = 1;

            sign = Math.Sign(numerator) * Math.Sign(denominator);
            this.numerator = (ulong)Math.Abs(numerator);
            this.denominator = (ulong)Math.Abs(denominator);

            ApplyDivision();
        }
        /// <inheritdoc cref="DoubleToFraction(double, double, int)"/>
        public Fraction(double value, double eps = 0.0001, int maxIterations = 20)
        {
            Fraction fr = DoubleToFraction(value, eps, maxIterations);
            this.numerator = fr.numerator;
            this.denominator = fr.denominator;
            this.sign = fr.sign;
        }
        /// <summary>
        /// Divides the numerator and the denominator by their greatest common denominator.
        /// </summary>
        void ApplyDivision()
        {
            if (numerator == 0)
                return;
            ulong gcd = GCD(numerator, denominator);
            numerator /= gcd;
            denominator /= gcd;
        }
        /// <summary>Reverses the current fraction.</summary>
        /// <returns>A new fraction, the denominator of which equals to the numerator of the original fraciton and vice versa.</returns>
        public Fraction Reverse()
        {
            return new Fraction(Denominator, Numerator);
        }

        public static ulong GCD(ulong a, ulong b)
        {
            while (a > 0 && b > 0)
                if (a > b)
                    a = a % b;
                else
                    b = b % a;
            return (a + b);
        }
        /// <summary>
        /// Converts a given value into a fraction using continuous fractions.
        /// </summary>
        /// <param name="value">The input value which is converted to a fraction.</param>
        /// <param name="eps">Used to check if the current proper fraction equals to zero.</param>
        /// <param name="maxIterations">The maximum amount amount of iterations that the function checks before it decides <paramref name="value"/> is an irrational number.</param>
        /// <returns>The fraction that approximately equals the given value.</returns>
        public static Fraction DoubleToFraction(double value, double eps = 0.0001, int maxIterations = 20)
        {
            if (eps <= 0 || maxIterations <= 0)
                throw new ArgumentException();
            // These lists contain the proper fractions and the whole numbers
            List<double> fullNumerators = new List<double>();
            List<double> fractions = new List<double>();

            int sign = Math.Sign(value);

            double absFraction = Math.Abs(value);
            fullNumerators.Add(Math.Floor(absFraction + eps));
            fractions.Add(Math.Abs(absFraction - fullNumerators[0]));
            int i;
            // Populate the next proper fraction and the next whole number with the results of reversing the current proper fraction
            for (i = 0; fractions[i] > eps && i < maxIterations; i++)
            {
                double reverse = 1 / fractions[i];

                fullNumerators.Add(Math.Floor(reverse + eps));
                fractions.Add(Math.Abs(reverse - fullNumerators[i + 1]));
            }

            i--;
            // if i equals -1 then the original value was a whole number
            if (i == -1)
                return new Fraction((long)value);
            // These are the final results of the algorithm
            double finalDenominator = 1;
            double finalNumerator = 0;

            for(int j = i; j >= 0; j--)
            {
                double denominator;
                // If we are in the first iteration, then we need to find the first denominator. fullNumerators[i + 1] should have the correct denimerator, or the approximation of it if the input was an irrational number
                if (j == i)
                    denominator = 1 / fractions[j];
                else  // Otherwise, we don't need to find the denominator, as it is equal to the previous numerator (which we already swapped in the last iteration)
                    denominator = finalDenominator;
                // The current numerator equals to previous denominator (which we already swapped) plus the current denominator times the whole number of the jth mixed fraction
                double numerator = denominator * fullNumerators[j] + finalNumerator;
                // If we are in the first iteration, then we have to increase the numerator by denominator * (1 / denominator), which equals 1
                if (i == j)
                    numerator++;
                if(j != 0) // If we are not in the last iteration, then we have to swap the denominator and the numerator
                {
                    finalDenominator = numerator;
                    finalNumerator = denominator;
                }
                else
                {
                    finalDenominator = denominator;
                    finalNumerator = numerator;
                }
            }
            return new Fraction(Convert.ToInt64(finalNumerator)* sign, Convert.ToInt64(finalDenominator));
        }
        public override string ToString() 
        {
            if (Numerator == 0)
                return "0";
            if (Denominator == 1)
                return Numerator.ToString();
            return $"{Numerator}/{Denominator}";
        }

        public static implicit operator double(Fraction f) => f.Value;
        public static implicit operator Fraction(double d) => DoubleToFraction(d);
        public static explicit operator long(Fraction f) => Convert.ToInt64(f.Value);
        public static implicit operator Fraction(long l) => new Fraction(l);

        public static Fraction operator -(Fraction a)
        {
            return new Fraction(-a.Numerator, a.Denominator);
            
        }
        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        }
        public static Fraction operator *(Fraction a, long b)
        {
            return new Fraction(a.Numerator * b, a.Denominator);
        }
        public static Fraction operator *(long a, Fraction b) => b * a;
        public static Fraction operator *(Fraction a, double b)
        {
            Fraction bFraction = new Fraction(b);
            return a * bFraction;
        }
        public static Fraction operator *(double a, Fraction b) => b * a;
        public static Fraction operator /(Fraction a, Fraction b)
        {
            return new Fraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        }
        public static Fraction operator /(Fraction a, long b)
        {
            return new Fraction(a.Numerator, a.Denominator * b);
        }
        public static Fraction operator /(long a, Fraction b) => b.Reverse() * a;
        public static Fraction operator /(Fraction a, double b)
        {
            return a / new Fraction(b);
        }
        public static Fraction operator /(double a, Fraction b) => b.Reverse() * a;
        public static Fraction operator +(Fraction a, Fraction b)
        {
            return new Fraction(a.Numerator * b.Denominator + b.Numerator * a.Denominator, a.Denominator * b.Denominator);
        }
        public static Fraction operator +(Fraction a, long b)
        {
            return new Fraction(a.Numerator + b * a.Denominator, a.Denominator);
        }
        public static Fraction operator +(long a, Fraction b) => b + a;
        public static Fraction operator +(Fraction a, double b)
        {
            return a + new Fraction(b);
        }
        public static Fraction operator +(double a, Fraction b) => b + a;
        public static Fraction operator -(Fraction a, Fraction b)
        {
            return new Fraction(a.Numerator * b.Denominator - b.Numerator * a.Denominator, a.Denominator * b.Denominator);
        }
        public static Fraction operator -(Fraction a, long b)
        {
            return new Fraction(a.Numerator - b * a.Denominator, a.Denominator);
        }
        public static Fraction operator -(long a, Fraction b) => -b + a;
        public static Fraction operator -(Fraction a, double b)
        {
            return a - new Fraction(b);
        }
        public static Fraction operator -(double a, Fraction b) => -b + a;
    }
}
