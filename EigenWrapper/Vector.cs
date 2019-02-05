using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EigenWrapper
{
    /// <summary>
    /// A class to represent a column vector.
    /// </summary>
    public class Vector
    {
        internal double[] vector;
        private int length;
        public int Length
        {
            get { return length; }
        }

        public Vector(int length)
        {
            this.length = length;
            vector = new double[length];
        }
        internal Vector(double[] data)
        {
            this.length = data.Length;
            vector = data;
        }

        public double this[int Idx]
        {
            get
            {
                return vector[Idx];
            }
            set
            {
                vector[Idx] = value;
            }
        }

        public static Vector operator *(Matrix A, Vector B)
        {
            if (A.Columns == B.length)
            {
                double[] C = MatrixMath.MultiplyMatrixVector(A.matrix, B.vector);
                return new Vector(C);
            }
            else
            {
                throw new IndexOutOfRangeException("The number of columns of the matrix must equal the number of elements in the vector.");
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

        public static Vector operator +(Vector A, Vector B)
        {
            if (A.length == B.length)
            {
                double[] C = VectorMath.AddVectors(A.vector, B.vector);
                return new Vector(C);
            }
            else
            {
                throw new IndexOutOfRangeException("Both vectors must contain the same number of elements.");
            }
        }
        public static Vector Add(Vector leftVector, Vector rightVector)
        {
            if (leftVector.length == rightVector.length)
            {
                double[] result = VectorMath.AddVectors(leftVector.vector, rightVector.vector);
                return new Vector(result);
            }
            else
            {
                throw new IndexOutOfRangeException("Both vectors must contain the same number of elements.");
            }
        }

        public static Vector operator -(Vector A, Vector B)
        {
            if (A.length == B.length)
            {
                double[] C = VectorMath.SubtractVectors(A.vector, B.vector);
                return new Vector(C);
            }
            else
            {
                throw new IndexOutOfRangeException("Both vectors must contain the same number of elements.");
            }
        }
        public static Vector Subtract(Vector leftVector, Vector rightVector)
        {
            if (leftVector.length == rightVector.length)
            {
                double[] result = VectorMath.SubtractVectors(leftVector.vector, rightVector.vector);
                return new Vector(result);
            }
            else
            {
                throw new IndexOutOfRangeException("Both vectors must contain the same number of elements.");
            }
        }

        public Vector CrossProduct(Vector secondVector)
        {
            if (length == secondVector.length && length == 3)
            {
                double[] result = VectorMath.CrossProduct(this.vector, secondVector.vector);
                return new Vector(result);
            }
            else
            {
                throw new IndexOutOfRangeException("Both vectors must contain the same number of elements and be of size 3.");
            }
        }
        public static Vector CrossProduct(Vector leftVector, Vector rightVector)
        {
            if (leftVector.length == rightVector.length && leftVector.length == 3)
            {
                double[] result = VectorMath.CrossProduct(leftVector.vector, rightVector.vector);
                return new Vector(result);
            }
            else
            {
                throw new IndexOutOfRangeException("Both vectors must contain the same number of elements and be of size 3.");
            }
        }

        public double DotProduct(Vector secondVector)
        {
            if (length == secondVector.length)
            {
                double result = VectorMath.DotProduct(this.vector, secondVector.vector);
                return result;
            }
            else
            {
                throw new IndexOutOfRangeException("Both vectors must contain the same number of elements.");
            }
        }
        public static double DotProduct(Vector leftVector, Vector rightVector)
        {
            if (leftVector.length == rightVector.length)
            {
                double result = VectorMath.DotProduct(leftVector.vector, rightVector.vector);
                return result;
            }
            else
            {
                throw new IndexOutOfRangeException("Both vectors must contain the same number of elements.");
            }
        }

        public Vector Normalize()
        {
            double[] result = VectorMath.NormalizeVector(this.vector);
            return new Vector(result);
        }
        public static Vector Normalize(Vector vector)
        {
            double[] result = VectorMath.NormalizeVector(vector.vector);
            return new Vector(result);
        }

        public double Magnitude()
        {
            double result = VectorMath.VectorMagnitude(this.vector);
            return result;
        }
        public static double Magnitude(Vector vector)
        {
            double result = VectorMath.VectorMagnitude(vector.vector);
            return result;
        }

        public static Vector RandomVector(int size, double minValue, double maxValue)
        {
            double[] data = new double[size];
            Random rand = new Random();

            for (int i = 0; i < size; i++)
            {
                data[i] = rand.NextDouble() * (maxValue - minValue) + minValue;
            }

            return new Vector(data);
        }
    }
}
