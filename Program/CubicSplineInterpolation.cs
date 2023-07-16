using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods;

public class CubicSplineInterpolation
{
	public struct Coeff
	{
		public double A;
		public double B;
		public double C;
		public double D;
	}
	public struct TridiagonalData
	{
		public TridiagonalData(double l, double m, double u, double r) => (Lower, Main, Upper, Res) = (l, m, u, r);
		public double Lower;
		public double Main;
		public double Upper;
		public double Res;
	}
	public Coeff[] Coeffs;
	private readonly Point2D[] _samples;
	public CubicSplineInterpolation(Point2D[] samples) 
	{
		_samples = samples;
		Coeffs = new Coeff[_samples.Length];
	}

	public void TridiagonalMatrix(TridiagonalData[] data) 
	{
		CheckMainDiagonal(data);
		for (int i = 1; i < data.Length; i++) 
		{ 
			var w = data[i].Lower/data[i-1].Main;
			data[i].Main -= w * data[i - 1].Upper;
			data[i].Res -= w * data[i - 1].Res;
		}
		Coeffs[data.Length - 1].B = data[data.Length - 1].Res / data[data.Length - 1].Main;
		for(int i = data.Length - 2; i >= 0; i--) 
		{
			Coeffs[i].B = ( data[i].Res - data[i].Upper * Coeffs[i + 1].B ) / data[i].Main;
		}
	}

	private static void CheckMainDiagonal(TridiagonalData[] data) 
	{
		var mainDiagonal = 0d;
		for (int i = 0; i < data.Length; i++)
		{
			mainDiagonal += data[i].Main;
		}
		var sum = data[0].Main + data[0].Upper;
		if (sum >= mainDiagonal) throw new MatrixException("Main diagonal doesn't dominate row 0.");
		sum = data[1].Lower + data[0].Main;
		if (sum >= mainDiagonal) throw new MatrixException("Main diagonal doesn't dominate column 0.");
		var lastIndex = data.Length - 1;
		for (int i = 1; i < lastIndex; i++)
		{
			sum = data[i].Lower + data[i].Main + data[i].Upper;
			if (sum >= mainDiagonal) throw new MatrixException($"Main diagonal doesn't dominate row {i}.");
			sum = data[i + 1].Lower + data[i].Main + data[i - 1].Upper;
			if (sum >= mainDiagonal) throw new MatrixException($"Main diagonal doesn't dominate column {i}.");
		}
		sum = data[lastIndex].Lower + data[lastIndex].Main;
		if (sum >= mainDiagonal) throw new MatrixException($"Main diagonal doesn't dominate row {lastIndex}.");
		sum = data[lastIndex].Main + data[lastIndex - 1].Upper;
		if (sum >= mainDiagonal) throw new MatrixException($"Main diagonal doesn't dominate column {lastIndex}.");
	}


	public double Interpolate(double x) 
	{
		var res = 0d;
		return res;
	}
}

public class MatrixException : Exception 
{
	public MatrixException(string message) : base(message) { }
}
