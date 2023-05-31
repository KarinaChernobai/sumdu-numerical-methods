using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods;

public static class SeidelMethod2
{
	private static double[]? Solve2(double[,] A, double[] b, double epsilon)
	{
		if(!isMatrixDiagDominant(A)) return null;
		var n = A.GetLength(0);
		var x0 = new double[n];
		var x = new double[n];
		var c = new double[n];
		var epsilonSqr = epsilon * epsilon;
		var sqrSumPrev = double.PositiveInfinity;
		
		for (var counter = 0; ; counter++)
		{
			UMatrixProdVector(A, x0, c);
			for (var i = 0; i < n; i++) c[i] = b[i] - c[i];

			ForwardSubstitution(A, c, x);

			var sqrSum = SumOfSqrVectorDiff(x, x0);

			Console.WriteLine($"\nIteration {counter}:");
			WriteVector(x);
			Console.WriteLine($"Sum of squares: {sqrSum:F10}");

			if (sqrSum >= sqrSumPrev) return null;
			if (sqrSum <= epsilonSqr) return x;
			
			sqrSumPrev = sqrSum;
			(x0, x) = (x, x0);
		}
	}

	private static bool isMatrixDiagDominant(double[,] A) 
	{
		var len = A.GetLength(0);
		for (var rowIndex = 0; rowIndex < len; rowIndex++) 
		{
			var sum = default(double);
			for (var columnIndex = 0; columnIndex < rowIndex; columnIndex++)
			{
				sum += Math.Abs(A[rowIndex, columnIndex]);
			}
			for (var columnIndex = rowIndex+1; columnIndex < len; columnIndex++)
			{
				sum += Math.Abs(A[rowIndex, columnIndex]);
			}
			if (sum > Math.Abs(A[rowIndex, rowIndex])) return false;
		}
		return true;
	}

	private static void ForwardSubstitution(double[,] lowerMatrix, double[] bVector, double[] rVector)
	{
		var rowCount = lowerMatrix.GetLength(0);
		rVector[0] = bVector[0] / lowerMatrix[0, 0];
		for (var rowIndex = 1; rowIndex < rowCount; rowIndex++)
		{
			ForwardSubstitutionSum(lowerMatrix, rowIndex, rVector);
			rVector[rowIndex] = (bVector[rowIndex] - rVector[rowIndex]) / lowerMatrix[rowIndex, rowIndex];
		}
	}

	private static void ForwardSubstitutionSum(double[,] lowerMatrix, int rowIndex, double[] rVector)
	{
		rVector[rowIndex] = 0;
		for (var i = 0; i < rowIndex; i++)
		{
			rVector[rowIndex] += rVector[i] * lowerMatrix[rowIndex, i];
		}
	}

	private static double SumOfSqrVectorDiff(double[] v2, double[] v1)
	{
		var sum = 0d;
		for (var i = 0; i < v1.Length; i++)
		{
			var d = v2[i] - v1[i];
			sum += d * d;
		}
		return sum;
	}

	private static void UMatrixProdVector(double[,] uMatrix, double[] v, double[] r)
	{
		var rowLastIndex = r.Length - 1;
		for (var rowIndex = 0; rowIndex < rowLastIndex; rowIndex++)
		{
			ref var sum = ref r[rowIndex];
			sum = 0;
			for (var columnIndex = rowIndex + 1; columnIndex <= rowLastIndex; columnIndex++) sum += uMatrix[rowIndex, columnIndex] * v[columnIndex];
		}
		r[rowLastIndex] = 0;
	}

	private static void WriteVector(double[] vector)
	{
		Console.Write(vector[0].ToString("F10"));
		for (var i = 1; i < vector.Length; i++)
		{
			Console.Write(" | ");
			Console.Write(vector[i].ToString("F10"));
		}
		Console.WriteLine();
	}

	public static void Run()
	{
		var A = new double[,] { 
			{ -0.68, -0.18, 0.02, 0.21 }, 
			{ 0.16, -0.88, -0.14, 0.27 }, 
			{ 0.37, 0.27, -1.02, -0.24 },
			{ 0.12, 0.21, -0.18, -0.75 }
		};
		var C = new double[] { -1.83, 0.65, -2.23, 1.13 };

		var epsilon = 0.0001;
		var x = Solve2(A, C, epsilon);
		if (x == null)
		{
			Console.WriteLine("\nThe condition of the converge is not met.");
		}
		else
		{
			Console.WriteLine("\nResult:");
			WriteVector(x);
		}
	}
}
