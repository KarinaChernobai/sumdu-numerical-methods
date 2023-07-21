using System;

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
		const double stepCount = 2;
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

		var spline = new CubicSplineInterpolation(samples);
		for (int i = 0; i < samples.Length - 1; i++)
		{
			Console.WriteLine($"S{i}");
			var x = samples[i].X;
			var dx = ( samples[i + 1].X - samples[i].X ) / stepCount;
			for (int step = 0; step <= stepCount; step++)
			{
				var y = spline.Interpolate(x, i);
				Console.WriteLine($"x = {x} y = {y}");
				x += dx;
			}
			Console.WriteLine();
		}
		res = spline.Interpolate(3.2);
		Console.WriteLine(res.ToString(DoubleFormat));
	}
}