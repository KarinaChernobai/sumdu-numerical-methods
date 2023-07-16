using System;

namespace NumMethods;

internal static class Program
{
	const string DoubleFormat = "0.##############";

	static void Main(string[] args)
	{
		// GradientDescent.Solve();
		var samples = new Point2D[]
		{
			new(0, 0.5),
			new(1, 2),
			new(2, 7.5),
			new(3, 20),
			new(4, 42.5),
		};
		var lagrange = new LagrangeInterpolation(samples);
		var res = lagrange.Interpolate(3.2);
		Console.WriteLine(res.ToString(DoubleFormat));

		// Cubic spline interpolation
		var spline = new CubicSplineInterpolation(samples);
		res = spline.Interpolate(3.2);
		Console.WriteLine(res.ToString(DoubleFormat));
	}
}