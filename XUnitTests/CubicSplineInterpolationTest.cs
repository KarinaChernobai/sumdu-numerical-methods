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
	public void InterpolateTest()
	{
		var samples = new Point2D[] { new(0, 0), new(1, 10) };
		var spline = new CubicSplineInterpolation(samples);
		var x = 0.5d;
		var res = spline.Interpolate(x);
		Assert.Equal(5d, res, Tolerance);
	}

	[Fact]
	public void InterpolateTest2()
	{
		var samples = new Point2D[]
		{
			new(3, 21),
			new(9, -3),
		};
		var spline = new CubicSplineInterpolation(samples);
		var x = 6d;
		var res = spline.Interpolate(x);
		Assert.Equal(9d, res, Tolerance);
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
		//var x = 1.5d;
		var x = 2.8d;
		var res = spline.Interpolate(x);
		Assert.Equal(1.472, res, Tolerance);
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
		Assert.Equal(-0.5, res, 0.01);
	}
}
