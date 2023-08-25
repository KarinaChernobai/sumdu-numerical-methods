using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods;

public static class Integral
{
	public static double IntegrateSimpson(double a, double b, double epsilon, Func<double, double> f)
	{
		var n = 4;
		var step = (b - a) / n;
		var diff = epsilon + 1;
		var I1 = default(double);
		var I2 = default(double);
		while (diff > epsilon) 
		{
			I1 = Calculate(n, step, a, b, f);
			n *= 2;
			step /= 2;
			I2 = Calculate(n, step, a, b, f);
			diff = Math.Abs(I1 - I2);
		}
		return I2;
	}

	private static double Calculate(double n, double step, double a, double b, Func<double, double> f) => (step * (f(a) + f(b) + 4 * SumOdd(n, step, a, f) + 2 * SumEven(n, step, a, f))) / 3;

	private static double SumOdd(double n, double step, double a, Func<double, double> f) 
	{
		var res = default(double);
		for (var k = 1; k < a * n / 2; k++) 
		{
			res += f(a + (2 * k - 1) * a * step);
		}
		return res;
	}

	private static double SumEven(double n, double step, double a, Func<double, double> f)
	{
		var res = default(double);
		for (var k = 2; k < a * n / 2; k++)
		{
			res += f(a + (2 * k - 2) * a * step);
		}
		return res;
	}
}
