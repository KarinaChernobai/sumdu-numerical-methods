using System;
using NumMethods2;
using static NumMethods.CubicSplineInterpolation;

namespace NumMethods;

internal static class Program
{
	const string DoubleFormat = "0.##############";

	static void Main(string[] args)
	{
		IntegrateSimpson();
	}

	static void IntegrateSimpson()
	{
		var a = 1;
		var b = 3;
		var res = Integral.IntegrateSimpson(a, b, 0.001, x => Math.Sqrt(1 + x * x * x));
		Console.WriteLine($"The definite integral from {a} to {b} is {res.ToString(DoubleFormat)}");
	}

	static void LeastSquares()
	{
		var samples = new NumMethods2.Point2D[]
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
		var obj = FnDescriptor.Create(samples);
		obj.LinearRegrCoeffs();
		Console.WriteLine($"The chosen function {obj.Name}\n");
		Console.WriteLine($"Coefficients a0 = {obj.A0.ToString(DoubleFormat)} a1 = {obj.A1.ToString(DoubleFormat)}\n");
		Console.WriteLine($"Coefficient of determination r^2 = {obj.DetermCoeff.ToString(DoubleFormat)}\n");
		Console.WriteLine($"Fisher's criteria f = {obj.FishersCriteria.ToString(DoubleFormat)}\n");
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