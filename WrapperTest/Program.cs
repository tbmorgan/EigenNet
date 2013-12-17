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
        static void Main(string[] args)
        {
            int size = 100;

            Console.WriteLine("Running Eigen wrapper test:");
            EigenWrapper.MatrixXd matrixA = new EigenWrapper.MatrixXd(size, size);
            EigenWrapper.MatrixXd matrixB = new EigenWrapper.MatrixXd(size, size);
            EigenWrapper.MatrixXd matrixC;
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                matrixC = matrixA * matrixB;
            }
            watch.Stop();
            Console.WriteLine("Processing took: " + watch.ElapsedMilliseconds);

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
