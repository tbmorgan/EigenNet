using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace EigenWrapper
{
    /// <summary>
    /// This is a low level class to do matrix math on column-major formatted 2D arrays.
    /// This class takes care of calling the C++ code for Eigen, and cleaning up after itself.
    /// There is no sanity checking in these functions!  Therefore, it is highly recommended that you used the Matrix class instead.
    /// </summary>
    public class MatrixMath
    {
        public static double[,] MultiplyMatrices(double[,] A, double[,] B)
        {
            IntPtr result;
            int rowsA = A.GetLength(1);
            int columnsA = A.GetLength(0);
            int rowsB = B.GetLength(1);
            int columnsB = B.GetLength(0);

            unsafe
            {
                fixed (double* aPtr = A, bPtr = B)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    IntPtr bIntPtr = new IntPtr(bPtr);
                    result = NativeInterop.RunMultiplyMatrices(aIntPtr, rowsA, columnsA, bIntPtr, rowsB, columnsB);
                }
            }
            //I know it seems redundant to copy it twice like this, but in my experience, it is faster than the alternative
            double[] C = new double[columnsB * rowsA];
            double[,] C2D = new double[columnsB, rowsA];
            Marshal.Copy(result, C, 0, columnsB * rowsA);
            System.Buffer.BlockCopy(C, 0, C2D, 0, sizeof(double) * columnsB * rowsA);
            NativeInterop.FreeMatrix(result);
            return C2D;
        }

        public static double[] MultiplyMatrixVector(double[,] A, double[] B)
        {
            IntPtr result;
            int rows = A.GetLength(1);
            int columns = A.GetLength(0);
            int elements = B.Length;

            unsafe
            {
                fixed (double* aPtr = A, bPtr = B)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    IntPtr bIntPtr = new IntPtr(bPtr);
                    result = NativeInterop.RunMultiplyMatrixVector(aIntPtr, rows, columns, bIntPtr, elements);
                }
            }
            double[] C = new double[rows];
            Marshal.Copy(result, C, 0, rows);
            NativeInterop.FreeMatrix(result);
            return C;
        }

        public static double[,] MultiplyMatrixScalar(double[,] A, double b)
        {
            IntPtr result;
            int rows = A.GetLength(1);
            int columns = A.GetLength(0);

            unsafe
            {
                fixed (double* aPtr = A)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    result = NativeInterop.RunMultiplyMatrixScalar(aIntPtr, rows, columns, b);
                }
            }
            double[] C = new double[columns * rows];
            double[,] C2D = new double[columns, rows];
            Marshal.Copy(result, C, 0, columns * rows);
            System.Buffer.BlockCopy(C, 0, C2D, 0, sizeof(double) * columns * rows);
            NativeInterop.FreeMatrix(result);
            return C2D;
        }

        public static double[,] DivideMatrixScalar(double[,] A, double b)
        {
            IntPtr result;
            int rows = A.GetLength(1);
            int columns = A.GetLength(0);

            unsafe
            {
                fixed (double* aPtr = A)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    result = NativeInterop.RunDivideMatrixScalar(aIntPtr, rows, columns, b);
                }
            }
            double[] C = new double[columns * rows];
            double[,] C2D = new double[columns, rows];
            Marshal.Copy(result, C, 0, columns * rows);
            System.Buffer.BlockCopy(C, 0, C2D, 0, sizeof(double) * columns * rows);
            NativeInterop.FreeMatrix(result);
            return C2D;
        }

        public static double[,] InvertMatrix(double[,] A)
        {
            IntPtr result;
            int rows = A.GetLength(1);
            int columns = A.GetLength(0);

            unsafe
            {
                fixed (double* aPtr = A)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    result = NativeInterop.RunInvertMatrix(aIntPtr, rows, columns);
                }
            }
            double[] B = new double[columns * rows];
            double[,] B2D = new double[columns, rows];
            Marshal.Copy(result, B, 0, columns * rows);
            System.Buffer.BlockCopy(B, 0, B2D, 0, sizeof(double) * columns * rows);
            NativeInterop.FreeMatrix(result);
            return B2D;
        }

        public static double[,] AddMatrices(double[,] A, double[,] B)
        {
            IntPtr result;
            int rows = A.GetLength(1);
            int columns = A.GetLength(0);

            unsafe
            {
                fixed (double* aPtr = A, bPtr = B)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    IntPtr bIntPtr = new IntPtr(bPtr);
                    result = NativeInterop.RunAddMatrices(aIntPtr, bIntPtr, rows, columns);
                }
            }
            double[] C = new double[columns * rows];
            double[,] C2D = new double[columns, rows];
            Marshal.Copy(result, C, 0, columns * rows);
            System.Buffer.BlockCopy(C, 0, C2D, 0, sizeof(double) * columns * rows);
            NativeInterop.FreeMatrix(result);
            return C2D;
        }

        public static double[,] SubtractMatrices(double[,] A, double[,] B)
        {
            IntPtr result;
            int rows = A.GetLength(1);
            int columns = A.GetLength(0);

            unsafe
            {
                fixed (double* aPtr = A, bPtr = B)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    IntPtr bIntPtr = new IntPtr(bPtr);
                    result = NativeInterop.RunSubtractMatrices(aIntPtr, bIntPtr, rows, columns);
                }
            }
            //TODO: There must be a way to remove the second copy step from this code...
            double[] C = new double[columns * rows];
            double[,] C2D = new double[columns, rows];
            Marshal.Copy(result, C, 0, columns * rows);
            System.Buffer.BlockCopy(C, 0, C2D, 0, sizeof(double) * columns * rows);
            NativeInterop.FreeMatrix(result);
            return C2D;
        }

        public static double[,] TransposeMatrix(double[,] A)
        {
            IntPtr result;
            int rows = A.GetLength(1);
            int columns = A.GetLength(0);

            unsafe
            {
                fixed (double* aPtr = A)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    result = NativeInterop.RunTransposeMatrix(aIntPtr, rows, columns);
                }
            }
            double[] B = new double[columns * rows];
            double[,] B2D = new double[rows, columns];
            Marshal.Copy(result, B, 0, columns * rows);
            System.Buffer.BlockCopy(B, 0, B2D, 0, sizeof(double) * columns * rows);
            NativeInterop.FreeMatrix(result);
            return B2D;
        }

        public static double MatrixDeterminant(double[,] A)
        {
            double result;
            int rows = A.GetLength(1);
            int columns = A.GetLength(0);

            unsafe
            {
                fixed (double* aPtr = A)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    result = NativeInterop.RunCalculateDeterminant(aIntPtr, rows, columns);
                }
            }
            return result;
        }

        public static bool HasInverse(double[,] A)
        {
            bool result;
            int rows = A.GetLength(1);
            int columns = A.GetLength(0);

            unsafe
            {
                fixed (double* aPtr = A)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    result = NativeInterop.RunIsInvertible(aIntPtr, rows, columns);
                }
            }
            return result;
        }

        public static double MatrixNorm(double[,] A)
        {
            double result;
            int rows = A.GetLength(1);
            int columns = A.GetLength(0);

            unsafe
            {
                fixed (double* aPtr = A)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    result = NativeInterop.RunCalculateMatrixNorm(aIntPtr, rows, columns);
                }
            }
            return result;
        }
    }
}
