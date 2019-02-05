// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include "NativeEigenWrapper.h"

BOOL APIENTRY DllMain( HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
		case DLL_PROCESS_ATTACH:
		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
		case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}

extern "C" double* WINAPI RunMultiplyMatrices(double* A, int ARows, int AColumns, double* B, int BRows, int BColumns)
{
	return MultiplyMatrices(A, ARows, AColumns, B, BRows, BColumns);
}

extern "C" double* WINAPI RunMultiplyMatrixVector(double* A, int ARows, int AColumns, double* B, int BElements)
{
	return MultiplyMatrixVector(A, ARows, AColumns, B, BElements);
}

extern "C" double* WINAPI RunMultiplyMatrixScalar(double* A, int rows, int columns, double b)
{
	return MultiplyMatrixScalar(A, rows, columns, b);
}

extern "C" double* WINAPI RunDivideMatrixScalar(double* A, int rows, int columns, double b)
{
	return DivideMatrixScalar(A, rows, columns, b);
}

extern "C" double* WINAPI RunInvertMatrix(double* A, int rows, int columns)
{
	return InvertMatrix(A, rows, columns);
}

extern "C" double* WINAPI RunAddMatrices(double* A, double* B, int rows, int columns)
{
	return AddMatrices(A, B, rows, columns);
}

extern "C" double* WINAPI RunSubtractMatrices(double* A, double* B, int rows, int columns)
{
	return SubtractMatrices(A, B, rows, columns);
}

extern "C" double* WINAPI RunTransposeMatrix(double* A, int rows, int columns)
{
	return TransposeMatrix(A, rows, columns);
}

extern "C" double WINAPI RunCalculateDeterminant(double* A, int rows, int columns)
{
	return CalculateDeterminant(A, rows, columns);
}

extern "C" bool WINAPI RunIsInvertible(double* A, int rows, int columns)
{
	return IsInvertible(A, rows, columns);
}

extern "C" double WINAPI RunCalculateMatrixNorm(double* A, int rows, int columns)
{
	return CalculateMatrixNorm(A, rows, columns);
}

extern "C" double* WINAPI RunCrossProduct(double* A, double* B)
{
	return CrossProduct(A, B);
}

extern "C" double WINAPI RunDotProduct(double* A, double* B, int elements)
{
	return DotProduct(A, B, elements);
}

extern "C" double* WINAPI RunAddVectors(double* A, double* B, int elements)
{
	return AddVectors(A, B, elements);
}

extern "C" double* WINAPI RunSubtractVectors(double* A, double* B, int elements)
{
	return SubtractVectors(A, B, elements);
}

extern "C" double* WINAPI RunNormalizeVector(double* A, int elements)
{
	return NormalizeVector(A, elements);
}

extern "C" double WINAPI RunVectorMagnitude(double* A, int elements)
{
	return VectorMagnitude(A, elements);
}

extern "C" void WINAPI FreeMatrix(double* A)
{
	delete[] A;
}
