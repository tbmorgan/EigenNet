// This is the main DLL file.
#include "Stdafx.h"
#include "EigenWrapper.h"

using namespace EigenWrapper;
using Eigen::MatrixXd;

EigenWrapper::MatrixXd::MatrixXd(int x, int y)
{
	matrix = new Eigen::MatrixXd(x, y);
	*matrix = Eigen::MatrixXd::Random(x, y);
}

EigenWrapper::MatrixXd::MatrixXd(Eigen::MatrixXd* mat)
{
	matrix = mat;
}

EigenWrapper::MatrixXd::~MatrixXd()
{
	this->!MatrixXd();
}

EigenWrapper::MatrixXd::!MatrixXd()
{
	//Apparently, the Eigen matrix doesn't need to be manually destroyed?
}

//double* MatrixMath::MultipleMatrices(double* A, double* B, int width, int height)
//{
//	typedef Eigen::Map<MatrixXd> MapType;
//	double* A2 = new double[10];
//	double* B2 = new double[10];
//	MapType aMap(A2, width, height);
//	MapType bMap(B2, width, height);
//	double* C = new double[width * height];
//	MapType cMap(C, width, height);
//	//cMap = aMap * bMap;
//	return C;
//	return NULL;
//}

array<double, 2>^ MatrixMath::MultiplyMatrices(array<double, 2>^ A, array<double, 2>^ B)
{
	typedef Eigen::Map<Eigen::MatrixXd> MapType;

	pin_ptr<double> pinA = &A[A->GetLowerBound(0), A->GetLowerBound(1)];
	MapType aMap(pinA, A->GetLength(0), A->GetLength(1));
	pin_ptr<double> pinB = &B[B->GetLowerBound(0), B->GetLowerBound(1)];
	MapType bMap(pinB, B->GetLength(0), B->GetLength(1));

	array<double, 2>^ C = gcnew array<double, 2>(A->GetLength(0), A->GetLength(1));
	pin_ptr<double> pinC = &C[C->GetLowerBound(0), C->GetLowerBound(1)];
	MapType cMap(pinC, C->GetLength(0), C->GetLength(1));

	cMap = aMap * bMap;
	return C;
}