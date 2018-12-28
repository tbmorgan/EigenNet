using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EigenWrapper;
using System.Diagnostics;

namespace WrapperTest
{
    class Program
    {
        static int size = 100;
        static int testIterations = 1000;

        static void Main(string[] args)
        {
            Console.WriteLine("Running Eigen wrapper test:");
            EigenWrapper.MatrixXd matrixA = new EigenWrapper.MatrixXd(size, size);
            EigenWrapper.MatrixXd matrixB = new EigenWrapper.MatrixXd(size, size);
            EigenWrapper.MatrixXd matrixC;
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < testIterations; i++)
            {
                matrixC = matrixA * matrixB;
            }
            watch.Stop();
            Console.WriteLine("Processing took: " + watch.ElapsedMilliseconds);

            Console.WriteLine("Testing with C# arrays...");
            double[,] matA = MatrixMathCS.RandomMatrix(size, size, 100, -100);
            double[,] matB = MatrixMathCS.RandomMatrix(size, size, 100, -100);
            double[,] C;

            watch.Start();
            for (int i = 0; i < testIterations; i++)
            {
                C = MatrixMath.MultiplyMatrices(matA, matB);
            }
            watch.Stop();
            Console.WriteLine("Processing took: {0}", watch.ElapsedMilliseconds);

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
