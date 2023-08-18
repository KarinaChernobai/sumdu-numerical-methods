using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods;
// the chart
// y = a0 + a1 * x
// y = a0 + a1 * ln(x)
// y = a0 + a1 / x
// y = a0 * a1 ^ x
// y = a0 * x ^ a1
// y = e ^ a0 + a1 / x
// y = 1 / ( a0 + a1 * x)
// y = 1 / ( a0 + a1 * ln(x) )
// y = x / ( a0 + a1 * x)

public class LeastSquares
{
	private int _regrFnNum;
	public struct Point2D
	{
		public Point2D(double x, double y) => (X, Y) = (x, y);
		public double X;
		public double Y;
	}

	public struct RegrFunc
	{
		public RegrFunc(double x, double y) => (meanX, meanY) = (x, y);
		public readonly double meanX;
		public readonly double meanY;
		public double regrY;
		public double regrExpr;
	}

	private Point2D[] _samples;
	// private RegrFunc[] _regrFns; 
	public LeastSquares(Point2D[] samples)
	{
		_samples = samples;
	}

	// write the functions in comments
	public int FindRegrFunction()
	{
		var n = _samples.Length;

		var arithMeanX = 0d;
		var geomMeanX = 1d;
		var harmMeanX = 0d;

		var arithMeanY = 0d;
		var geomMeanY = 1d;
		var harmMeanY = 0d;


		for (int i = 0; i < n; i++)
		{
			arithMeanX += _samples[i].X;
			arithMeanY += _samples[i].Y;
			geomMeanX *= _samples[i].X;
			geomMeanY *= _samples[i].Y;
			harmMeanX += 1 / _samples[i].X;
			harmMeanY += 1 / _samples[i].Y;
		}
		arithMeanX /= n;
		arithMeanY /= n;
		geomMeanX = Math.Pow(geomMeanX, 1.0 / n);
		geomMeanY = Math.Pow(geomMeanY, 1.0 / n);
		harmMeanX = n / harmMeanX;
		harmMeanY = n / harmMeanY;

		var fns = new RegrFunc[]
		{
			new(arithMeanX, arithMeanY),
			new(geomMeanX, arithMeanY),
			new(harmMeanX, arithMeanY),
			new(arithMeanX, geomMeanY),
			new(geomMeanX, geomMeanY),
			new(harmMeanX, geomMeanY),
			new(arithMeanX, harmMeanY),
			new(geomMeanX, harmMeanY),
			new(harmMeanX, harmMeanY)
		};

		var j1 = 0;
		var j2 = 0;
		var j3 = 0;
		for (int i = 1; i < n; i++)
		{
			var x = _samples[i].X;
			if (x > arithMeanX)
			{
				j3 = i - 1;
				break;
			}
			if (x > geomMeanX) j2 = i - 1;
			else if (x > harmMeanX) j1 = i - 1;
		}

		for (int i = 0; i < fns.Length; i += 3)
		{
			fns[i].regrY = _samples[j3].Y + ((_samples[j3 + 1].Y - _samples[j3].Y) / (_samples[j3 + 1].X - _samples[j3].X)) * (fns[i].meanX - _samples[j3].X);
		}
		for (int i = 1; i < fns.Length; i += 3)
		{
			fns[i].regrY = _samples[j2].Y + ((_samples[j2 + 1].Y - _samples[j2].Y) / (_samples[j2 + 1].X - _samples[j2].X)) * (fns[i].meanX - _samples[j2].X);
		}
		for (int i = 2; i < fns.Length; i += 3)
		{
			fns[i].regrY = _samples[j1].Y + ((_samples[j1 + 1].Y - _samples[j1].Y) / (_samples[j1 + 1].X - _samples[j1].X)) * (fns[i].meanX - _samples[j1].X);
		}

		fns[0].regrExpr = Math.Abs((fns[0].meanY - fns[0].regrY) / fns[0].regrY);
		var minRegrY = fns[0].regrExpr;
		var minInx = 0;
		for (int i = 1; i < fns.Length; i++)
		{
			fns[i].regrExpr = Math.Abs((fns[i].meanY - fns[i].regrY) / fns[i].regrY);
			if (fns[i].regrExpr < minRegrY)
			{
				minRegrY = fns[i].regrExpr;
				minInx = i;
			}
		}
		Console.WriteLine($"Function index is {minInx + 1}");
		_regrFnNum = minInx;
		return minInx;
	}

	public void Linearize(int num) 
	{
		switch(num)
		{
			case 0:
				return;
			case 1:
				for (int i = 0; i < _samples.Length; i++)
				{
					_samples[i].X = Math.Log(_samples[i].X);
				}
				break;
			case 2:
				for (int i = 0; i < _samples.Length; i++)
				{
					_samples[i].X = 1 / _samples[i].X;
				}
				break;
			case 3:
				for (int i = 0; i < _samples.Length; i++)
				{
					_samples[i].Y = Math.Log(_samples[i].Y);
				}
				break;
			case 4:
				for (int i = 0; i < _samples.Length; i++)
				{
					_samples[i].X = Math.Log(_samples[i].X);
					_samples[i].Y = Math.Log(_samples[i].Y);
				}
				break;
			case 5:
				for (int i = 0; i < _samples.Length; i++)
				{
					_samples[i].X = 1 / _samples[i].X;
					_samples[i].Y = Math.Log(_samples[i].Y);
				}
				break;
			case 6:
				for (int i = 0; i < _samples.Length; i++)
				{
					_samples[i].Y = 1 / _samples[i].Y;
				}
				break;
			case 7:
				for (int i = 0; i < _samples.Length; i++)
				{
					_samples[i].X = Math.Log(_samples[i].X);
					_samples[i].Y = 1 / _samples[i].Y;
				}
				break;
			case 8:
				for (int i = 0; i < _samples.Length; i++)
				{
					_samples[i].X = 1 / _samples[i].X;
					_samples[i].Y = 1 / _samples[i].Y;
				}
				break;
		}
	}

	// print regr fn
	public (double, double) LinearRegrCoeffs(int num)
	{
		var n = _samples.Length;

		var sumX = 0d; // 144
		var sumY = 0d; // 422
		var sumSqrtX = 0d; // 3236
		var sumSqrtY = 0d;
		var sumProdXY = 0d; // 8994

		for (int i = 0; i < n; i++)
		{
			sumX += _samples[i].X;
			sumY += _samples[i].Y;
			sumSqrtX += _samples[i].X * _samples[i].X;
			sumSqrtY += _samples[i].Y * _samples[i].Y;
			sumProdXY += _samples[i].X * _samples[i].Y;
		}

		var (a0, a1) = SolveEq(n, sumX, sumX, sumSqrtX, sumY, sumProdXY);
		Console.WriteLine($"a0 = {a0} a1 = {a1}");

		switch (num)
		{
			case 3:
				a0 = Math.Pow(Math.E, a0);
				a1 = Math.Pow(Math.E, a1);
				break;
			case 4:
				a0 = Math.Pow(Math.E, a0);
				break;
			case 8:
				a0 = a1 + a0;
				a1 = a0 - a1;
				a0 = a0 - a1;
				break;
		}
		// коефіцієнта детермінації
		var r = a1 * (sumProdXY - (sumX * sumY) / n) / (sumSqrtY - (sumY * sumY) / n );
		Console.WriteLine($"R^2 = {r}");
		// критерій Фішера
		var f = ( r / (1 - r) ) * ( (n - 1 - 1) / 1);
		Console.WriteLine($"F = {f}");
		return (a0, a1);
	}

	public (double, double) SolveEq(double a, double b, double c, double d, double res1, double res2) 
	{
		var det = a * d - b * c;
		var det1 = res1 * d - b * res2;
		var det2 = a * res2 - c * res1;
		var x1 = det1 / det;
		var x2 = det2 / det;
		return (x1, x2);
	}
}
