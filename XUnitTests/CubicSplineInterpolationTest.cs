using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static NumMethods.CubicSplineInterpolation;

namespace NumMethods;

public class CubicSplineInterpolationTest
{
	private const double Tolerance = 0.000001;

	[Fact]
	public void TridiagonalMatrixTest()
	{
		var  data = new TridiagonalData[] { new(0, 1, 0, 10), new(0, 1, 0, 20), new (0, 1, 0, 30) };
		var coeffs = CubicSplineInterpolation.TestAccess.TridiagonalMatrix(data);
		Assert.Equal(10, coeffs[0].B, Tolerance);
		Assert.Equal(20, coeffs[1].B, Tolerance);
		Assert.Equal(30, coeffs[2].B, Tolerance);
	}

	[Fact]
	public void TridiagonalMatrixTest2()
	{
		var data = new TridiagonalData[] { new(0, 2, 1, 40), new(1, 2, 1, 55), new(1, 1, 0, 25) };
		var coeffs = CubicSplineInterpolation.TestAccess.TridiagonalMatrix(data);
		Assert.Equal(10, coeffs[0].B, Tolerance);
		Assert.Equal(20, coeffs[1].B, Tolerance);
		Assert.Equal(5, coeffs[2].B, Tolerance);
	}

	[Fact]
	public void TridiagonalMatrixTest3()
	{
		var data = new TridiagonalData[] { new(0, 1, 1, 30), new(1, 2, 1, 55), new(1, 1, 0, 25) };
		//var coeffs = CubicSplineInterpolation.TestAccess.TridiagonalMatrix(data);
		Assert.Throws<MatrixException>(() => CubicSplineInterpolation.TestAccess.TridiagonalMatrix(data));
	}

	[Fact]
	public void InterpolateTest3()
	{
		var samples = new Point2D[]
		{
			new(0, 0),
			new(1, 0.5),
			new(2, 2),
			new(3, 1.5),
		};
		var spline = new CubicSplineInterpolation(samples);
		var testAccess = new TestAccess(spline);
		for (int i = 0; i < samples.Length - 1; i++) 
		{
			var res = spline.Interpolate(samples[i].X, i);
			Assert.Equal(samples[i].Y, res, Tolerance);
			res = spline.Interpolate(samples[i + 1].X, i);
			Assert.Equal(samples[i + 1].Y, res, Tolerance);
		}

		Assert.Equal(0, testAccess.Derivative2(samples[0].X, 0), Tolerance);
		Assert.Equal(0, testAccess.Derivative2(samples[samples.Length - 1].X, samples.Length - 2), Tolerance);
		for (int i = 1; i < samples.Length - 1; i++) 
		{
			var der1 = testAccess.Derivative1(samples[i].X, i - 1);
			var der2 = testAccess.Derivative1(samples[i].X, i);
			Assert.True(Math.Abs(der1 - der2) < Tolerance, $"i = {i}, der1 = {der1}, der2 = {der2}");

			der1 = testAccess.Derivative2(samples[i].X, i - 1);
			der2 = testAccess.Derivative2(samples[i].X, i);
			Assert.True(Math.Abs(der1 - der2) < Tolerance);
		}
	}

	[Fact]
	public void InterpolateTest4()
	{
		var samples = new Point2D[]
		{
			new(0.4, -0.916291),
			new(0.5, -0.693147),
			new(0.7, -0.356675),
			new(0.8, -0.223144),
		};
		var spline = new CubicSplineInterpolation(samples);
		var x = 0.6d;
		var res = spline.Interpolate(x);
		Assert.Equal(-0.509976, res, 0.01);
	}
}
