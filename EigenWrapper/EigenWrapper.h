// EigenWrapper.h

#pragma once
#include <Eigen/Dense>

using namespace System;

namespace EigenWrapper {

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
}
