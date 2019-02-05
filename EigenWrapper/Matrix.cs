using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EigenWrapper
{
    public class Matrix
    {
        internal double[,] matrix;
        private int rows;
        public int Rows
        {
            get { return rows; }
        }
        private int columns;
        public int Columns
        {
            get { return columns; }
        }
        public bool HasInverse
        {
            get
            {
                if (rows == columns)
                {
                    return MatrixMath.HasInverse(this.matrix);
                }
                else
                {
                    return false;
                }
            }
        }

        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            matrix = new double[columns, rows]; //This will put it in column major order to match the default in Eigen
        }
        internal Matrix(double[,] data)
        {
            this.rows = data.GetLength(1);
            this.columns = data.GetLength(0);
            matrix = data;
        }

        public double this[int rowIdx, int columnIdx]
        {
            //These are reversed to index it matrix style (column major), instead of image style (row major)
            get
            {
                return matrix[columnIdx, rowIdx];
            }
            set
            {
                matrix[columnIdx, rowIdx] = value;
            }
        }

        public Matrix Inverse()
        {
            double[,] inverseData = MatrixMath.InvertMatrix(this.matrix);
            return new Matrix(inverseData);
        }
        public static Matrix Inverse(Matrix A)
        {
            double[,] inverseData = MatrixMath.InvertMatrix(A.matrix);
            return new Matrix(inverseData);
        }

        public static Matrix operator *(Matrix A, Matrix B)
        {
            if (A.columns == B.rows)
            {
                double[,] C = MatrixMath.MultiplyMatrices(A.matrix, B.matrix);
                return new Matrix(C);
            }
            else
            {
                throw new IndexOutOfRangeException("The number of columns of matrix A must equal the number of rows of matrix B");
            }
        }
        public static Matrix operator *(Matrix A, double b)
        {
            double[,] c = MatrixMath.MultiplyMatrixScalar(A.matrix, b);
            return new Matrix(c);
        }
        public static Matrix operator *(double a, Matrix B)
        {
            double[,] c = MatrixMath.MultiplyMatrixScalar(B.matrix, a);
            return new Matrix(c);
        }
        public static Matrix Multiply(Matrix leftMatrix, Matrix rightMatrix)
        {
            if (leftMatrix.columns == rightMatrix.rows)
            {
                double[,] result = MatrixMath.MultiplyMatrices(leftMatrix.matrix, rightMatrix.matrix);
                return new Matrix(result);
            }
            else
            {
                throw new IndexOutOfRangeException("The number of columns of the left matrix must equal the number of rows of the right matrix");
            }
        }
        public static Vector Multiply(Matrix matrix, Vector vector)
        {
            if (matrix.Columns == vector.Length)
            {
                double[] result = MatrixMath.MultiplyMatrixVector(matrix.matrix, vector.vector);
                return new Vector(result);
            }
            else
            {
                throw new IndexOutOfRangeException("The number of columns of the matrix must equal the number of elements in the vector.");
            }
        }
        public static Matrix Multiply(Matrix matrix, double scalar)
        {
            double[,] result = MatrixMath.MultiplyMatrixScalar(matrix.matrix, scalar);
            return new Matrix(result);
        }

        public static Matrix operator /(Matrix A, double b)
        {
            double[,] C = MatrixMath.DivideMatrixScalar(A.matrix, b);
            return new Matrix(C);
        }

        public static Matrix operator +(Matrix A, Matrix B)
        {
            if (A.columns == B.columns && A.rows == B.rows)
            {
                double[,] C = MatrixMath.AddMatrices(A.matrix, B.matrix);
                return new Matrix(C);
            }
            else
            {
                throw new IndexOutOfRangeException("Both matrices must be the same size.");
            }
        }
        public static Matrix Add(Matrix leftMatrix, Matrix rightMatrix)
        {
            if (leftMatrix.columns == rightMatrix.columns && leftMatrix.rows == rightMatrix.rows)
            {
                double[,] C = MatrixMath.AddMatrices(leftMatrix.matrix, rightMatrix.matrix);
                return new Matrix(C);
            }
            else
            {
                throw new IndexOutOfRangeException("Both matrices must be the same size.");
            }
        }

        public static Matrix operator -(Matrix A, Matrix B)
        {
            if (A.columns == B.columns && A.rows == B.rows)
            {
                double[,] C = MatrixMath.SubtractMatrices(A.matrix, B.matrix);
                return new Matrix(C);
            }
            else
            {
                throw new IndexOutOfRangeException("Both matrices must be the same size.");
            }
        }
        public static Matrix Subtract(Matrix leftMatrix, Matrix rightMatrix)
        {
            if (leftMatrix.columns == rightMatrix.columns && leftMatrix.rows == rightMatrix.rows)
            {
                double[,] C = MatrixMath.SubtractMatrices(leftMatrix.matrix, rightMatrix.matrix);
                return new Matrix(C);
            }
            else
            {
                throw new IndexOutOfRangeException("Both matrices must be the same size.");
            }
        }

        public static Matrix RandomMatrix(int rows, int columns, double minValue, double maxValue)
        {
            double[,] data = new double[rows, columns];
            Random rand = new Random();

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    data[i, j] = rand.NextDouble() * (maxValue - minValue) + minValue;
                }
            }

            return new Matrix(data);
        }

        public static Matrix Identity(int size)
        {
            double[,] data = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                data[i, i] = 1.0;
            }
            return new Matrix(data);
        }

        public Matrix Transpose()
        {
            double[,] data = MatrixMath.TransposeMatrix(this.matrix);
            return new Matrix(data);
        }
        public static Matrix Transpose(Matrix A)
        {
            double[,] data = MatrixMath.TransposeMatrix(A.matrix);
            return new Matrix(data);
        }

        public double Determinant()
        {
            if (rows == columns)
            {
                return MatrixMath.MatrixDeterminant(this.matrix);
            }
            else
            {
                throw new IndexOutOfRangeException("Determinants only exist on square matrices.");
            }
        }
        public static double Determinant(Matrix A)
        {
            if (A.rows == A.columns)
            {
                return MatrixMath.MatrixDeterminant(A.matrix);
            }
            else
            {
                throw new IndexOutOfRangeException("Determinants only exist on square matrices.");
            }
        }

        public double Norm()
        {
            return MatrixMath.MatrixNorm(this.matrix);
        }
        public static double Norm(Matrix A)
        {
            return MatrixMath.MatrixNorm(A.matrix);
        }
    }
}
