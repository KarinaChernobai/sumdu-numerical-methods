using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods;

public class GradientDescent2
{
	// Using the steepest-descent method to search
	// for minimum values of a multi-variable function
	public static void steepestDescent(double[] x, double alpha, double tolerance)
	{
		int n = x.Length; //Size of input array
		double h = 1e-6; //Tolerance factor
		double g0 = g(x); //Initial estimate of result
						  //Calculate initial gradient
		double[] fi = new double[n];
		fi = GradG(x, h);
		//Calculate initial norm
		double DelG = 0;
		for (int i = 0; i < n; ++i)
			DelG += fi[i] * fi[i];
		DelG = Math.Sqrt(DelG);
		double b = alpha / DelG;
		//Iterate until value is <= tolerance limit
		while (DelG > tolerance)
		{
			//Calculate next value
			for (int i = 0; i < n; ++i)
				x[i] -= b * fi[i];
			h /= 2;
			fi = GradG(x, h); //Calculate next gradient
							  //Calculate next norm
			DelG = 0;
			for (int i = 0; i < n; ++i)
				DelG += fi[i] * fi[i];
			DelG = Math.Sqrt(DelG);
			b = alpha / DelG;
			//Calculate next value
			double g1 = g(x);
			//Adjust parameter
			if (g1 > g0) alpha /= 2;
			else g0 = g1;
		}
	}

	// Provides a rough calculation of gradient g(x).
	public static double[] GradG(double[] x, double h)
	{
		int n = x.Length;
		double[] z = new double[n];
		double[] y = (double[])x.Clone();
		double g0 = g(x);
		for (int i = 0; i < n; ++i)
		{
			y[i] += h;
			z[i] = (g(y) - g0) / h;
		}
		return z;
	}
	// Method to provide function g(x).
	public static double g(double[] x)
	{
		return (x[0] - 1) * (x[0] - 1) * Math.Exp(-x[1] * x[1]) +
		x[1] * (x[1] + 2) * Math.Exp(-2 * x[0] * x[0]);

	}

	public static void Run() 
	{
		// check if differentiable
		// check if convex -- calculate second derivative and check if it os greater than 0

		double tolerance = 1e-6;
		double alpha = 0.1;
		double[] x = new double[2];
		x[0] = 0.1; //Initial guesses
		x[1] = -1; //of location of minimums
		steepestDescent(x, alpha, tolerance);
		Console.WriteLine("Testing steepest descent method\n");
		Console.WriteLine("The minimum is at x[0] = " + x[0] + ", x[1] = " + x[1]);
		Console.ReadLine();
	}
}
