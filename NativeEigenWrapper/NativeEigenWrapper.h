#include "stdafx.h"

double* MultiplyMatrices(double* A, int ARows, int AColumns, double* B, int BRows, int BColumns);
double* MultiplyMatrixVector(double* A, int ARows, int AColumns, double* B, int BElements);
double* MultiplyMatrixScalar(double* A, int rows, int columns, double b);
double* DivideMatrixScalar(double* A, int rows, int columns, double b);
double* InvertMatrix(double* A, int rows, int columns);
double* AddMatrices(double* A, double* B, int rows, int columns);
double* SubtractMatrices(double* A, double* B, int rows, int columns);
double* TransposeMatrix(double* A, int rows, int columns);
double CalculateDeterminant(double* A, int rows, int columns);
bool IsInvertible(double* A, int rows, int columns);
double CalculateMatrixNorm(double* A, int rows, int columns);
double* CrossProduct(double* A, double* B);
double DotProduct(double* A, double* B, int elements);
double* AddVectors(double* A, double* B, int elements);
double* SubtractVectors(double* A, double* B, int elements);
double* NormalizeVector(double* A, int elements);
double VectorMagnitude(double* A, int elements);