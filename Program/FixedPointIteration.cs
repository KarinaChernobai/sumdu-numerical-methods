using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods;

public static class FixedPointIteration
{
	private static double? Solve(Func<double, double> g, double x0, double epsilon)
	{
		var x1 = x0;
		Console.WriteLine($"x0 = {x1:F17};");
		var i = 1;
		var prevD = double.PositiveInfinity;
		while (true)
		{
			var x2 = g(x1);
			var d = Math.Abs(x2 - x1);
			Console.WriteLine($"x{i} = {x2:F17}; d = {d:F17}");
			if (prevD < d) return null;
			prevD = d;
			if (d < epsilon) return x2;
			x1 = x2;
			i++;
		}
	}

	public static void Run() 
	{
		(string, Func<double, double>)[] gList = new (string, Func<double, double>)[]
		{
			("g1(x) = tg(1/(3 * x^3))", x => Math.Tan(1d/(3d*Math.Pow(x, 3d)))),
			("g2(x) = (1/(3 * arctg(x)))^1/3", x => Math.Pow(1d / (3d * Math.Atan(x)), 1d / 3d)),
		};
		foreach (var (descr, fn) in gList)
		{
			Console.WriteLine(descr);
			var x = Solve(fn, 0.5, 0.001);
			if (x != null)
			{
				Console.WriteLine($"Solution: x = {x:F17}");
				break;
			}
			Console.WriteLine("The function does not converge.\n");
		}
	}
}
