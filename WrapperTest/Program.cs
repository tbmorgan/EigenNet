using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WrapperTest
{
    //Two notes on the testing:
    //1) All Eigen library based methods will be significantly slower in debug mode
    //2) The MatrixMathCS reads the double[,] data transposed from how the Eigen method reads it, therefore, in the correctness tests, all the input and output are transposed
    class Program
    {
        static int matrixSize = 10;
        static int testIterations = 10;
        static bool runMultSpeed = true;
        static bool runInvSpeed = true;
        static bool runCorrectness = true;
        static bool addPauses = true;

        static void Main(string[] args)
        {
            //Create the test matrices
            double[,] A = MatrixMathCS.RandomMatrix(matrixSize, matrixSize, 100, -100);
            double[,] B = MatrixMathCS.RandomMatrix(matrixSize, matrixSize, 100, -100);
            EigenWrapper.Matrix AMatrix = EigenWrapper.Matrix.RandomMatrix(matrixSize, matrixSize, -100, 100);
            EigenWrapper.Matrix BMatrix = EigenWrapper.Matrix.RandomMatrix(matrixSize, matrixSize, -100, 100); 
            EigenWrapper_CppCLI.MatrixXd matrixA = new EigenWrapper_CppCLI.MatrixXd(matrixSize, matrixSize);
            EigenWrapper_CppCLI.MatrixXd matrixB = new EigenWrapper_CppCLI.MatrixXd(matrixSize, matrixSize);

            //Run the multiplication speed tests, if requested
            if (runMultSpeed)
            {
                TestCSMultiplySpeed(A, B);
                TestCppCLIMultiplySpeed(matrixA, matrixB);
                TestEigenMultiplySpeed(A, B);
                TestMatrixMultiplySpeed(AMatrix, BMatrix);
            }
            else
            {
                Console.WriteLine("Skipping multiplication speed tests...");
            }
            ConditionalPause();
            Console.WriteLine();

            //Run the matrix inversion speed tests, if requested
            if (runInvSpeed)
            {
                TestCSInvertSpeed(A);
                //CppCLI inverse hasn't been implemented
                TestEigenInvertSpeed(A);
                TestMatrixInvertSpeed(AMatrix);
            }
            else
            {
                Console.WriteLine("Skipping matrix inversion speed tests...");
            }
            ConditionalPause();
            Console.WriteLine();

            //Run the method correctness tests, if requested
            if (runCorrectness)
            {
                //Test the correctness of the matrix multiplication
                TestMultiplyCorrectness();
                ConditionalPause();
                Console.WriteLine();

                //Test the correctness of the matrix inverse calculations
                TestInverseCorrectness();
                ConditionalPause();
                Console.WriteLine();

                //Test the correctness of the matrix addition calculations
                TestAdditionCorrectness();
                ConditionalPause();
                Console.WriteLine();

                //Test the correctness of the matrix subtraction calculations
                TestSubtractionCorrectness();
                ConditionalPause();
                Console.WriteLine();

                //Test the correctness of the transpose
                TestTransposeCorrectness();
                ConditionalPause();
                Console.WriteLine();

                //Test the correctness of the matrix * scalar calculation
                TestMatrixScalarMultiplyCorrectness();
                ConditionalPause();
                Console.WriteLine();

                //Test the correctness of the matrix / scalar calculation
                TestMatrixScalarDivideCorrectness();
                ConditionalPause();
                Console.WriteLine();

                //Test the correctness of the matrix * vector calculation
                TestMatrixVectorMultiplyCorrectness();
                ConditionalPause();
                Console.WriteLine();

                //Test the correctness of the determinant calculation
                TestMatrixDeterminantCorrectness();
                ConditionalPause();
                Console.WriteLine();

                //Test the correctness of the has inverse calculation
                TestIsInvertableCorrectness();
                ConditionalPause();
                Console.WriteLine();

                //Test the correctness of the matrix norm calculation
                TestMatrixNormCorrectness();
                ConditionalPause();
                Console.WriteLine();

                //Test the correctness of the vector cross product calculation
                TestVectorCrossProduct();
                ConditionalPause();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Skipping correctness tests...");
            }

            Console.WriteLine();
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        //Matrix multiplication speed test functions
        static void TestCppCLIMultiplySpeed(EigenWrapper_CppCLI.MatrixXd A, EigenWrapper_CppCLI.MatrixXd B)
        {
            EigenWrapper_CppCLI.MatrixXd C;
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < testIterations; i++)
            {
                C = A * B;
            }
            watch.Stop();
            Console.WriteLine("C++/CLI Eigen matrix multiply test took: " + watch.ElapsedMilliseconds + " ms.");
        }
        static void TestCSMultiplySpeed(double[,] A, double[,] B)
        {
            double[,] C = new double[matrixSize, matrixSize];
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < testIterations; i++)
            {
                C = MatrixMathCS.MultiplyMatrices(A, B);
            }
            watch.Stop();
            Console.WriteLine("C# multiply test took: " + watch.ElapsedMilliseconds + " ms.");
        }
        static void TestEigenMultiplySpeed(double[,] A, double[,] B)
        {
            double[,] C = new double[matrixSize, matrixSize];
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < testIterations; i++)
            {
                C = EigenWrapper.MatrixMath.MultiplyMatrices(A, B);
            }
            watch.Stop();
            Console.WriteLine("P/Invoked Eigen 2D double array multiply test took: " + watch.ElapsedMilliseconds + " ms.");
        }
        static void TestMatrixMultiplySpeed(EigenWrapper.Matrix A, EigenWrapper.Matrix B)
        {
            EigenWrapper.Matrix C;
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < testIterations; i++)
            {
                C = A * B;
            }
            watch.Stop();
            Console.WriteLine("P/Invoked Eigen matrix class multiply test took: " + watch.ElapsedMilliseconds + " ms.");
        }

        //Matrix inversion speed test functions
        static void TestCSInvertSpeed(double[,] A)
        {
            double[,] C = new double[matrixSize, matrixSize];
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < testIterations; i++)
            {
                C = MatrixMathCS.InverseMatrix(A);
            }
            watch.Stop();
            Console.WriteLine("C# inverse test took: " + watch.ElapsedMilliseconds + " ms.");
        }
        static void TestEigenInvertSpeed(double[,] A)
        {
            double[,] C = new double[matrixSize, matrixSize];
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < testIterations; i++)
            {
                C = EigenWrapper.MatrixMath.InvertMatrix(A);
            }
            watch.Stop();
            Console.WriteLine("P/Invoked Eigen 2D double array inverse test took: " + watch.ElapsedMilliseconds + " ms.");
        }
        static void TestMatrixInvertSpeed(EigenWrapper.Matrix A)
        {
            EigenWrapper.Matrix C;
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < testIterations; i++)
            {
                C = A.Inverse();
            }
            watch.Stop();
            Console.WriteLine("P/Invoked Eigen matrix class inverse test took: " + watch.ElapsedMilliseconds + " ms.");
        }

        //Correctness test functions (these are not rigorous tests, just a spot check!)
        static void TestMultiplyCorrectness()
        {
            double[,] ATest = { { 1, 3, 5, 7 }, { 2, 4, 6, 8 } };
            double[,] BTest = { { 1, 4 }, { 2, 5 }, { 3, 6 } };
            EigenWrapper.Matrix AMatTest = new EigenWrapper.Matrix(4, 2);
            EigenWrapper.Matrix BMatTest = new EigenWrapper.Matrix(2, 3);

            //Set the values for the matrix validation tests
            AMatTest[0, 0] = 1;
            AMatTest[0, 1] = 2;
            AMatTest[1, 0] = 3;
            AMatTest[1, 1] = 4;
            AMatTest[2, 0] = 5;
            AMatTest[2, 1] = 6;
            AMatTest[3, 0] = 7;
            AMatTest[3, 1] = 8;
            BMatTest[0, 0] = 1;
            BMatTest[0, 1] = 2;
            BMatTest[0, 2] = 3;
            BMatTest[1, 0] = 4;
            BMatTest[1, 1] = 5;
            BMatTest[1, 2] = 6;

            double[,] C = new double[BTest.GetLength(0), ATest.GetLength(1)];
            double[,] D = new double[BTest.GetLength(0), ATest.GetLength(1)];
            EigenWrapper.Matrix CMat;

            //Testing the C# multiplcation accuracy
            Console.WriteLine("C# multiplication accuracy test:");
            Console.WriteLine("The input matrices are:");
            PrintMatrix(ATest);
            Console.WriteLine("and:");
            PrintMatrix(BTest);
            Console.WriteLine("The result is:");
            double[,] Atrans = MatrixMathCS.Transpose(ATest);
            double[,] Btrans = MatrixMathCS.Transpose(BTest);
            C = MatrixMathCS.MultiplyMatrices(Atrans, Btrans);
            double[,] Ctrans = MatrixMathCS.Transpose(C);
            PrintMatrix(Ctrans);
            Console.WriteLine();

            //Test the Eigen multiplication accuracy
            Console.WriteLine("P/Invoke Eigen 2D double array multiplication correctness test:");
            Console.WriteLine("The input matrices are:");
            PrintMatrix(ATest);
            Console.WriteLine("and:");
            PrintMatrix(BTest);
            Console.WriteLine("The result is:");
            D = EigenWrapper.MatrixMath.MultiplyMatrices(ATest, BTest);
            PrintMatrix(D);
            Console.WriteLine();

            //Test the Matrix class multiplication accuracy
            Console.WriteLine("P/Invoke Eigen matrix class multiplication correctness test:");
            Console.WriteLine("The input matrices are:");
            PrintMatrix(AMatTest);
            Console.WriteLine("and:");
            PrintMatrix(BMatTest);
            Console.WriteLine("The result is:");
            CMat = AMatTest * BMatTest;
            PrintMatrix(CMat);
        }
        static void TestInverseCorrectness()
        {
            double[,] A = { { 1, -2, -3, -4 }, { 2, -5, -6, -7 }, { 3, 7, -8, -9 }, { 4, 8, 12, 16 } };
            EigenWrapper.Matrix AMat = new EigenWrapper.Matrix(4, 4);
            AMat[0, 0] = 1;
            AMat[0, 1] = 2;
            AMat[0, 2] = 3;
            AMat[0, 3] = 4;
            AMat[1, 0] = -2;
            AMat[1, 1] = -5;
            AMat[1, 2] = 7;
            AMat[1, 3] = 8;
            AMat[2, 0] = -3;
            AMat[2, 1] = -6;
            AMat[2, 2] = -8;
            AMat[2, 3] = 12;
            AMat[3, 0] = -4;
            AMat[3, 1] = -7;
            AMat[3, 2] = -9;
            AMat[3, 3] = 16;

            //Test the C# inverse correctness
            Console.WriteLine("C# matrix inverse...");
            Console.WriteLine("The input matrix is:");
            PrintMatrix(A);
            Console.WriteLine("The inverse matrix is:");
            double[,] Atrans = MatrixMathCS.Transpose(A);
            double[,] C = MatrixMathCS.InverseMatrix(Atrans);
            double[,] Ctrans = MatrixMathCS.Transpose(C);
            PrintMatrix(Ctrans);
            Console.WriteLine();

            //Test the Eigen matrix inverse correctness
            Console.WriteLine("P/Invoke Eigen 2D double array matrix inverse...");
            Console.WriteLine("The input matrix is:");
            PrintMatrix(A);
            Console.WriteLine("The inverse matrix is:");
            double[,] D = EigenWrapper.MatrixMath.InvertMatrix(A);
            PrintMatrix(D);
            Console.WriteLine();

            //Test the matrix class inverse correctness
            Console.WriteLine("P/Invoke Eigen matrix class inverse...");
            Console.WriteLine("The input matrix is:");
            PrintMatrix(AMat);
            Console.WriteLine("The inverse matrix is:");
            EigenWrapper.Matrix CMat = AMat.Inverse();
            PrintMatrix(CMat);
        }
        static void TestAdditionCorrectness()
        {
            double[,] ATest = { { 1, 4, 7 }, { 2, 5, 8 }, { 3, 6, 9 } };
            double[,] BTest = { { 1, 4, 7 }, { 2, 5, 8 }, { 3, 6, 9 } };
            EigenWrapper.Matrix AMatTest = new EigenWrapper.Matrix(3, 3);
            EigenWrapper.Matrix BMatTest = new EigenWrapper.Matrix(3, 3);

            //Set the values for the matrix validation tests
            AMatTest[0, 0] = 1;
            AMatTest[0, 1] = 2;
            AMatTest[0, 2] = 3;
            AMatTest[1, 0] = 4;
            AMatTest[1, 1] = 5;
            AMatTest[1, 2] = 6;
            AMatTest[2, 0] = 7;
            AMatTest[2, 1] = 8;
            AMatTest[2, 2] = 9;

            BMatTest[0, 0] = 1;
            BMatTest[0, 1] = 2;
            BMatTest[0, 2] = 3;
            BMatTest[1, 0] = 4;
            BMatTest[1, 1] = 5;
            BMatTest[1, 2] = 6;
            BMatTest[2, 0] = 7;
            BMatTest[2, 1] = 8;
            BMatTest[2, 2] = 9;

            //Testing the C# addition accuracy
            Console.WriteLine("C# addition correctness test:");
            Console.WriteLine("The input matrices are:");
            PrintMatrix(ATest);
            Console.WriteLine("and:");
            PrintMatrix(BTest);
            Console.WriteLine("The result is:");
            double[,] Atrans = MatrixMathCS.Transpose(ATest);
            double[,] Btrans = MatrixMathCS.Transpose(BTest);
            double[,] C = MatrixMathCS.AddMatrices(Atrans, Btrans);
            double[,] Ctrans = MatrixMathCS.Transpose(C);
            PrintMatrix(Ctrans);
            Console.WriteLine();

            //Test the Eigen addition accuracy
            Console.WriteLine("P/Invoke Eigen 2D double array addition correctness test:");
            Console.WriteLine("The input matrices are:");
            PrintMatrix(ATest);
            Console.WriteLine("and:");
            PrintMatrix(BTest);
            Console.WriteLine("The result is:");
            double[,] D = EigenWrapper.MatrixMath.AddMatrices(ATest, BTest);
            PrintMatrix(D);
            Console.WriteLine();

            //Test the Matrix class addition accuracy
            Console.WriteLine("P/Invoke Eigen matrix class addition correctness test:");
            Console.WriteLine("The input matrices are:");
            PrintMatrix(AMatTest);
            Console.WriteLine("and:");
            PrintMatrix(BMatTest);
            Console.WriteLine("The result is:");
            EigenWrapper.Matrix CMat = AMatTest + BMatTest;
            PrintMatrix(CMat);
        }
        static void TestSubtractionCorrectness()
        {
            double[,] ATest = { { 1, 4, 7 }, { 2, 5, 8 }, { 3, 6, 9 } };
            double[,] BTest = { { 10, 40, 70 }, { 20, 50, 80 }, { 30, 60, 90 } };
            EigenWrapper.Matrix AMatTest = new EigenWrapper.Matrix(3, 3);
            EigenWrapper.Matrix BMatTest = new EigenWrapper.Matrix(3, 3);

            //Set the values for the matrix validation tests
            AMatTest[0, 0] = 1;
            AMatTest[0, 1] = 2;
            AMatTest[0, 2] = 3;
            AMatTest[1, 0] = 4;
            AMatTest[1, 1] = 5;
            AMatTest[1, 2] = 6;
            AMatTest[2, 0] = 7;
            AMatTest[2, 1] = 8;
            AMatTest[2, 2] = 9;

            BMatTest[0, 0] = 10;
            BMatTest[0, 1] = 20;
            BMatTest[0, 2] = 30;
            BMatTest[1, 0] = 40;
            BMatTest[1, 1] = 50;
            BMatTest[1, 2] = 60;
            BMatTest[2, 0] = 70;
            BMatTest[2, 1] = 80;
            BMatTest[2, 2] = 90;

            //Testing the C# subtraction accuracy
            Console.WriteLine("C# subtraction correctness test:");
            Console.WriteLine("The input matrices are:");
            PrintMatrix(ATest);
            Console.WriteLine("and:");
            PrintMatrix(BTest);
            Console.WriteLine("The result is:");
            double[,] Atrans = MatrixMathCS.Transpose(ATest);
            double[,] Btrans = MatrixMathCS.Transpose(BTest);
            double[,] C = MatrixMathCS.SubtractMatrices(Atrans, Btrans);
            double[,] Ctrans = MatrixMathCS.Transpose(C);
            PrintMatrix(Ctrans);
            Console.WriteLine();

            //Test the Eigen subtraction accuracy
            Console.WriteLine("P/Invoke Eigen 2D double array subtraction correctness test:");
            Console.WriteLine("The input matrices are:");
            PrintMatrix(ATest);
            Console.WriteLine("and:");
            PrintMatrix(BTest);
            Console.WriteLine("The result is:");
            double[,] D = EigenWrapper.MatrixMath.SubtractMatrices(ATest, BTest);
            PrintMatrix(D);
            Console.WriteLine();

            //Test the Matrix class subtraction accuracy
            Console.WriteLine("P/Invoke Eigen matrix class subtraction correctness test:");
            Console.WriteLine("The input matrices are:");
            PrintMatrix(AMatTest);
            Console.WriteLine("and:");
            PrintMatrix(BMatTest);
            Console.WriteLine("The result is:");
            EigenWrapper.Matrix CMat = AMatTest - BMatTest;
            PrintMatrix(CMat);
        }
        static void TestTransposeCorrectness()
        {
            double[,] ATest = { { 1, 4 }, { 2, 5 }, { 3, 6 } };
            EigenWrapper.Matrix AMatTest = new EigenWrapper.Matrix(2, 3);

            //Set the values for the matrix validation tests
            AMatTest[0, 0] = 1;
            AMatTest[0, 1] = 2;
            AMatTest[0, 2] = 3;
            AMatTest[1, 0] = 4;
            AMatTest[1, 1] = 5;
            AMatTest[1, 2] = 6;

            //Testing the C# transpose accuracy
            Console.WriteLine("C# transpose correctness test:");
            Console.WriteLine("The input matrix is:");
            PrintMatrix(ATest);
            Console.WriteLine("The result is:");
            double[,] Atrans = MatrixMathCS.Transpose(ATest);
            double[,] C = MatrixMathCS.Transpose(ATest);
            double[,] Ctrans = MatrixMathCS.Transpose(C);
            PrintMatrix(C);
            Console.WriteLine();

            //Test the Eigen transpose accuracy
            Console.WriteLine("P/Invoke Eigen 2D double array transpose correctness test:");
            Console.WriteLine("The input matrix is:");
            PrintMatrix(ATest);
            Console.WriteLine("The result is:");
            double[,] D = EigenWrapper.MatrixMath.TransposeMatrix(ATest);
            PrintMatrix(D);
            Console.WriteLine();

            //Test the Matrix class transpose accuracy
            Console.WriteLine("P/Invoke Eigen matrix class transpose correctness test:");
            Console.WriteLine("The input matrix is:");
            PrintMatrix(AMatTest);
            Console.WriteLine("The result is:");
            EigenWrapper.Matrix CMat = AMatTest.Transpose();
            PrintMatrix(CMat);
        }
        static void TestMatrixScalarMultiplyCorrectness()
        {
            double[,] ATest = { { 1, 4 }, { 2, 5 }, { 3, 6 } };
            double b = 3;
            EigenWrapper.Matrix AMatTest = new EigenWrapper.Matrix(2, 3);

            //Set the values for the matrix validation tests
            AMatTest[0, 0] = 1;
            AMatTest[0, 1] = 2;
            AMatTest[0, 2] = 3;
            AMatTest[1, 0] = 4;
            AMatTest[1, 1] = 5;
            AMatTest[1, 2] = 6;

            //Testing the C# multiplcation accuracy
            Console.WriteLine("C# matrix * scalar correctness test:");
            Console.WriteLine("The input matrix is:");
            PrintMatrix(ATest);
            Console.WriteLine("The input scalar is:");
            Console.WriteLine(b);
            Console.WriteLine("The result is:");
            double[,] Atrans = MatrixMathCS.Transpose(ATest);
            double[,] C = MatrixMathCS.ScalarTimesMatrix(b, Atrans);
            double[,] Ctrans = MatrixMathCS.Transpose(C);
            PrintMatrix(Ctrans);
            Console.WriteLine();

            //Test the Eigen multiplication accuracy
            Console.WriteLine("P/Invoke Eigen 2D double array matrix * scalar correctness test:");
            Console.WriteLine("The input matrix is:");
            PrintMatrix(ATest);
            Console.WriteLine("The input scalar is:");
            Console.WriteLine(b);
            Console.WriteLine("The result is:");
            double[,] D = EigenWrapper.MatrixMath.MultiplyMatrixScalar(ATest, b);
            PrintMatrix(D);
            Console.WriteLine();

            //Test the Matrix class multiplication accuracy
            Console.WriteLine("P/Invoke Eigen matrix class matrix * scalar correctness test:");
            Console.WriteLine("The input matrix is:");
            PrintMatrix(AMatTest);
            Console.WriteLine("The input scalar is:");
            Console.WriteLine(b);
            Console.WriteLine("The b*A result is:");
            EigenWrapper.Matrix CMat = b * AMatTest;
            PrintMatrix(CMat);
            Console.WriteLine("The A*b result is:");
            EigenWrapper.Matrix DMat = AMatTest * b;
            PrintMatrix(DMat);
        }
        static void TestMatrixVectorMultiplyCorrectness()
        {
            double[,] ATest = { { 1, 4 }, { 2, 5 }, { 3, 6 } };
            double[] BTest = { 1, 2, 3 };
            EigenWrapper.Matrix AMatTest = new EigenWrapper.Matrix(2, 3);
            EigenWrapper.Vector BVecTest = new EigenWrapper.Vector(3);

            //Set the values for the matrix validation tests
            AMatTest[0, 0] = 1;
            AMatTest[0, 1] = 2;
            AMatTest[0, 2] = 3;
            AMatTest[1, 0] = 4;
            AMatTest[1, 1] = 5;
            AMatTest[1, 2] = 6;
            BVecTest[0] = 1;
            BVecTest[1] = 2;
            BVecTest[2] = 3;

            //This doesn't test well because of the transposed interpretation of a 2D double array in the C# version
            //Testing the C# multiplcation accuracy
            //Console.WriteLine("C# matrix * vector correctness test:");
            //Console.WriteLine("The input matrix is:");
            //PrintMatrix(ATest);
            //Console.WriteLine("The input scalar is:");
            //PrintVector(BTest);
            //Console.WriteLine("The result is:");
            //double[,] Atrans = MatrixMathCS.Transpose(ATest);
            //double[] C = MatrixMathCS.VectorTimesMatrix(BTest, Atrans);
            //PrintVector(C);
            //Console.WriteLine();

            //Test the Eigen multiplication accuracy
            Console.WriteLine("P/Invoke Eigen 2D double array matrix * vector correctness test:");
            Console.WriteLine("The input matrix is:");
            PrintMatrix(ATest);
            Console.WriteLine("The input scalar is:");
            PrintVector(BTest);
            Console.WriteLine("The result is:");
            double[] D = EigenWrapper.MatrixMath.MultiplyMatrixVector(ATest, BTest);
            PrintVector(D);
            Console.WriteLine();

            //Test the Matrix class multiplication accuracy
            Console.WriteLine("P/Invoke Eigen matrix class matrix * vector correctness test:");
            Console.WriteLine("The input matrix is:");
            PrintMatrix(AMatTest);
            Console.WriteLine("The input scalar is:");
            PrintVector(BVecTest);
            Console.WriteLine("The result is:");
            EigenWrapper.Vector CVec = AMatTest * BVecTest;
            PrintVector(CVec);
        }
        static void TestMatrixScalarDivideCorrectness()
        {
            double[,] ATest = { { 1, 4 }, { 2, 5 }, { 3, 6 } };
            double b = 2;
            EigenWrapper.Matrix AMatTest = new EigenWrapper.Matrix(2, 3);

            //Set the values for the matrix validation tests
            AMatTest[0, 0] = 1;
            AMatTest[0, 1] = 2;
            AMatTest[0, 2] = 3;
            AMatTest[1, 0] = 4;
            AMatTest[1, 1] = 5;
            AMatTest[1, 2] = 6;

            //Testing the C# divide accuracy
            Console.WriteLine("C# matrix / scalar correctness test:");
            Console.WriteLine("The input matrix is:");
            PrintMatrix(ATest);
            Console.WriteLine("The input scalar is:");
            Console.WriteLine(b);
            Console.WriteLine("The result is:");
            double[,] Atrans = MatrixMathCS.Transpose(ATest);
            double[,] C = MatrixMathCS.DivideMatrixbyScalar(Atrans, b);
            double[,] Ctrans = MatrixMathCS.Transpose(C);
            PrintMatrix(Ctrans);
            Console.WriteLine();

            //Test the Eigen divide accuracy
            Console.WriteLine("P/Invoke Eigen 2D double array matrix / scalar correctness test:");
            Console.WriteLine("The input matrix is:");
            PrintMatrix(ATest);
            Console.WriteLine("The input scalar is:");
            Console.WriteLine(b);
            Console.WriteLine("The result is:");
            double[,] D = EigenWrapper.MatrixMath.DivideMatrixScalar(ATest, b);
            PrintMatrix(D);
            Console.WriteLine();

            //Test the Matrix class divide accuracy
            Console.WriteLine("P/Invoke Eigen matrix class matrix / scalar correctness test:");
            Console.WriteLine("The input matrix is:");
            PrintMatrix(AMatTest);
            Console.WriteLine("The input scalar is:");
            Console.WriteLine(b);
            Console.WriteLine("The result is:");
            EigenWrapper.Matrix CMat = AMatTest / b;
            PrintMatrix(CMat);
        }
        static void TestMatrixDeterminantCorrectness()
        {
            double[,] A = { { 1, 4, 7 }, { 2, 5, 8 }, { 3, 6, 9 } };  //Det = 0
            double[,] B = { { 1, 4, 3 }, { 2, 5, 8 }, { 3, 6, 9 } };  //Det = 12

            EigenWrapper.Matrix AMat = new EigenWrapper.Matrix(3, 3);
            EigenWrapper.Matrix BMat = new EigenWrapper.Matrix(3, 3);

            //Set the values for the matrix validation tests
            AMat[0, 0] = 1;
            AMat[0, 1] = 2;
            AMat[0, 2] = 3;
            AMat[1, 0] = 4;
            AMat[1, 1] = 5;
            AMat[1, 2] = 6;
            AMat[2, 0] = 7;
            AMat[2, 1] = 8;
            AMat[2, 2] = 9;

            BMat[0, 0] = 1;
            BMat[0, 1] = 2;
            BMat[0, 2] = 3;
            BMat[1, 0] = 4;
            BMat[1, 1] = 5;
            BMat[1, 2] = 6;
            BMat[2, 0] = 3;
            BMat[2, 1] = 8;
            BMat[2, 2] = 9;

            //Test the C# determinant accuracy
            Console.WriteLine("C# determinant correctness test:");
            Console.WriteLine("The first input matrix is:");
            PrintMatrix(A);
            double[,] Atrans = MatrixMathCS.Transpose(A);
            Console.WriteLine("The determinant is {0}", MatrixMathCS.Determinant(Atrans));
            Console.WriteLine("The second input matrix is:");
            PrintMatrix(B);
            double[,] Btrans = MatrixMathCS.Transpose(B);
            Console.WriteLine("The determinant is {0}", MatrixMathCS.Determinant(Btrans));
            Console.WriteLine();

            //Test the Eigen determinant accuracy
            Console.WriteLine("P/Invoke Eigen 2D double array determinant correctness test:");
            Console.WriteLine("The first input matrix is:");
            PrintMatrix(A);
            Console.WriteLine("The determinant is {0}", EigenWrapper.MatrixMath.MatrixDeterminant(A));
            Console.WriteLine("The second input matrix is:");
            PrintMatrix(B);
            Console.WriteLine("The determinant is {0}", EigenWrapper.MatrixMath.MatrixDeterminant(B));
            Console.WriteLine();

            //Test the Matrix class determinant accuracy
            Console.WriteLine("P/Invoke Eigen matrix class has inverse correctness test:");
            Console.WriteLine("The first input matrix is:");
            PrintMatrix(AMat);
            Console.WriteLine("The determinant is {0}", AMat.Determinant());
            Console.WriteLine("The second input matrix is:");
            PrintMatrix(BMat);
            Console.WriteLine("The determinant is {0}", EigenWrapper.Matrix.Determinant(BMat));
        }
        static void TestIsInvertableCorrectness()
        {
            double[,] A = { { 1, 4 }, { 2, 5 }, { 3, 6 } };
            double[,] B = { { 1, 4, 7 }, { 2, 5, 8 }, { 3, 6, 9 } };  //Det = 0
            double[,] C = { { 1, 4, 3 }, { 2, 5, 8 }, { 3, 6, 9 } };  //Det = 12

            EigenWrapper.Matrix AMat = new EigenWrapper.Matrix(2, 3);
            EigenWrapper.Matrix BMat = new EigenWrapper.Matrix(3, 3);
            EigenWrapper.Matrix CMat = new EigenWrapper.Matrix(3, 3);

            //Set the values for the matrix validation tests
            AMat[0, 0] = 1;
            AMat[0, 1] = 2;
            AMat[0, 2] = 3;
            AMat[1, 0] = 4;
            AMat[1, 1] = 5;
            AMat[1, 2] = 6;

            BMat[0, 0] = 1;
            BMat[0, 1] = 2;
            BMat[0, 2] = 3;
            BMat[1, 0] = 4;
            BMat[1, 1] = 5;
            BMat[1, 2] = 6;
            BMat[2, 0] = 7;
            BMat[2, 1] = 8;
            BMat[2, 2] = 9;

            CMat[0, 0] = 1;
            CMat[0, 1] = 2;
            CMat[0, 2] = 3;
            CMat[1, 0] = 4;
            CMat[1, 1] = 5;
            CMat[1, 2] = 6;
            CMat[2, 0] = 3;
            CMat[2, 1] = 8;
            CMat[2, 2] = 9;

            //Test the Eigen multiplication accuracy
            Console.WriteLine("P/Invoke Eigen 2D double array has inverse correctness test:");
            Console.WriteLine("The first input matrix is:");
            PrintMatrix(A);
            Console.WriteLine("Has inverse? {0}", EigenWrapper.MatrixMath.HasInverse(A));
            Console.WriteLine("The second input matrix is:");
            PrintMatrix(B);
            Console.WriteLine("Has inverse? {0}", EigenWrapper.MatrixMath.HasInverse(B));
            Console.WriteLine("The third input matrix is:");
            PrintMatrix(C);
            Console.WriteLine("Has inverse? {0}", EigenWrapper.MatrixMath.HasInverse(C));
            Console.WriteLine();

            //Test the Matrix class multiplication accuracy
            Console.WriteLine("P/Invoke Eigen matrix class has inverse correctness test:");
            Console.WriteLine("The first input matrix is:");
            PrintMatrix(AMat);
            Console.WriteLine("Has inverse? {0}", AMat.HasInverse);
            Console.WriteLine("The second input matrix is:");
            PrintMatrix(BMat);
            Console.WriteLine("Has inverse? {0}", BMat.HasInverse);
            Console.WriteLine("The third input matrix is:");
            PrintMatrix(CMat);
            Console.WriteLine("Has inverse? {0}", CMat.HasInverse);
        }
        static void TestVectorCrossProduct()
        {
            double[] a = { 1, 0, 0 };
            double[] b = { 0, 1, 0 };

            EigenWrapper.Vector aVec = new EigenWrapper.Vector(3);
            EigenWrapper.Vector bVec = new EigenWrapper.Vector(3);
            aVec[0] = 1;
            aVec[1] = 0;
            aVec[2] = 0;
            bVec[0] = 0;
            bVec[1] = 1;
            bVec[2] = 0;

            //Test the Vector class cross product
            Console.WriteLine("C# vector cross product correctness test:");
            Console.WriteLine("The first input vector is:");
            PrintVector(a);
            Console.WriteLine("The second input vector is:");
            PrintVector(b);
            Console.WriteLine("The cross product of a x b is:");
            PrintVector(MatrixMathCS.CrossProduct(a, b));
            Console.WriteLine();

            //Test the Eigen cross product
            Console.WriteLine("P/Invoke Eigen double array cross product correctness test:");
            Console.WriteLine("The first input vector is:");
            PrintVector(a);
            Console.WriteLine("The second input vector is:");
            PrintVector(b);
            Console.WriteLine("The cross product of a x b is:");
            PrintVector(EigenWrapper.VectorMath.CrossProduct(a, b));
            Console.WriteLine();

            //Test the Vector class cross product
            Console.WriteLine("P/Invoke Eigen matrix class cross product correctness test:");
            Console.WriteLine("The first input vector is:");
            PrintVector(aVec);
            Console.WriteLine("The second input vector is:");
            PrintVector(bVec);
            Console.WriteLine("The cross product of a x b is:");
            PrintVector(aVec.CrossProduct(bVec));
        }
        static void TestMatrixNormCorrectness()
        {
            double[,] A = { { 51, 634, 70 }, { 2, 57, 8 }, { 53, 63, 91 } };  //Det = 0

            EigenWrapper.Matrix AMat = new EigenWrapper.Matrix(3, 3);

            //Set the values for the matrix validation tests
            AMat[0, 0] = 51;
            AMat[0, 1] = 2;
            AMat[0, 2] = 53;
            AMat[1, 0] = 634;
            AMat[1, 1] = 57;
            AMat[1, 2] = 63;
            AMat[2, 0] = 70;
            AMat[2, 1] = 8;
            AMat[2, 2] = 91;

            //Test the C# matrix norm
            Console.WriteLine("C# matrix norm correctness test:");
            Console.WriteLine("The test matrix A is:");
            PrintMatrix(A);
            Console.WriteLine("The matrix norm of A is:");
            double[,] Atrans = MatrixMathCS.Transpose(A);
            Console.WriteLine(MatrixMathCS.MatrixNorm(Atrans).ToString());
            Console.WriteLine();

            //Test the Eigen matrix norm
            Console.WriteLine("P/Invoke Eigen 2D double array matrix norm correctness test:");
            Console.WriteLine("The test matrix A is:");
            PrintMatrix(A);
            Console.WriteLine("The matrix norm of A is:");
            Console.WriteLine(EigenWrapper.MatrixMath.MatrixNorm(A));
            Console.WriteLine();

            //Test the Matrix class norm
            Console.WriteLine("P/Invoke Eigen matrix class matrix norm correctness test:");
            Console.WriteLine("The test matrix A is:");
            PrintMatrix(AMat);
            Console.WriteLine("The matrix norm of A is:");
            Console.WriteLine(AMat.Norm().ToString());
        }

        //Helper functions
        static void PrintMatrix(double[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    Console.Write(matrix[j, i].ToString());
                    if (j < matrix.GetLength(0) - 1)
                    {
                        Console.Write(", ");
                    }
                }
                Console.Write("\r\n");
            }
        }
        static void PrintMatrix(EigenWrapper.Matrix matrix)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    Console.Write(matrix[i, j].ToString());
                    if (j < matrix.Columns - 1)
                    {
                        Console.Write(", ");
                    }
                }
                Console.Write("\r\n");
            }
        }
        static void PrintVector(double[] vector)
        {
            Console.Write("{");
            for (int i = 0; i < vector.Length; i++)
            {
                Console.Write(vector[i]);
                if (i < vector.Length - 1)
                {
                    Console.Write(", ");
                }
            }
            Console.Write("}\r\n");
        }
        static void PrintVector(EigenWrapper.Vector vector)
        {
            Console.Write("{");
            for (int i = 0; i < vector.Length; i++)
            {
                Console.Write(vector[i]);
                if (i < vector.Length - 1)
                {
                    Console.Write(", ");
                }
            }
            Console.Write("}\r\n");
        }
        static void ConditionalPause()
        {
            if (addPauses)
            {
                Console.Write("Press enter to continue...");
                Console.ReadLine();
            }
        }
    }
}
