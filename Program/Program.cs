using System;

namespace NumMethods;

internal class Program
{
	static void Main(string[] args)
	{
		// GradientDescent.Solve();
		// LagrangeInterpolation.Solve();
		double[,] samples = { { 0, 1, 2, 3, 4 }, { 0.5, 2, 7.5, 20, 42.5} };
		var lagrange = new LagrangeInterpolation2(samples);
		lagrange.Interpolate(3.2);
	}
}