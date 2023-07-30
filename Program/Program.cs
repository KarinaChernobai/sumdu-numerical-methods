using System;
using static NumMethods.CubicSplineInterpolation;

namespace NumMethods;

internal static class Program
{
	const string DoubleFormat = "0.##############";

	static void Main(string[] args)
	{
		Interpolate();
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