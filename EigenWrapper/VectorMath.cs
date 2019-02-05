using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace EigenWrapper
{
    public class VectorMath
    {
        //This is just an alias for the same function in the MatrixMath class
        public static double[] MultiplyMatrixVector(double[,] A, double[] B)
        {
            return MatrixMath.MultiplyMatrixVector(A, B);
        }

        public static double[] CrossProduct(double[] A, double[] B)
        {
            IntPtr result;
            int elements = A.Length;

            if (elements != 3)
            {
                throw new ArgumentException("The vectors must be of size 3 to compute a cross product.");
            }

            unsafe
            {
                fixed (double* aPtr = A, bPtr = B)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    IntPtr bIntPtr = new IntPtr(bPtr);
                    result = NativeInterop.RunCrossProduct(aIntPtr, bIntPtr);
                }
            }
            double[] C = new double[elements];
            Marshal.Copy(result, C, 0, elements);
            NativeInterop.FreeMatrix(result);
            return C;
        }

        public static double DotProduct(double[] A, double[] B)
        {
            double result;
            int elements = A.Length;

            unsafe
            {
                fixed (double* aPtr = A, bPtr = B)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    IntPtr bIntPtr = new IntPtr(bPtr);
                    result = NativeInterop.RunDotProduct(aIntPtr, bIntPtr, elements);
                }
            }
            return result;
        }

        public static double[] AddVectors(double[] A, double[] B)
        {
            IntPtr result;
            int elements = A.Length;

            unsafe
            {
                fixed (double* aPtr = A, bPtr = B)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    IntPtr bIntPtr = new IntPtr(bPtr);
                    result = NativeInterop.RunAddVectors(aIntPtr, bIntPtr, elements);
                }
            }
            double[] C = new double[elements];
            Marshal.Copy(result, C, 0, elements);
            NativeInterop.FreeMatrix(result);
            return C;
        }

        public static double[] SubtractVectors(double[] A, double[] B)
        {
            IntPtr result;
            int elements = A.Length;

            unsafe
            {
                fixed (double* aPtr = A, bPtr = B)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    IntPtr bIntPtr = new IntPtr(bPtr);
                    result = NativeInterop.RunSubtractVectors(aIntPtr, bIntPtr, elements);
                }
            }
            double[] C = new double[elements];
            Marshal.Copy(result, C, 0, elements);
            NativeInterop.FreeMatrix(result);
            return C;
        }

        public static double[] NormalizeVector(double[] A)
        {
            IntPtr result;
            int elements = A.Length;

            unsafe
            {
                fixed (double* aPtr = A)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    result = NativeInterop.RunNormalizeVector(aIntPtr, elements);
                }
            }
            double[] B = new double[elements];
            Marshal.Copy(result, B, 0, elements);
            NativeInterop.FreeMatrix(result);
            return B;
        }

        public static double VectorMagnitude(double[] A)
        {
            double result;
            int elements = A.Length;

            unsafe
            {
                fixed (double* aPtr = A)
                {
                    IntPtr aIntPtr = new IntPtr(aPtr);
                    result = NativeInterop.RunVectorMagnitude(aIntPtr, elements);
                }
            }
            return result;
        }
    }
}
