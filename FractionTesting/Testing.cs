using System;
using Fractions;

namespace FractionTesting
{
    class Testing
    {
        /// <summary> Test </summary>
        static void OperationTest(int aNumerator, int aDenominator, int bNumerator, int bDenominator, double eps = 1e-6)
        {
            Fraction a = new Fraction(aNumerator, aDenominator), b = new Fraction(bNumerator, bDenominator);
            double aControl = (double)aNumerator / aDenominator, bControl = (double)bNumerator / bDenominator;
            Fraction test;
            double testControl;
            bool error = false;
            test = a + b; testControl = aControl + bControl; 
            if(Math.Abs(test.Value - testControl) > eps)
            {
                error = true;
                Console.WriteLine("Error in addition test: ({0}) + ({1}) = {2, 3}, doesn't equal {3, 3}", a, b, test.Value, testControl);
            }

            test = a - b; testControl = aControl - bControl;
            if (Math.Abs(test.Value - testControl) > eps)
            {
                error = true;
                Console.WriteLine("Error in subtraction test: ({0}) - ({1}) = {2, 3}, doesn't equal {3, 3}", a, b, test.Value, testControl);
            }

            test = a * b; testControl = aControl * bControl;
            if (Math.Abs(test.Value - testControl) > eps)
            {
                error = true;
                Console.WriteLine("Error in multiplication test: ({0}) - ({1}) = {2, 3}, doesn't equal {3, 3}", a, b, test.Value, testControl);
            }

            test = a / b; testControl = aControl / bControl;
            if (Math.Abs(test.Value - testControl) > eps)
            {
                error = true;
                Console.WriteLine("Error in division test: ({0}) - ({1}) = {2, 3}, doesn't equal {3, 3}", a, b, test.Value, testControl);
            }

            if (!error)
            {
                Console.WriteLine("No errors in operation test");
            }
        }

        /// <summary> Tests the Fraction.DoubleToFraction function. </summary>
        static void TestDoubleToFraction(int numeratorBounds = 1000, int denominatorBounds = 1000, double eps = 0.0001, int maxIterations = 15)
        {
            if(numeratorBounds < 0 || denominatorBounds <= 0)
            {
                Console.WriteLine("Double to fraction test: incorrect bounds");
                return;
            }
            bool errors = false;
            for (int i = -numeratorBounds; i <= numeratorBounds; i++)
            {
                for (int j = 1; j <= denominatorBounds; j++)
                {
                    if (i < 0 && j < 0 || Fraction.GCD((uint)Math.Abs(i), (uint)Math.Abs(j)) > 1)
                        continue;
                    Fraction fraction = new Fraction((double)i / j, eps, maxIterations);
                    if (fraction.Denominator != j || fraction.Numerator != i)
                    {
                        errors = true;
                        Console.WriteLine("Error at {0}, {1}, result is {2}. Error: {3}.", i, j, fraction, Math.Abs((double)i / j - fraction.Value));
                        fraction = Fraction.DoubleToFraction((double)i / j, eps, maxIterations);
                    }
                }
            }
            if (!errors)
                Console.WriteLine("No errors in \"Double To Fraction\" test");
        }
        /// <summary>
        /// Tests the accuracy of Fraction.DoubleToFraction function with an irrational number as an input
        /// </summary>
        static void PIApproximationTest(double eps1 = 0.01, double eps2 = 0.000001, int maxIterations1 = 5, int maxIterations2 = 20)
        {
            Console.WriteLine("Pi approximation test:");
            Fraction PI1 = new Fraction(Math.PI, eps1, maxIterations1);
            Fraction PI2 = new Fraction(Math.PI, eps2, maxIterations1);
            Fraction PI3 = new Fraction(Math.PI, eps1, maxIterations2);
            Fraction PI4 = new Fraction(Math.PI, eps2, maxIterations2);
            Console.WriteLine("eps = {2, -5}, max iterations = {3, 2}: {0}, error = {1}", PI1, Math.Abs(Math.PI - PI1.Value), eps1, maxIterations1);
            Console.WriteLine("eps = {2, -5}, max iterations = {3, 2}: {0}, error = {1}", PI2, Math.Abs(Math.PI - PI2.Value), eps2, maxIterations1);
            Console.WriteLine("eps = {2, -5}, max iterations = {3, 2}: {0}, error = {1}", PI3, Math.Abs(Math.PI - PI3.Value), eps1, maxIterations2);
            Console.WriteLine("eps = {2, -5}, max iterations = {3, 2}: {0}, error = {1}", PI4, Math.Abs(Math.PI - PI4.Value), eps2, maxIterations2);
        }

        static void Main(string[] args)
        {
            OperationTest(100, 57, 92, 11);
            TestDoubleToFraction(1000, 1000, 0.0001, 15);
            PIApproximationTest(0.01, 1e-6, 5, 20);
            Console.WriteLine(Fraction.DoubleToFraction((double)99999 / 100000).ToString());
        }
    }
}
