using System;
using Fractions;

namespace FractionDemo
{/// <summary>
/// A representation of a Matrix using an array of Fractions.
/// </summary>
    class Matrix
    {
        int n, m;
        Fraction[,] values;
        public int N
        {
            get { return n; }
        }
        public int M
        {
            get { return m; }
        }

        public Fraction this[int i, int j]
        {
            get
            {
                return values[i, j];
            }
            set
            {
                values[i, j] = value;
            }
        }

        public Matrix(int n, int m)
        {
            if (n <= 0 || m <= 0)
                throw new ArgumentException();
            this.n = n;
            this.m = m;

            values = new Fraction[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    values[i, j] = new Fraction(0);


        }

        public Matrix Transpose()
        {
            Matrix ret = new Matrix(M, N);
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    ret[j, i] = values[i, j];
            return ret;

        }

        public static Matrix RandomWhole(int n, int m)
        {
            Matrix ret = new Matrix(n, m);
            Random random = new Random();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    ret[i, j] = new Fraction(random.Next(-99, 100));
                }
            }

            return ret;
        }

        public static Matrix RandomFraction(int n, int m)
        {
            Matrix ret = new Matrix(n, m);
            Random random = new Random();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    ret[i, j] = new Fraction(random.Next(-9, 10), random.Next(1, 10));
                }
            }

            return ret;
        }

        public static Matrix RandomDouble(int n, int m)
        {
            Matrix ret = new Matrix(n, m);
            Random random = new Random();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    ret[i, j] = new Fraction(random.NextDouble(), 0.0001, 10);
                }
            }

            return ret;
        }

        /// <summary>
        /// Converts the Matrix into a string value
        /// </summary>
        /// <returns> The WolframAlpha representation of a matrix.</returns>
        public override string ToString()
        {
            string s = "{";
            for (int i = 0; i < N; i++)
            {
                s += '{';
                for (int j = 0; j < M; j++)
                {
                    s += values[i, j];
                    if (j != M - 1)
                        s += ", ";
                }
                s += '}';
                if (i != N - 1)
                    s += ", ";
            }
            s += "}";
            return s;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.N != b.N || a.M != b.M)
            {
                throw new InvalidOperationException();
            }

            Matrix ret = new Matrix(a.N, a.M);
            for (int i = 0; i < a.N; i++)
                for (int j = 0; j < a.M; j++)
                    ret[i, j] = a[i, j] + b[i, j];
            return ret;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.N != b.N || a.M != b.M)
            {
                throw new InvalidOperationException();
            }

            Matrix ret = new Matrix(a.N, a.M);
            for (int i = 0; i < a.N; i++)
                for (int j = 0; j < a.M; j++)
                    ret[i, j] = a[i, j] - b[i, j];
            return ret;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.M != b.N)
            {
                throw new InvalidOperationException();
            }

            Matrix ret = new Matrix(a.N, b.M);
            for (int i = 0; i < a.N; i++)
                for (int j = 0; j < b.M; j++)
                    for (int k = 0; k < a.M; k++)
                        ret[i, j] += a[i, k] * b[k, j];
            return ret;
        }
    }

    class MatrixDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Matrix multiplication with whole numbers:");
            Matrix a = Matrix.RandomWhole(2, 3);
            Matrix b = Matrix.RandomWhole(3, 2);
            Matrix c = a * b;
            Console.WriteLine("{0} * {1} = {2}", a, b, c);

            Console.WriteLine("Matrix multiplication with simple fractions:");
            a = Matrix.RandomFraction(2, 3);
            b = Matrix.RandomFraction(3, 2);
            c = a * b;
            Console.WriteLine("{0} * {1} = {2}", a, b, c);

            Console.WriteLine("Matrix multiplication with doubles approximated as fractions:");
            a = Matrix.RandomDouble(2, 3);
            b = Matrix.RandomDouble(3, 2);
            c = a * b;
            Console.WriteLine("{0} * {1} = {2}", a, b, c);
        }
    }
}
