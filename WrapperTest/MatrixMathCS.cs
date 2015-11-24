using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrapperTest
{
    static class MatrixMathCS
    {
        internal static double[,] RandomMatrix(int rows, int cols, double maxVal, double minVal)
        {
            Random rand = new Random();
            double[,] result = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = (maxVal - minVal) * rand.NextDouble() + minVal;
                }
            }
            return result;
        }

        //Note: identity matrices must be square
        internal static double[,] IdentMatrix(int size)
        {
            double[,] result = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                result[i, i] = 1.0;
            }
            return result;
        }

        internal static double[,] CopyMatrix(double[,] matrix)
        {
            double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
            Array.Copy(matrix, result, matrix.Length);
            return result;
        }

        
        internal static double[,] MultiplyMatrices(double[,] A, double[,] B)
        {
            int aHeight = A.GetLength(0);
            int aWidth = A.GetLength(1);
            int bHeight = B.GetLength(0);
            int bWidth = B.GetLength(1);

            if (aWidth != bHeight)
            {
                throw new ArgumentOutOfRangeException("A or B", "The number of columns of matrix A must equal the number of rows of matrix B");
            }

            double[,] result = new double[aHeight, bWidth];

            for (int i = 0; i < aHeight; i++)
            {
                for (int j = 0; j < bWidth; j++)
                {
                    double tempResult = 0.0;
                    for (int k = 0; k < aWidth; k++)
                    {
                        tempResult += A[i, k] * B[k, j];
                    }
                    result[i, j] = tempResult;
                }
            }

            return result;
        }

        /// <summary>
        /// Decomposes a matrix using Doolitle LUP decomposition
        /// </summary>
        /// <param name="matrix">The matrix to decompose (must be square).</param>
        /// <param name="pVect">The row permutations.</param>
        /// <param name="toggle">Tracks row swaps and returns even or odd (+1 or -1).</param>
        /// <returns>The decomposed matrix.</returns>
        internal static double[,] DecomposeMatrix(double[,] matrix, out int[] pVect, out int toggle)
        {
            //Matrix must be square
            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                throw new ArgumentException("Only square matrices may be decomposed!", "matrix");
            }

            //Doolittle LUP composition
            int n = matrix.GetLength(0);
            double[,] result = CopyMatrix(matrix);

            //Initialize the permutation vector
            pVect = new int[n];
            for (int i = 0; i < n; i++)
            {
                pVect[i] = i;
            }

            //Initialize the toggle
            toggle = 1;

            for (int j = 0; j < n - 1; j++) //Loop through the columns
            {
                //Find the largest value in column j
                double colMax = Math.Abs(result[j, j]);
                int pRow = j;
                for (int i = j + 1; i < n; i++)
                {
                    if (result[i, j] > colMax)
                    {
                        colMax = result[i, j];
                        pRow = i;
                    }
                }

                //If the largest value is not on the pivot, swap the rows
                if (pRow != j)
                {
                    double temp;
                    for (int k = 0; k < n; k++)
                    {
                        temp = result[pRow, k];
                        result[pRow, k] = result[j, k];
                        result[j, k] = temp;
                    }

                    //Swap the permutation information
                    int tempRow = pVect[pRow];
                    pVect[pRow] = pVect[j];
                    pVect[j] = tempRow;

                    //Adjust the row-swap toggle
                    toggle = -toggle;
                }

                if (Math.Abs(result[j, j]) < 1.0E-20)
                {
                    throw new ArgumentException("The matrix is not decomposable.", "matrix");
                }

                for (int i = j + 1; i < n; i++)
                {
                    result[i, j] /= result[j, j];
                    for (int k = j + 1; k < n; k++)
                    {
                        result[i, k] -= result[i, j] * result[j, k];
                    }
                }
            }

            return result;
        }

        //Helper method used to assist in finding the inverse
        private static double[] HelperSolve(double[,] luMatrix, double[] b)
        {
            //Solves luMatrix * x = b
            int n = luMatrix.GetLength(0);
            double[] x = new double[n];
            b.CopyTo(x, 0);

            for (int i = 1; i < n; i++)
            {
                double sum = x[i];
                for (int j = 0; j < i; j++)
                {
                    sum -= luMatrix[i, j] * x[j];
                }
                x[i] = sum;
            }

            x[n - 1] /= luMatrix[n - 1, n - 1];
            for (int i = n - 2; i >= 0; i--)
            {
                double sum = x[i];
                for (int j = i + 1; j < n; j++)
                {
                    sum -= luMatrix[i, j] * x[j];
                }
                x[i] = sum / luMatrix[i, i];
            }

            return x;
        }

        internal static double[,] InverseMatrix(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[,] result = CopyMatrix(matrix);

            int[] perm;
            int toggle;
            double[,] lum = DecomposeMatrix(matrix, out perm, out toggle);

            double[] b = new double[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == perm[j])
                    {
                        b[j] = 1.0;
                    }
                    else
                    {
                        b[j] = 0.0;
                    }
                }

                double[] x = HelperSolve(lum, b);

                for (int j = 0; j < n; j++)
                {
                    result[j, i] = x[j];
                }
            }

            return result;
        }

        //This saves a decomposition step when both the determinant and inverse are needed for the same matrix
        internal static double[,] InverseMatrixandDeterminante(double[,] matrix, out double determinant)
        {
            int n = matrix.GetLength(0);
            double[,] result = CopyMatrix(matrix);

            int[] perm;
            int toggle;
            double[,] lum = DecomposeMatrix(matrix, out perm, out toggle);

            //Compute the inverse
            double[] b = new double[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == perm[j])
                    {
                        b[j] = 1.0;
                    }
                    else
                    {
                        b[j] = 0.0;
                    }
                }

                double[] x = HelperSolve(lum, b);

                for (int j = 0; j < n; j++)
                {
                    result[j, i] = x[j];
                }
            }

            //Compute the determinant
            determinant = toggle;
            for (int i = 0; i < lum.GetLength(0); i++)
            {
                determinant *= lum[i, i];
            }

            return result;
        }

        internal static double Determinant(double[,] matrix)
        {
            int[] perm;
            int toggle;
            double[,] lum = DecomposeMatrix(matrix, out perm, out toggle);

            double result = toggle;
            for (int i = 0; i < lum.GetLength(0); i++)
            {
                result *= lum[i, i];
            }

            return result;
        }

        /// <summary>
        /// Calculates the product of a row vector (size 1 x m) by a matrix (size m x n)
        /// </summary>
        /// <param name="vector">Input vector of size 1 x m.</param>
        /// <param name="?">Input matrix of size m x n.</param>
        /// <returns>?Vector (size 1 x n)</returns>
        internal static double[] VectorTimesMatrix(double[] vector, double[,] matrix)
        {
            if (vector.Length != matrix.GetLength(0))
            {
                throw new ArgumentException("The vector length must equal the number of rows in the matrix.");
            }

            double[] result = new double[matrix.GetLength(1)];

            for (int i = 0; i < result.Length; i++)
            {
                double tempResult = 0.0;
                for (int j = 0; j < vector.Length; j++)
                {
                    tempResult += vector[j] * matrix[j, i];
                }
                result[i] = tempResult;
            }

            return result;
        }

        /// <summary>
        /// Calculates the inner product (dot product) of two vectors.  Note: All vectors are stored as 1D arrays, the notations of which is a row vector and which is a column vector are for reference only.
        /// </summary>
        /// <param name="A">The row vector (size 1 x m).</param>
        /// <param name="B">The column vector (size m x 1).</param>
        /// <returns>The scalar result of the inner product.</returns>
        internal static double InnerProduct(double[] A, double[] B)
        {
            if (A.Length != B.Length)
            {
                throw new ArgumentException("The sizes of vectors A and B must be the same.");
            }

            double result = 0.0;
            for (int i = 0; i < A.Length; i++)
            {
                result += A[i] * B[i];
            }

            return result;
        }

        /// <summary>
        /// Calculates the outer product of two vectors.  Note: All vectors are stored as 1D arrays, the notations of which is a row vector and which is a column vector are for reference only.
        /// </summary>
        /// <param name="A">The column vector (size m x 1).</param>
        /// <param name="B">The row vector (size 1 x m).</param>
        /// <returns></returns>
        internal static double[,] OuterProduct(double[] A, double[] B)
        {
            if (A.Length != B.Length)
            {
                throw new ArgumentException("The sizes of vectors A and B must be the same.");
            }

            double[,] result = new double[A.Length, B.Length];

            for (int i = 0; i < A.Length; i++) //Row
            {
                for (int j = 0; j < B.Length; j++) //Column
                {
                    result[i, j] = A[i] * B[j];
                }
            }

            return result;
        }

        /// <summary>
        /// Subtracts vector B from vector A.
        /// </summary>
        /// <param name="A">A vector of length m.</param>
        /// <param name="B">Another vector, also of length m.</param>
        /// <returns></returns>
        internal static double[] SubtractVectors(double[] A, double[] B)
        {
            if (A.Length != B.Length)
            {
                throw new ArgumentException("Both vectors must be of the same length.");
            }

            double[] result = new double[A.Length];
            for (int i = 0; i < A.Length; i++)
            {
                result[i] = A[i] - B[i];
            }

            return result;
        }

        /// <summary>
        /// Adds vector B to vector A.
        /// </summary>
        /// <param name="A">A vector of length m.</param>
        /// <param name="B">Another vector, also of length m.</param>
        /// <returns></returns>
        internal static double[] AddVectors(double[] A, double[] B)
        {
            if (A.Length != B.Length)
            {
                throw new ArgumentException("Both vectors must be of the same length.");
            }

            double[] result = new double[A.Length];
            for (int i = 0; i < A.Length; i++)
            {
                result[i] = A[i] + B[i];
            }

            return result;
        }

        internal static double[] ScalarTimesVector(double A, double[] B)
        {
            double[] result = new double[B.Length];

            for (int i = 0; i < B.Length; i++)
            {
                result[i] = A * B[i];
            }

            return result;
        }

        internal static double[,] ScalarTimesMatrix(double A, double[,] B)
        {
            int height = B.GetLength(0);
            int width = B.GetLength(1);
            double[,] result = new double[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    result[i, j] = A * B[i, j];
                }
            }

            return result;
        }

        internal static double[,] AddMatrices(double[,] A, double[,] B)
        {
            if (A.GetLength(0) != B.GetLength(0) || A.GetLength(1) != B.GetLength(1))
            {
                throw new ArgumentException("Both matrices must be of the same size.");
            }

            int height = A.GetLength(0);
            int width = A.GetLength(1);
            double[,] result = new double[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    result[i, j] = A[i, j] + B[i, j];
                }
            }

            return result;
        }
    }
}
