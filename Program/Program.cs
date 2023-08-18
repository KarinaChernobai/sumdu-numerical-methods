using System;
using static NumMethods.CubicSplineInterpolation;

namespace NumMethods;

internal static class Program
{
	const string DoubleFormat = "0.##############";

	static void Main(string[] args)
	{
		Console.WriteLine("");
		var samples = new LeastSquares.Point2D[]
		{
			new(1, 33.5),
			new(10, 37),
			new(20, 41.2),
			new(30, 46.1),
			new(40, 50),
			new(50, 52.9),
			new(60, 56.8),
			new(70, 64.3),
			new(80, 69.9),
		};
		var leastSquares = new LeastSquares(samples);
		var num = leastSquares.FindRegrFunction();
		leastSquares.Linearize(num);
		leastSquares.LinearRegrCoeffs(num);

		var samples2 = new LeastSquares.Point2D[]
		{
			new(2, 14),
			new(5, 21),
			new(7, 25),
			new(8, 26),
			new(10, 30),
			new(12, 34),
			new(15, 41),
			new(20, 54),
			new(25, 67),
			new(40, 110),
		};
		var leastSquares2 = new LeastSquares(samples2);
		num = leastSquares2.FindRegrFunction();
		leastSquares2.Linearize(num);
		leastSquares2.LinearRegrCoeffs(num);
	}

	static void Interpolate() 
	{
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
		Console.WriteLine($"Lagrange: {res.ToString(DoubleFormat)}");
		Console.WriteLine();

		var spline = new CubicSplineInterpolation(samples);
		foreach (var (a, b, c, d) in new TestAccess(spline).GetCoeffs())
		{
			Console.WriteLine($"{a},{b},{c},{d}");
		}

		res = spline.Interpolate(3.2);
		Console.WriteLine($"Spline: {res.ToString(DoubleFormat)}");
	}
}