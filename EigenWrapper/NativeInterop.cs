using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace EigenWrapper
{
    internal class NativeInterop
    {
        //NOTE: This assumes a column vector
        [DllImport("NativeEigenWrapper")]
        internal static extern IntPtr RunMultiplyMatrices(IntPtr A, int ARows, int AColumns, IntPtr B, int BRows, int BColumns);
        [DllImport("NativeEigenWrapper")]
        internal static extern IntPtr RunMultiplyMatrixVector(IntPtr A, int ARows, int AColumns, IntPtr B, int BElements);
        [DllImport("NativeEigenWrapper")]
        internal static extern IntPtr RunMultiplyMatrixScalar(IntPtr A, int rows, int columns, double b);
        [DllImport("NativeEigenWrapper")]
        internal static extern IntPtr RunDivideMatrixScalar(IntPtr A, int rows, int columns, double b);
        [DllImport("NativeEigenWrapper")]
        internal static extern IntPtr RunInvertMatrix(IntPtr A, int rows, int columns);
        [DllImport("NativeEigenWrapper")]
        internal static extern IntPtr RunAddMatrices(IntPtr A, IntPtr B, int rows, int columns);
        [DllImport("NativeEigenWrapper")]
        internal static extern IntPtr RunSubtractMatrices(IntPtr A, IntPtr B, int rows, int columns);
        [DllImport("NativeEigenWrapper")]
        internal static extern IntPtr RunTransposeMatrix(IntPtr A, int rows, int columns);
        [DllImport("NativeEigenWrapper")]
        internal static extern double RunCalculateDeterminant(IntPtr A, int rows, int columns);
        [DllImport("NativeEigenWrapper")]
        internal static extern bool RunIsInvertible(IntPtr A, int rows, int columns);
        [DllImport("NativeEigenWrapper")]
        internal static extern double RunCalculateMatrixNorm(IntPtr A, int rows, int columns);
        [DllImport("NativeEigenWrapper")]
        internal static extern IntPtr RunCrossProduct(IntPtr A, IntPtr B);
        [DllImport("NativeEigenWrapper")]
        internal static extern double RunDotProduct(IntPtr A, IntPtr B, int elements);
        [DllImport("NativeEigenWrapper")]
        internal static extern IntPtr RunAddVectors(IntPtr A, IntPtr B, int elements);
        [DllImport("NativeEigenWrapper")]
        internal static extern IntPtr RunSubtractVectors(IntPtr A, IntPtr B, int elements);
        [DllImport("NativeEigenWrapper")]
        internal static extern IntPtr RunNormalizeVector(IntPtr A, int elements);
        [DllImport("NativeEigenWrapper")]
        internal static extern double RunVectorMagnitude(IntPtr A, int elements);
        [DllImport("NativeEigenWrapper")]
        internal static extern void FreeMatrix(IntPtr A);
    }
}
