using System;
using NumMethods2;
using static NumMethods.CubicSplineInterpolation;

namespace NumMethods;

internal static class Program
{
	const string DoubleFormat = "0.##############";

	static void Main(string[] args)
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
		Console.WriteLine($"a0: {obj.A0} a1: {obj.A1} detr: {obj.DetermCoeff} fisher: {obj.FishersCriteria}");
		Console.WriteLine("");
/*
		var samples2 = new NumMethods2.Point2D[]
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
		var obj2 = FnDescriptor.Create(samples2);
		obj2.LinearRegrCoeffs();
		Console.WriteLine($"a0: {a0} a1: {a1} detr: {obj2.DetermCoeff} fisher: {obj2.FishersCriteria}");
		Console.WriteLine("");

		var samples3 = new NumMethods2.Point2D[]
{
			new(0.23, 95.2),
			new(0.29, 89.2),
			new(0.39, 73.4),
			new(0.53, 62.7),
			new(0.67, 55.5),
			new(0.82, 49.3),
			new(0.99, 38.4),
			new(1.23, 26.7),
			new(1.4, 16.4),
};
		var obj3 = FnDescriptor.Create(samples3);
		(a0, a1) = obj3.LinearRegrCoeffs();
		Console.WriteLine($"a0: {a0} a1: {a1} detr: {obj3.DetermCoeff} fisher: {obj3.FishersCriteria}");
		Console.WriteLine("");*/
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