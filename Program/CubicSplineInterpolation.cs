using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods;

public class CubicSplineInterpolation
{
	private const double Tolerance = 0.000001;
	public struct Coeff
	{
		public double A;
		public double B;
		public double C;
	}
	public struct TridiagonalData
	{
		public TridiagonalData(double l, double m, double u, double r) => (Lower, Main, Upper, Res) = (l, m, u, r);
		public double Lower;
		public double Main;
		public double Upper;
		public double Res;
	}

	private readonly Coeff[] _coeffs;
	private readonly Point2D[] _samples;

	private CubicSplineInterpolation(int len)
	{
		_coeffs = new Coeff[len];
		_samples = Array.Empty<Point2D>();
	}

	public CubicSplineInterpolation(Point2D[] samples) 
	{
		_samples = samples;
		_coeffs = new Coeff[_samples.Length];
		var data = new TridiagonalData[_samples.Length];
		var n1 = data.Length - 1;
		var n2 = data.Length - 2;
		data[0].Main = 1;
		for (int i = 1; i <= n1; i++)
		{
			data[i].Lower = _samples[i].X - _samples[i - 1].X;
		}
		for (int i = 1; i < n2; i++)
		{
			data[i].Main = 2 * ( data[i + 1].Lower + data[i].Lower );
			data[i].Upper = data[i + 1].Lower;
			data[i].Res = 3 * ( ( _samples[i + 2].Y - _samples[i + 1].Y ) / data[i + 2].Lower - (samples[i + 1].Y - samples[i].Y) / data[i + 1].Lower );
		}
		data[n1].Main = 1;
		TridiagonalMatrix(data);
		for (int i = 0; i < n1; i++)
		{
			_coeffs[i].A = (_coeffs[i + 1].B - _coeffs[i].B) / (3 * data[i + 1].Lower);
			_coeffs[i].C = (_samples[i + 1].Y - _samples[i].Y) / data[i + 1].Lower - ((2 * _coeffs[i].B + _coeffs[i + 1].B) / 3) * data[i + 1].Lower;
		}
	}

	private void TridiagonalMatrix(TridiagonalData[] data) 
	{
		//CheckMainDiagonal(data);
		for (int i = 1; i < data.Length; i++) 
		{ 
			var w = data[i].Lower/data[i-1].Main;
			data[i].Main -= w * data[i - 1].Upper;
			if (Eq(data[i].Main, 0)) throw new MatrixException($"Unable to use Tridiagonal Thomas algorithm: zero element in the main diagonal at {i}.");
			data[i].Res -= w * data[i - 1].Res;
		}
		_coeffs[data.Length - 1].B = data[data.Length - 1].Res / data[data.Length - 1].Main;
		for(int i = data.Length - 2; i >= 0; i--) 
		{
			_coeffs[i].B = ( data[i].Res - data[i].Upper * _coeffs[i + 1].B ) / data[i].Main;
		}
	}

	public double Interpolate(double x, int i)
	{
		return _coeffs[i].A * Math.Pow(x - _samples[i].X, 3) + _coeffs[i].B * Math.Pow(x - _samples[i].X, 2) + _coeffs[i].C * (x - _samples[i].X) + _samples[i].Y;
	}

	private static bool Eq(double v1, double v2) => Math.Abs(v1 - v2) < Tolerance;

	public double Interpolate(double x) 
	{
		if (x < _samples[0].X || x > _samples[_samples.Length - 1].X) throw new ArgumentOutOfRangeException(nameof(x), "x is out of range.");
		for (int i = 0; i < _samples.Length; i++)
		{
			if (Eq(x, _samples[i].X)) return _samples[i].Y;
			if (x > _samples[i + 1].X) continue;
			return Interpolate(x, i);
		}
		throw new Exception("Internal Error.");
	}

	public readonly struct TestAccess
	{
		public static Coeff[] TridiagonalMatrix(TridiagonalData[] data)
		{
			var inst = new CubicSplineInterpolation(data.Length);
			inst.TridiagonalMatrix(data);
			return inst._coeffs;
		}
	}
}

public class MatrixException : Exception 
{
	public MatrixException(string message) : base(message) { }
}
