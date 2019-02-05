// NativeEigenWrapper.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <Eigen\Core>
#include <Eigen\Dense>
#include <Eigen\LU>

using Eigen::MatrixXd;
using Eigen::VectorXd;
using Eigen::Vector3d;

double* MultiplyMatrices(double* A, int ARows, int AColumns, double* B, int BRows, int BColumns)
{
	typedef Eigen::Map<MatrixXd> MapMatrix;
	MapMatrix aMap(A, ARows, AColumns);
	MapMatrix bMap(B, BRows, BColumns);
	double* C = new double[ARows * BColumns];
	MapMatrix cMap(C, ARows, BColumns);
	cMap = aMap * bMap;
	return C;
}

double* MultiplyMatrixVector(double* A, int ARows, int AColumns, double* B, int BElements)
{
	typedef Eigen::Map<MatrixXd> MapMatrix;
	typedef Eigen::Map<VectorXd> MapVector;
	MapMatrix aMap(A, ARows, AColumns);
	MapVector bMap(B, BElements);
	double* C = new double[ARows];
	MapVector cMap(C, ARows);
	cMap = aMap * bMap;
	return C;
}

double* MultiplyMatrixScalar(double* A, int rows, int columns, double b)
{
	typedef Eigen::Map<MatrixXd> MapMatrix;
	MapMatrix aMap(A, rows, columns);
	double* C = new double[rows * columns];
	MapMatrix cMap(C, rows, columns);
	cMap = aMap * b;
	return C;
}

double* DivideMatrixScalar(double* A, int rows, int columns, double b)
{
	typedef Eigen::Map<MatrixXd> MapMatrix;
	MapMatrix aMap(A, rows, columns);
	double* C = new double[rows * columns];
	MapMatrix cMap(C, rows, columns);
	cMap = aMap / b;
	return C;
}

double* InvertMatrix(double* A, int rows, int columns)
{
	typedef Eigen::Map<MatrixXd> MapMatrix;
	double* A2 = new double[rows * columns];
	//TODO: Do I really need a memcpy here?  Why can't I just use the original A?
	memcpy(A2, A, sizeof(double) * rows * columns);
	MapMatrix aMap(A2, rows, columns);
	double* B = new double[rows * columns];
	MapMatrix bMap(B, rows, columns);
	bMap = aMap.inverse();
	delete[] A2;
	return B;
}

double* AddMatrices(double* A, double* B, int rows, int columns)
{
	typedef Eigen::Map<MatrixXd> MapMatrix;
	MapMatrix aMap(A, rows, columns);
	MapMatrix bMap(B, rows, columns);
	double* C = new double[rows * columns];
	MapMatrix cMap(C, rows, columns);
	cMap = aMap + bMap;
	return C;
}

double* SubtractMatrices(double* A, double* B, int rows, int columns)
{
	typedef Eigen::Map<MatrixXd> MapMatrix;
	MapMatrix aMap(A, rows, columns);
	MapMatrix bMap(B, rows, columns);
	double* C = new double[rows * columns];
	MapMatrix cMap(C, rows, columns);
	cMap = aMap - bMap;
	return C;
}

double* TransposeMatrix(double* A, int rows, int columns)
{
	typedef Eigen::Map<MatrixXd> MapMatrix;
	double* A2 = new double[rows * columns];
	memcpy(A2, A, sizeof(double) * rows * columns);
	MapMatrix aMap(A2, rows, columns);
	double* B = new double[rows * columns];
	MapMatrix bMap(B, columns, rows);
	bMap = aMap.transpose();
	delete[] A2;
	return B;
}

double CalculateDeterminant(double* A, int rows, int columns)
{
	typedef Eigen::Map<MatrixXd> MapMatrix;
	double* A2 = new double[rows * columns];
	memcpy(A2, A, sizeof(double) * rows * columns);
	MapMatrix aMap(A2, rows, columns);
	double b = aMap.determinant();
	delete[] A2;
	return b;
}

bool IsInvertible(double* A, int rows, int columns)
{
	typedef Eigen::Map<MatrixXd> MapMatrix;
	double* A2 = new double[rows * columns];
	memcpy(A2, A, sizeof(double) * rows * columns);
	MapMatrix aMap(A2, rows, columns);
	bool isInvertable = aMap.fullPivLu().isInvertible();
	delete[] A2;
	return isInvertable;
}

double CalculateMatrixNorm(double* A, int rows, int columns)
{
	typedef Eigen::Map<MatrixXd> MapMatrix;
	double* A2 = new double[rows * columns];
	memcpy(A2, A, sizeof(double) * rows * columns);
	MapMatrix aMap(A2, rows, columns);
	double b = aMap.norm();
	delete[] A2;
	return b;
}

double* CrossProduct(double* A, double* B)
{
	typedef Eigen::Map<Vector3d> MapVector;
	MapVector aMap(A, 3);
	MapVector bMap(B, 3);
	double* C = new double[3];
	MapVector cMap(C, 3);
	cMap = aMap.cross(bMap);
	return C;
}

double DotProduct(double* A, double* B, int elements)
{
	typedef Eigen::Map<VectorXd> MapVector;
	MapVector aMap(A, elements);
	MapVector bMap(B, elements);
	double C = aMap.dot(bMap);
	return C;
}

double* AddVectors(double* A, double* B, int elements)
{
	typedef Eigen::Map<VectorXd> MapVector;
	MapVector aMap(A, elements);
	MapVector bMap(B, elements);
	double* C = new double[elements];
	MapVector cMap(C, elements);
	cMap = aMap + bMap;
	return C;
}

double* SubtractVectors(double* A, double* B, int elements)
{
	typedef Eigen::Map<VectorXd> MapVector;
	MapVector aMap(A, elements);
	MapVector bMap(B, elements);
	double* C = new double[elements];
	MapVector cMap(C, elements);
	cMap = aMap - bMap;
	return C;
}

double* NormalizeVector(double* A, int elements)
{
	typedef Eigen::Map<VectorXd> MapVector;
	MapVector aMap(A, elements);
	double* B = new double[elements];
	memcpy(B, A, sizeof(double) * elements);
	MapVector bMap(B, elements);
	bMap.normalize();
	return B;
}

double VectorMagnitude(double* A, int elements)
{
	typedef Eigen::Map<VectorXd> MapVector;
	MapVector aMap(A, elements);
	double b = aMap.norm();
	return b;
}
