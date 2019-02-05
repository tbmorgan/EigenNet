// EigenWrapper.h

#pragma once
#include <Eigen/Dense>
#include <Eigen/Core>
#include <memory>

using namespace System;

namespace EigenWrapper_CppCLI
{
	public ref class MatrixXd
	{
		// TODO: Add your methods for this class here.
	public:
		MatrixXd(int x, int y);
		~MatrixXd();
		!MatrixXd();
		static MatrixXd^ operator *(MatrixXd^ lhs, MatrixXd^ rhs)
		{
			Eigen::MatrixXd tempMat = *(lhs->matrix) * *(rhs->matrix);
			return gcnew MatrixXd(&tempMat);
		}
	internal:
	 	Eigen::MatrixXd* matrix;
		MatrixXd(Eigen::MatrixXd* matrix);
	};

	public ref class MatrixMath
	{
	public:
		//static double* MultipleMatrices(double* A, double* B,  int width, int height);
		static array<double, 2>^ MultiplyMatrices(array<double, 2>^ A, array<double, 2>^ B);
	};
}
