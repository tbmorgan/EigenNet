// This is the main DLL file.

#include "stdafx.h"

#include "EigenWrapper.h"

using namespace EigenWrapper;

MatrixXd::MatrixXd(int x, int y)
{
	matrix = new Eigen::MatrixXd(x, y);
	*matrix = Eigen::MatrixXd::Random(x, y);
}

MatrixXd::MatrixXd(Eigen::MatrixXd* mat)
{
	matrix = mat;
}

MatrixXd::~MatrixXd()
{
	this->!MatrixXd();
}

MatrixXd::!MatrixXd()
{
	delete matrix;
}