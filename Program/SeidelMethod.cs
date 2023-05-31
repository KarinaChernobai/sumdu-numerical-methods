using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods;

public static class SeidelMethod
{
	private static double[] Solve(double [,] A, double[] C, double epsilon) 
	{
		// square mtrx ?
		var n = A.GetLength(0);
		var x0 = new double[n];
		var x = new double[n];
		var B = new double[n, n];
		var D = new double[n];

		// assign values to mrtx B and vector D
		for (var i = 0; i < n; i++)
		{
			for (var j = 0; j < n; j++)
			{
				B[i, j] = i == j ? 0 : - A[i, j] / A[i, i];
			}
			D[i] = C[i] / A[i, i];
		}
		// find norm of the mtrx B
		var normB = default(double);
		for (var i = 0; i < n; i++)
		{
			var sum = default(double);
			for (var j = 0; j < n; j++)
			{
				sum += Math.Abs(B[i, j]);
			}
			if (normB < sum) normB = sum;
		}
		// find delta
		var delta = (1 - normB) / normB * epsilon;

		var counter = 0;
		while (true) 
		{
			// find next x
			var tmpX = new double[n];
			x[0] = D[0];
			for (var j = 1; j < n; j++)
			{
				x[0] += B[0, j] * x0[j];
			}
			for (var i = 1; i < n; i++)
			{
				x[i] = D[i];
				for (var j = 0; j < i + 1; j++)
				{
					x[i] += B[i, j] * tmpX[i-1];
				}
				for (var j = i + 1; j < n; j++)
				{
					x[i] += B[i, j] * x0[i];
				}
				tmpX[i] = x[i];
			}
			// find norm of the diff x and x0
			var normX = default(double);
			for (var i = 0; i < n; i++)
			{
				var xDiff = Math.Abs(x[i] - x0[i]);
				if (xDiff > normX) normX = xDiff;
			}
			if (normX < delta) break;
			(x0, x) = (x, x0);
			if (counter > 1000) 
			{
				Console.WriteLine("Answer was not found");
				return new double[n];
			}
			counter++;
		}
		return x;
	}

	private static double[] Solve2(double[,] A, double[] b, double epsilon) 
	{
		var n = A.GetLength(0);
		var x0 = new double[n];
		var x = new double[n];
		var L = new double[n, n];
		var U = new double[n, n];
		var epsilonSqr = epsilon * epsilon;

		// L and U mtrx set values
		for (var i = 0; i < n; i++)
		{
			for(var j = 0; j <= i; j++)
			{
				L[i, j] = A[i, j];
			}
			for (var j = i + 1; j < n; j++)
			{
				U[i, j] = A[i, j];
			}
		}
		var counter = 0;
		while (true) 
		{
			// U * x
			var Ux = new double[n];
			for (var i = 0; i < n; i++)
			{
				var sum = default(double);
				for (var j = 0; j < n; j++)
				{
					sum += U[i, j] * x0[j];
				}
				Ux[i] = sum;
			}
			// vecor diff b - Ux
			var vecDiff = new double[n];
			for (var i = 0; i < n; i++)
			{
				vecDiff[i] = b[i] - Ux[i];
			}
			// find next x-- forward substitution
			for (var i = 0; i < n; i++)
			{
				var sum = default(double);
				for (var j = 0; j < i - 1; j++)
				{
					sum += L[i, j] * x0[j];
				}
				x[i] = (b[i] - sum) / L[i, i];
			}

			var xSum = default(double);
			for (var i = 0; i < n; i++)
			{
				xSum += Math.Pow(x[i] - x0[i], 2);
			}
			if (xSum <= Math.Pow(epsilon, 2)) return x;

			(x0, x) = (x, x0);
			if (counter > 1000)
			{
				Console.WriteLine("Answer was not found");
				return new double[n];
			}
			counter++;
		}
	}

	public static void Run()
	{
		var A = new double[,] { { 2, 1}, { 1, 2 } };
		var C = new double[] { 8, 1 };

		var epsilon = 0.0001;
		var x = Solve2(A, C, epsilon);
		for (var i = 0; i < x.Length; i++) Console.WriteLine(x[i]);
	}
}
